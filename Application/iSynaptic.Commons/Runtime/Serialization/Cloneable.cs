using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Security.Permissions;
using iSynaptic.Commons.Reflection;
using iSynaptic.Commons.Reflection.Emit;

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

        public static T CloneTo<T>(this T source, T destination)
        {
            return Cloneable<T>.CloneTo(source, destination);
        }

        public static T ShallowCloneTo<T>(this T source, T destination)
        {
            return Cloneable<T>.ShallowCloneTo(source, destination);
        }
    }

    internal class CloneContext
    {
        private IDictionary<object, object> _CloneMap = null;

        public CloneContext(bool isShallowClone, bool shouldUseExistingObjects)
        {
            IsShallowClone = isShallowClone;
            ShouldUseExistingObjects = shouldUseExistingObjects;
        }

        public bool IsShallowClone { get; private set; }
        public bool ShouldUseExistingObjects { get; private set; }

        public IDictionary<object, object> CloneMap
        {
            get { return _CloneMap ?? (_CloneMap = new Dictionary<object, object>()); }
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
        private static readonly MethodInfo _CloneContextGetShouldUseExistingObjectsMethod = null;
        private static readonly MethodInfo _CloneContextGetIsShallowCloneMethod = null;
        private static readonly MethodInfo _CloneContextGetCloneMapMethod = null;
        private static readonly MethodInfo _DictionaryContainsKeyMethod = null;
        private static readonly MethodInfo _DictionaryGetItemMethod = null;

        private static readonly object _SyncLock = null;

        private static bool? _CanClone = null;
        private static bool? _CanShallowClone = null;

        private static Func<T, T, CloneContext, T> _Strategy = null;

        private static readonly Predicate<FieldInfo> _FieldIncludeFilter = f =>
            (f.IsDefined(typeof(NonSerializedAttribute), true) != true);

        static Cloneable()
        {
            _TargetType = typeof(T);
            _CloneContextGetShouldUseExistingObjectsMethod = GetMethod(typeof(CloneContext), "get_ShouldUseExistingObjects");
            _CloneContextGetIsShallowCloneMethod = GetMethod(typeof(CloneContext), "get_IsShallowClone");
            _CloneContextGetCloneMapMethod = GetMethod(typeof (CloneContext), "get_CloneMap");

            Type dictionaryType = typeof(IDictionary<,>).MakeGenericType(typeof(object), typeof(object));

            _DictionaryContainsKeyMethod = GetMethod(dictionaryType, "ContainsKey", typeof(object));
            _DictionaryGetItemMethod = GetMethod(dictionaryType, "get_Item", typeof(object));

            _SyncLock = new object();
        }

        #region Build Methods

        private static Func<T, T, CloneContext, T> BuildStrategy()
        {
            bool canShallowClone = CanShallowClone();
            bool canClone = CanClone();

            bool isTargetTypeArray = _TargetType.IsArray;
            bool isReferenceType = !_TargetType.IsValueType;
            bool isNullableType = _TargetType.IsGenericType &&
                                  _TargetType.GetGenericTypeDefinition() == typeof (Nullable<>);

            bool canReturnSourceAsClone = IsRootTypeCloneablePrimitive(_TargetType) && isTargetTypeArray != true;

            Func<T, T, CloneContext, T> dynamicStrategy = null;
            Func<Array, Array, CloneContext, T> arrayCloneStrategy = null;

            return (s, d, c) =>
            {
                if (canShallowClone != true || (c.IsShallowClone != true && canClone != true))
                    throw new InvalidOperationException("This type cannot be cloned.");

                if (canReturnSourceAsClone)
                    return s;

                if (s == null)
                    return default(T);

                if (isReferenceType && c.CloneMap.ContainsKey(s))
                    return (T) c.CloneMap[s];

                if (isTargetTypeArray)
                {
                    Array destArray = (Array) (object) d;
                    Array sourceArray = (Array) (object) s;

                    if (destArray == null || destArray.LongLength != sourceArray.LongLength)
                    {
                        d = (T) sourceArray.Clone();
                        destArray = (Array)(object)d;
                    }
                    else if (c.IsShallowClone && sourceArray.LongLength > 0)
                        Array.Copy(sourceArray, destArray, sourceArray.LongLength);

                    if (c.IsShallowClone || sourceArray.LongLength <= 0)
                    {
                        c.CloneMap.Add(s, d);
                        return d;
                    }

                    if(arrayCloneStrategy == null)
                    {
                        var arrayCloneMethod = GetMethod(typeof(Cloneable<T>),
                                                         "ArrayClone",
                                                         typeof (Array),
                                                         typeof (Array),
                                                         typeof (CloneContext));

                        arrayCloneMethod = arrayCloneMethod.MakeGenericMethod(_TargetType.GetElementType());
                        arrayCloneStrategy = arrayCloneMethod.ToFunc<Array, Array, CloneContext, T>();
                    }

                    d = arrayCloneStrategy(sourceArray, destArray, c);
                    c.CloneMap.Add(s, d);

                    return d;
                }

                if(d == null && (isReferenceType || isNullableType))
                    d = (T)FormatterServices.GetSafeUninitializedObject(_TargetType);

                if (dynamicStrategy == null)
                    dynamicStrategy = BuildDynamicStrategy();

                if(isReferenceType)
                    c.CloneMap.Add(s, d);

                return dynamicStrategy(s, d, c);
            };
        }

        private static Func<T, T, CloneContext, T> BuildDynamicStrategy()
        {
            string dynamicMethodName = string.Format("Cloneable<{0}>_CloneDynamicStrategy", _TargetType.Name);
            DynamicMethod dynamicStrategyMethod = new DynamicMethod(dynamicMethodName,
                                                          _TargetType,
                                                          new []
                                                          {
                                                              _TargetType, _TargetType, typeof(CloneContext)
                                                          },
                                                          _TargetType, true);


            ILGenerator gen = dynamicStrategyMethod.GetILGenerator();

            gen.DeclareLocal(_TargetType);

            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Stloc_0);

            foreach (FieldInfo field in GetFields(_TargetType, _FieldIncludeFilter))
            {
                if (field.IsDefined(typeof(CloneReferenceOnlyAttribute), true) ||
                    field.FieldType.IsDefined(typeof(CloneReferenceOnlyAttribute), true) ||
                    IsTypeCloneablePrimative(field.FieldType))
                {
                    EmitCopyField(gen, field);
                }
                else
                {
                    EmitCloneFieldWithShallowCheck(gen, field);
                }
            }

            gen.Emit(OpCodes.Ldloc_0);
            gen.Emit(OpCodes.Ret);

            return dynamicStrategyMethod.ToFunc<T, T, CloneContext, T>();
        }

        #endregion

        #region Emit Methods

        private static void EmitCopyField(ILGenerator gen, FieldInfo field)
        {
            var copyFieldLabel = gen.DefineLabel();
            var storeFieldLabel = gen.DefineLabel();

            // handles self-referencing fields
            if (field.FieldType.IsValueType != true && field.FieldType.IsAssignableFrom(_TargetType))
            {
                // check clone map
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldfld, field);
                gen.Emit(OpCodes.Brfalse_S, copyFieldLabel);

                gen.Emit(OpCodes.Ldarg_2);
                gen.Emit(OpCodes.Call, _CloneContextGetCloneMapMethod);

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldfld, field);

                gen.Emit(OpCodes.Callvirt, _DictionaryContainsKeyMethod);
                gen.Emit(OpCodes.Brfalse_S, copyFieldLabel);

                gen.Emit(OpCodes.Ldarg_2);
                gen.Emit(OpCodes.Call, _CloneContextGetCloneMapMethod);

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldfld, field);

                gen.Emit(OpCodes.Call, _DictionaryGetItemMethod);

                if (field.DeclaringType.IsValueType)
                    gen.Emit(OpCodes.Ldloca_S, (byte)0);
                else
                    gen.Emit(OpCodes.Ldloc_0);

                gen.Emit(OpCodes.Br_S, storeFieldLabel);
            }

            // copy field
            gen.MarkLabel(copyFieldLabel);
            if (field.DeclaringType.IsValueType)
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

            gen.MarkLabel(storeFieldLabel);
            gen.Emit(OpCodes.Stfld, field);
        }

        private static void EmitCloneFieldWithShallowCheck(ILGenerator gen, FieldInfo field)
        {
            var ifNotShallowCloneLabel = gen.DefineLabel();
            var afterDestinationCopied = gen.DefineLabel();

            gen.Emit(OpCodes.Ldarg_2);
            gen.Emit(OpCodes.Call, _CloneContextGetIsShallowCloneMethod);
            gen.Emit(OpCodes.Brfalse_S, ifNotShallowCloneLabel);

            EmitCopyField(gen, field);
            gen.Emit(OpCodes.Br_S, afterDestinationCopied);

            gen.MarkLabel(ifNotShallowCloneLabel);

            // used by final store field (Stfld) instruction
            if (field.DeclaringType.IsValueType)
                gen.Emit(OpCodes.Ldloca_S, (byte)0);
            else
                gen.Emit(OpCodes.Ldloc_0);

            EmitCloneField(gen, field);
            gen.Emit(OpCodes.Stfld, field);

            gen.MarkLabel(afterDestinationCopied);
        }

        private static void EmitCloneField(ILGenerator gen, FieldInfo field)
        {
            Type fieldClonableType = typeof(Cloneable<>).MakeGenericType(field.FieldType);
            MethodInfo getStrategyMethod = GetMethod(fieldClonableType, "get_Strategy");
            
            Type strategyMethodType = typeof(Func<,,,>).MakeGenericType(field.FieldType, field.FieldType, typeof(CloneContext), field.FieldType);
            MethodInfo getFuncMethod = GetMethod(strategyMethodType, "Invoke", field.FieldType, field.FieldType, typeof(CloneContext));

            gen.Emit(OpCodes.Call, getStrategyMethod);
            
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldfld, field);

            EmitLoadDestination(gen, field);

            gen.Emit(OpCodes.Ldarg_2);

            gen.Emit(OpCodes.Callvirt, getFuncMethod);
        }

        private static void EmitLoadDestination(ILGenerator gen, FieldInfo field)
        {
            if(field.FieldType.IsValueType)
            {
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldfld, field);
            }
            else
            {
                var ifShouldUseExistingLabel = gen.DefineLabel();
                var afterLoadingDestinationLabel = gen.DefineLabel();

                gen.Emit(OpCodes.Ldarg_2);
                gen.Emit(OpCodes.Call, _CloneContextGetShouldUseExistingObjectsMethod);
                gen.Emit(OpCodes.Brtrue_S, ifShouldUseExistingLabel);

                gen.Emit(OpCodes.Ldnull);
                gen.Emit(OpCodes.Br_S, afterLoadingDestinationLabel);

                gen.MarkLabel(ifShouldUseExistingLabel);
                gen.Emit(OpCodes.Ldloc_0);
                gen.Emit(OpCodes.Ldfld, field);

                gen.MarkLabel(afterLoadingDestinationLabel);
            }
        }

        #endregion

        #region Helper Methods

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
            return IsTypeCloneablePrimative(rootType);
        }

        private static bool IsTypeCloneablePrimative(Type inputType)
        {
            if (Array.Exists(_CloneablePrimitives, x => x == inputType))
                return true;

            if (inputType.IsPrimitive)
                return true;

            return false;
        }

        private static Type GetRootType(Type type)
        {
            if(type == null)
                throw new ArgumentNullException("type");

            Type currentType = type;

            while (currentType.IsArray)
                currentType = currentType.GetElementType();

            if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(Nullable<>))
                return currentType.GetGenericArguments()[0];

            return currentType;
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

        #endregion

        #region Can Methods

        private static bool CanClone(Type type, bool isShallow)
        {
            Type typeToCheck = GetRootType(type);

            if (typeToCheck.IsInterface && typeToCheck.IsDefined(typeof(CloneReferenceOnlyAttribute), true) != true)
                return false;

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

                if (fieldType.IsInterface)
                {
                    if (field.IsDefined(typeof(CloneReferenceOnlyAttribute), true) != true &&
                        fieldType.IsDefined(typeof(CloneReferenceOnlyAttribute), true) != true)
                         return false;
                     else
                         continue;
                }

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

        public static T Clone(T source)
        {
            var context = new CloneContext(false, false);
            return Strategy(source, default(T), context);
        }

        public static T ShallowClone(T source)
        {
            var context = new CloneContext(true, false);
            return Strategy(source, default(T), context);
        }

        public static T CloneTo(T source, T destination)
        {
            if(source == null)
                throw new ArgumentNullException("source");

            if (destination == null)
                throw new ArgumentNullException("destination");

            if (_TargetType.IsValueType != true && ReferenceEquals(source, destination))
                throw new InvalidOperationException("The destination object cannot be the same as the source.");

            var sourceArray = source as Array;
            var destArray = destination as Array;

            if(sourceArray != null && destArray != null && destArray.LongLength != sourceArray.LongLength)
                throw new InvalidOperationException("The destination array must be the same size (length) as the source array.");

            var context = new CloneContext(false, true);
            return Strategy(source, destination, context);
        }

        public static T ShallowCloneTo(T source, T destination)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (destination == null)
                throw new ArgumentNullException("destination");

            if(_TargetType.IsValueType != true && ReferenceEquals(source, destination))
                throw new InvalidOperationException("The destination object cannot be the same as the source.");

            var sourceArray = source as Array;
            var destArray = destination as Array;

            if (sourceArray != null && destArray != null && destArray.LongLength != sourceArray.LongLength)
                throw new InvalidOperationException("The destination array must be the same size (length) as the source array.");

            var context = new CloneContext(true, true);
            return Strategy(source, destination, context);
        }

        private static T ArrayClone<U>(Array sourceArray, Array destArray, CloneContext context)
        {
            var i = new ArrayIndex(sourceArray);
            while(true)
            {
                U sourceValue = (U)sourceArray.GetValue(i);
                U currentValue = context.ShouldUseExistingObjects ? (U)destArray.GetValue(i) : default(U);

                U value = Cloneable<U>.Strategy(sourceValue, currentValue, context);
                destArray.SetValue(value, i);

                if(i.CanIncrement())
                    i.Increment();
                else
                    break;
            }

            return (T)(object)destArray;
        }


        #endregion

        private static Func<T, T, CloneContext, T> Strategy
        {
            [ReflectionPermission(SecurityAction.Demand, ReflectionEmit = true)]
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
