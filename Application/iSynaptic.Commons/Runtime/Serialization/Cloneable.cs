using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;

namespace iSynaptic.Commons.Runtime.Serialization
{
    public static class Cloneable<T>
    {
        private static Type[] _FixedCloneablePrimitive =
        {
            typeof(string),
            typeof(decimal),
            typeof(DateTime),
            typeof(TimeSpan),
            typeof(Guid)
        };

        private static Type _TargetType = null;
        private static Func<T, IDictionary<object, object>, T> _CloneHandler = null;

        static Cloneable()
        {
            _TargetType = typeof(T);
        }

        public static bool CanClone()
        {
            return CanClone(_TargetType);
        }

        private static MethodInfo GetMethod(Type type, string methodName, params Type[] argumentTypes)
        {
            BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            if (argumentTypes != null && argumentTypes.Length > 0 && argumentTypes[0] == typeof(void))
            {
                return type.GetMethod(methodName, bindingFlags);
            }
            else
            {
                return type.GetMethod
                (
                    methodName,
                    bindingFlags,
                    null,
                    argumentTypes,
                    null
                );
            }
        }

        private static bool CanClone(Type type)
        {
            if (type.IsDefined(typeof(NonSerializedAttribute), true))
                return false;

            if (IsNotCloneable(type))
                return false;

            if (IsCloneablePrimitive(type))
                return true;

            if (type.IsArray)
                return CanClone(GetUnderlyingArrayType(type));

            Predicate<FieldInfo> whereFilter = f =>
                (f.FieldType != type) &&
                (f.IsDefined(typeof(NonSerializedAttribute), true) != true) &&
                (f.FieldType.IsDefined(typeof(NonSerializedAttribute), true) != true);

            foreach (FieldInfo field in GetFields(type, whereFilter))
            {
                if (IsNotCloneable(field.FieldType))
                    return false;

                if (IsCloneablePrimitive(field.FieldType))
                    continue;

                if (type.IsArray)
                {
                    if (CanClone(GetUnderlyingArrayType(field.FieldType)))
                        continue;
                    else
                        return false;
                }

                Type fieldClonableType = typeof(Cloneable<>).MakeGenericType(field.FieldType);
                MethodInfo canCloneMethod = GetMethod(fieldClonableType, "CanClone");
                Func<bool> canClone = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), canCloneMethod);

                if (canClone())
                    continue;
                else
                    return false;
            }

            return true;
        }

        private static Type GetUnderlyingArrayType(Type type)
        {
            Type currentType = type;

            while (currentType.IsArray)
                currentType = currentType.GetElementType();

            return currentType;
        }

        private static IEnumerable<FieldInfo> GetFields(Type type)
        {
            return GetFields(type, null);
        }

        private static IEnumerable<FieldInfo> GetFields(Type type, Predicate<FieldInfo> whereFilter)
        {
            Type currentType = type;
            while (currentType != null)
            {
                foreach (FieldInfo info in currentType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (whereFilter == null)
                        yield return info;
                    else if (whereFilter(info))
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

        private static bool IsCloneablePrimitive(Type inputType)
        {
            if(Array.Exists(_FixedCloneablePrimitive, x => x == inputType))
                return true;

            if (inputType.IsPrimitive)
                return true;

            return false;
        }

        private static Func<T, IDictionary<object, object>, T> BuildCloneHandler(Type type)
        {
            MethodInfo getTypeFromHandlerMethod = GetMethod(typeof(Type), "GetTypeFromHandle", typeof(RuntimeTypeHandle));
            MethodInfo getSafeUninitializedObjectMethod = GetMethod(typeof(FormatterServices), "GetSafeUninitializedObject", typeof(Type));

            Type dictionaryType = typeof(IDictionary<,>).MakeGenericType(typeof(object), typeof(object));

            MethodInfo mapAddMethod = GetMethod(dictionaryType, "Add", typeof(object), typeof(object));
                
            DynamicMethod cloneMethod = new DynamicMethod(string.Format("Cloneable<{0}>_Clone", type.Name), type, new Type[] { type, dictionaryType }, type, true);
            ILGenerator gen = cloneMethod.GetILGenerator();

            gen.DeclareLocal(type);

            gen.Emit(OpCodes.Ldtoken, type);
            gen.Emit(OpCodes.Call, getTypeFromHandlerMethod);
            gen.Emit(OpCodes.Call, getSafeUninitializedObjectMethod);
            gen.Emit(OpCodes.Castclass, type);
            gen.Emit(OpCodes.Stloc_0);

            if (!type.IsValueType)
            {
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldloc_0);

                gen.Emit(OpCodes.Call, mapAddMethod);
            }

            Predicate<FieldInfo> whereFilter = f =>
                (f.IsDefined(typeof(NonSerializedAttribute), true) != true) &&
                (f.FieldType.IsDefined(typeof(NonSerializedAttribute), true) != true);

            foreach (FieldInfo field in GetFields(type, whereFilter))
            {
                if (IsCloneablePrimitive(field.FieldType))
                {
                    gen.Emit(OpCodes.Ldloc_0);
                    gen.Emit(OpCodes.Ldarg_0);
                    gen.Emit(OpCodes.Ldfld, field);
                    gen.Emit(OpCodes.Stfld, field);
                }
                else
                {
                    gen.Emit(OpCodes.Ldloc_0);
                    gen.Emit(OpCodes.Ldarg_0);
                    gen.Emit(OpCodes.Ldfld, field);
                    gen.Emit(OpCodes.Ldarg_1);

                    Type fieldClonableType =typeof(Cloneable<>).MakeGenericType(field.FieldType);

                    MethodInfo childCloneMethod = GetMethod(fieldClonableType, "Clone", field.FieldType, dictionaryType);

                    gen.Emit(OpCodes.Call, childCloneMethod);
                    gen.Emit(OpCodes.Stfld, field);
                }
            }

            gen.Emit(OpCodes.Ldloc_0);
            gen.Emit(OpCodes.Ret);

            return (Func<T, IDictionary<object, object>, T>)cloneMethod.CreateDelegate(typeof(Func<T, IDictionary<object, object>, T>));
        }

        private static Func<T, IDictionary<object, object>, T> BuildArrayCloneHandler(Type itemType)
        {
            Type clonableType = typeof(Cloneable<T>);
            MethodInfo info = GetMethod(clonableType, "ArrayClone", typeof(T), typeof(IDictionary<object, object>));

            info = info.MakeGenericMethod(itemType);

            return (Func<T, IDictionary<object, object>, T>)Delegate.CreateDelegate(typeof(Func<T, IDictionary<object, object>, T>), info);
        }

        public static T Clone(T source)
        {
            Dictionary<object, object> map = new Dictionary<object, object>();

            return Clone(source, map);
        }

        private static T Clone(T source, IDictionary<object, object> map)
        {
            if (source == null)
                return default(T);

            if (typeof(T).IsValueType != true && map.ContainsKey(source))
                return (T)map[source];

            if (_CloneHandler == null)
            {
                if (CanClone())
                {
                    if (IsCloneablePrimitive(_TargetType))
                        _CloneHandler = (s, m) => s;
                    else if (_TargetType.IsArray)
                        _CloneHandler = BuildArrayCloneHandler(_TargetType.GetElementType());
                    else
                        _CloneHandler = BuildCloneHandler(_TargetType);
                }
                else
                    _CloneHandler = (s, m) => { throw new InvalidOperationException("This type cannot be cloned."); };
            }

            return _CloneHandler(source, map);
        }

        private static T ArrayClone<U>(T source, IDictionary<object, object> map)
        {
            if (source == null)
                return default(T);

            if (map.ContainsKey(source))
                return (T)map[source];

            Array sourceArray = (Array)(object)source;
            ArrayIndex index = new ArrayIndex(sourceArray);

            Array destArray = (Array)sourceArray.Clone();

            map.Add(source, (T)(object)destArray);

            if (sourceArray.Length <= 0)
                return (T)(object)destArray;

            while (true)
            {
                U item = (U)sourceArray.GetValue(index);
                destArray.SetValue(Cloneable<U>.Clone(item, map), index);

                if (index.CanIncrement())
                    index.Increment();
                else
                    break;
            }

            return (T)(object)destArray;
        }
    }
}
