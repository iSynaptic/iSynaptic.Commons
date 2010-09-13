using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;

using iSynaptic.Commons.Extensions;
using System.Security.Permissions;

namespace iSynaptic.Commons.Runtime.Serialization
{
    public static class Cloneable
    {
        public static T Clone<T>(this T source)
        {
            return Cloneable<T>.Clone(source);
        }

        public static T ShallowClone<T>(this T source)
        {
            return Cloneable<T>.ShallowClone(source);
        }

        public static void CloneTo<T>(this T source, T destination)
            where T : class
        {
            Cloneable<T>.CloneTo(source, destination);
        }

        public static void ShallowCloneTo<T>(this T source, T destination)
            where T : class
        {
            Cloneable<T>.ShallowCloneTo(source, destination);
        }
    }

    public static class Cloneable<T>
    {
        private static Type[] _CloneablePrimitives =
        {
            typeof(string),
            typeof(decimal),
            typeof(DateTime),
            typeof(TimeSpan),
            typeof(Guid)
        };

        private static readonly Type _TargetType = null;
        private static readonly object _SyncLock = null;

        private static bool? _CanClone = null;
        private static bool? _CanShallowClone = null;

        private static Func<T, T, bool, IDictionary<object, object>, T> _Strategy = null;

        private static readonly Predicate<FieldInfo> _FieldIncludeFilter = f =>
            (f.IsDefined(typeof(NonSerializedAttribute), true) != true);

        static Cloneable()
        {
            _TargetType = typeof(T);
            _SyncLock = new object();
        }

        #region Helper Methods

        private static Func<T, T, bool, IDictionary<object, object>, T> BuildStrategy()
        {
            bool canShallowClone = CanShallowClone();
            bool canClone = CanClone();

            bool isTargetTypeArray = _TargetType.IsArray;

            Func<T, T, bool, IDictionary<object, object>, T> dynamicStrategy = null;

            return (s, d, isShallow, m) =>
            {
                if (canShallowClone != true || (isShallow != true && canClone != true))
                    throw new InvalidOperationException("This type cannot be cloned.");

                if (IsRootTypeCloneablePrimitive(_TargetType) && isTargetTypeArray != true)
                    return s;

                if (s == null)
                    return default(T);

                if (isTargetTypeArray && isShallow)
                {
                    if (s == null)
                        return default(T);

                    Array sourceArray = s as Array;
                    return (T)sourceArray.Clone();
                }

                if (dynamicStrategy == null)
                    dynamicStrategy = BuildDynamicStrategy();

                return dynamicStrategy(s, d, isShallow, m ?? new Dictionary<object, object>());
            };
        }

        private static Func<T, T, bool, IDictionary<object, object>, T> BuildDynamicStrategy()
        {
            throw new NotImplementedException();
        }


        private static Type GetRootType(Type type)
        {
            Type currentType = type;

            while (currentType.IsArray)
                currentType = currentType.GetElementType();

            if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(Nullable<>))
                return currentType.GetGenericArguments()[0];

            return currentType;
        }

        private static IEnumerable<FieldInfo> GetFields(Type type, Predicate<FieldInfo> includeFilter)
        {
            Type currentType = type;
            while (currentType != null)
            {
                foreach (FieldInfo info in currentType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (includeFilter != null && includeFilter(info) != true)
                        continue;

                    yield return info;
                }

                if (currentType.BaseType != null)
                    currentType = currentType.BaseType;
                else
                    currentType = null;
            }
        }

        private static bool IsNotCloneable(Type inputType)
        {
            if (typeof(Delegate).IsAssignableFrom(inputType))
                return true;

            if (inputType == typeof(IntPtr))
                return true;

            return false;
        }

        private static bool IsRootTypeCloneablePrimitive(Type inputType)
        {
            Type rootType = GetRootType(inputType);

            if (Array.Exists(_CloneablePrimitives, x => x == rootType))
                return true;

            if (rootType.IsPrimitive)
                return true;

            return false;
        }

        private static MethodInfo GetMethod(Type type, string methodName, params Type[] argumentTypes)
        {
            BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            return type.GetMethod
            (
                methodName,
                bindingFlags,
                null,
                argumentTypes,
                null
            );
        }

        #endregion

        #region Can Methods

        private static bool CanClone(Type type, bool isShallow)
        {
            Type typeToCheck = GetRootType(type);

            if (IsNotCloneable(typeToCheck))
                return false;

            if (IsRootTypeCloneablePrimitive(typeToCheck))
                return true;

            Predicate<FieldInfo> includeFilter = _FieldIncludeFilter.And(f => f.FieldType != typeToCheck);

            foreach (FieldInfo field in GetFields(typeToCheck, includeFilter))
            {
                Type fieldType = GetRootType(field.FieldType);

                if (IsNotCloneable(fieldType))
                    return false;

                if (IsRootTypeCloneablePrimitive(fieldType))
                    continue;

                if (isShallow != true)
                {
                    Type fieldClonableType = typeof(Cloneable<>).MakeGenericType(fieldType);
                    MethodInfo canCloneMethod = GetMethod(fieldClonableType, "CanClone");
                    var canClone = canCloneMethod.ToFunc<bool>();

                    if (canClone() != true)
                        return false;
                }
            }

            return true;
        }

        public static bool CanClone()
        {
            if (_CanClone.HasValue != true)
                _CanClone = CanClone(_TargetType, false);

            return _CanClone.Value;
        }

        public static bool CanShallowClone()
        {
            if (_CanShallowClone.HasValue != true)
                _CanShallowClone = CanClone(_TargetType, true);

            return _CanShallowClone.Value;
        }

        #endregion

        #region Clone Methods

        [ReflectionPermission(SecurityAction.Demand, ReflectionEmit = true)]
        public static T Clone(T source)
        {
            return Clone(source, null);
        }

        private static T Clone(T source, IDictionary<object, object> map)
        {
            return Strategy(source, default(T), false, map);
        }

        public static T ShallowClone(T source)
        {
            return Strategy(source, default(T), true, null);
        }

        public static void CloneTo(T source, T destination)
        {
            if(_TargetType.IsValueType)
                throw new InvalidOperationException("CloneTo only works on reference types.");

            if(source == null)
                throw new ArgumentNullException("source");

            if (destination == null)
                throw new ArgumentNullException("destination");

            if (ReferenceEquals(source, destination))
                throw new InvalidOperationException("The destination object cannot be the same as the source.");

            Dictionary<object, object> map = new Dictionary<object, object>();
            Strategy(source, default(T), false, null);
        }

        public static void ShallowCloneTo(T source, T destination)
        {
            if (_TargetType.IsValueType)
                throw new InvalidOperationException("ShallowCloneTo only works on reference types.");

            if (source == null)
                throw new ArgumentNullException("source");

            if (destination == null)
                throw new ArgumentNullException("destination");

            if(ReferenceEquals(source, destination))
                throw new InvalidOperationException("The destination object cannot be the same as the source.");

            Dictionary<object, object> map = new Dictionary<object, object>();
            Strategy(source, default(T), true, null);
        }

        private static T ArrayClone<U>(T source, T destination, IDictionary<object, object> map)
        {
            Array sourceArray = (Array)(object)source;
            ArrayIndex index = new ArrayIndex(sourceArray);

            Array destArray = (Array)(object)destination;

            if (sourceArray.Length <= 0)
                return destination;

            while (true)
            {
                U sourceItem = (U)sourceArray.GetValue(index);
                if (sourceItem == null)
                    destArray.SetValue(null, index);
                else
                    destArray.SetValue(Cloneable<U>.Clone(sourceItem, map), index);

                if (index.CanIncrement())
                    index.Increment();
                else
                    break;
            }

            return destination;
        }


        #endregion

        #region Build Handler Methods

        private static Func<T, IDictionary<object, object>, T> BuildCloneHandler(Type type)
        {
            bool isValueType = type.IsValueType;

            Type dictionaryType = typeof(IDictionary<,>).MakeGenericType(typeof(object), typeof(object));

            MethodInfo mapContainsKeyMethod = GetMethod(dictionaryType, "ContainsKey", typeof(object));
            MethodInfo mapGetItemMethod = GetMethod(dictionaryType, "get_Item", typeof(object));
            MethodInfo mapAddMethod = GetMethod(dictionaryType, "Add", typeof(object), typeof(object));

            DynamicMethod cloneMethod = new DynamicMethod(string.Format("Cloneable<{0}>_Clone", type.Name), type, new Type[] { type, dictionaryType }, type, true);
            ILGenerator gen = cloneMethod.GetILGenerator();

            Label checkMapLabel = gen.DefineLabel();
            Label beginCloningLabel = gen.DefineLabel();

            gen.DeclareLocal(type);

            if (isValueType != true)
            {
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Brtrue_S, checkMapLabel);
                gen.Emit(OpCodes.Ldnull);
                gen.Emit(OpCodes.Ret);

                gen.MarkLabel(checkMapLabel);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Call, mapContainsKeyMethod);
                gen.Emit(OpCodes.Brfalse_S, beginCloningLabel);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Call, mapGetItemMethod);
                gen.Emit(OpCodes.Ret);
            }

            gen.MarkLabel(beginCloningLabel);

            EmitInitializeObject(type, gen);

            if (isValueType != true)
            {
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldloc_0);

                gen.Emit(OpCodes.Call, mapAddMethod);
            }

            foreach (FieldInfo field in GetFields(type, _FieldIncludeFilter))
            {
                if (IsRootTypeCloneablePrimitive(field.FieldType) ||
                    (field.FieldType.IsValueType != true && field.IsDefined(typeof(CloneReferenceOnlyAttribute), true)))
                {
                    EmitCopyField(isValueType, gen, field);
                }
                else
                {
                    if (isValueType)
                    {
                        gen.Emit(OpCodes.Ldloca_S, (byte)0);
                        gen.Emit(OpCodes.Ldarga_S, (byte)0);
                    }
                    else
                    {
                        gen.Emit(OpCodes.Ldloc_0);
                        gen.Emit(OpCodes.Ldarg_0);
                    }

                    gen.Emit(OpCodes.Ldfld, field);
                    gen.Emit(OpCodes.Ldarg_1);

                    Type fieldClonableType = typeof(Cloneable<>).MakeGenericType(field.FieldType);

                    MethodInfo childCloneMethod = GetMethod(fieldClonableType, "Clone", field.FieldType, dictionaryType);

                    gen.Emit(OpCodes.Call, childCloneMethod);
                    gen.Emit(OpCodes.Stfld, field);
                }
            }

            gen.Emit(OpCodes.Ldloc_0);
            gen.Emit(OpCodes.Ret);

            return cloneMethod.ToFunc<T, IDictionary<object, object>, T>();
        }

        private static Func<T, IDictionary<object, object>, T> BuildArrayCloneHandler(Type itemType)
        {
            Type clonableType = typeof(Cloneable<T>);
            MethodInfo info = GetMethod(clonableType, "ArrayClone", typeof(T), typeof(IDictionary<object, object>));

            info = info.MakeGenericMethod(itemType);

            return info.ToFunc<T, IDictionary<object, object>, T>();
        }

        private static Func<T, T> BuildShallowCloneHandler(Type type)
        {
            bool isValueType = type.IsValueType;

            MethodInfo getTypeFromHandlerMethod = GetMethod(typeof(Type), "GetTypeFromHandle", typeof(RuntimeTypeHandle));
            MethodInfo getSafeUninitializedObjectMethod = GetMethod(typeof(FormatterServices), "GetSafeUninitializedObject", typeof(Type));
            MethodInfo referenceEquals = GetMethod(typeof(object), "ReferenceEquals", typeof(object), typeof(object));

            DynamicMethod cloneMethod = new DynamicMethod(string.Format("Cloneable<{0}>_ShallowClone", type.Name), type, new Type[] { type }, type, true);
            ILGenerator gen = cloneMethod.GetILGenerator();

            gen.DeclareLocal(type);

            if (isValueType != true)
            {
                Label continueLabel = gen.DefineLabel();
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Brtrue_S, continueLabel);
                gen.Emit(OpCodes.Ldnull);
                gen.Emit(OpCodes.Ret);
                gen.MarkLabel(continueLabel);
            }

            EmitInitializeObject(type, gen);

            foreach (FieldInfo field in GetFields(type, _FieldIncludeFilter))
            {
                if (isValueType != true && field.FieldType == type)
                {
                    Label copyField = gen.DefineLabel();
                    Label doneWithField = gen.DefineLabel();

                    gen.Emit(OpCodes.Ldarg_0);
                    gen.Emit(OpCodes.Ldarg_0);
                    gen.Emit(OpCodes.Ldfld, field);
                    gen.Emit(OpCodes.Call, referenceEquals);
                    gen.Emit(OpCodes.Brfalse, copyField);
                    gen.Emit(OpCodes.Ldloc_0);
                    gen.Emit(OpCodes.Ldloc_0);
                    gen.Emit(OpCodes.Stfld, field);
                    gen.Emit(OpCodes.Br, doneWithField);

                    gen.MarkLabel(copyField);
                    EmitCopyField(isValueType, gen, field);
                    gen.MarkLabel(doneWithField);
                }
                else
                    EmitCopyField(isValueType, gen, field);
            }

            gen.Emit(OpCodes.Ldloc_0);
            gen.Emit(OpCodes.Ret);

            return cloneMethod.ToFunc<T, T>();
        }

        private static void EmitInitializeObject(Type type, ILGenerator gen)
        {
            bool isValueType = type.IsValueType;

            MethodInfo getTypeFromHandlerMethod = GetMethod(typeof(Type), "GetTypeFromHandle", typeof(RuntimeTypeHandle));
            MethodInfo getSafeUninitializedObjectMethod = GetMethod(typeof(FormatterServices), "GetSafeUninitializedObject", typeof(Type));

            if (isValueType)
            {
                gen.Emit(OpCodes.Ldloca_S, (byte)0);
                gen.Emit(OpCodes.Initobj, type);
            }
            else
            {
                gen.Emit(OpCodes.Ldtoken, type);
                gen.Emit(OpCodes.Call, getTypeFromHandlerMethod);
                gen.Emit(OpCodes.Call, getSafeUninitializedObjectMethod);
                gen.Emit(OpCodes.Castclass, type);
                gen.Emit(OpCodes.Stloc_0);
            }
        }

        private static void EmitCopyField(bool isValueType, ILGenerator gen, FieldInfo field)
        {
            if (isValueType)
            {
                gen.Emit(OpCodes.Ldloca_S, (byte)0);
                gen.Emit(OpCodes.Ldarga_S, (byte)0);
            }
            else
            {
                gen.Emit(OpCodes.Ldloc_0);
                gen.Emit(OpCodes.Ldarg_0);
            }

            gen.Emit(OpCodes.Ldfld, field);
            gen.Emit(OpCodes.Stfld, field);
        }

        #endregion

        private static Func<T, T, bool, IDictionary<object, object>, T> Strategy
        {
            get
            {
                if (_Strategy == null)
                {
                    lock (_SyncLock)
                    {
                        if (_Strategy == null)
                        {
                            _Strategy = BuildStrategy();
                        }
                    }
                }

                return _Strategy;
            }
        }
    }
}
