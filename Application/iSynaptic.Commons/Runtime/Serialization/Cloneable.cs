using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;

using iSynaptic.Commons.Extensions;

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

        private static bool? _CanClone = null;
        private static bool? _CanShallowClone = null;

        private static Func<T, IDictionary<object, object>, T> _CloneHandler = null;
        private static Func<T, T> _ShallowCloneHandler = null;

        private static readonly Predicate<FieldInfo> _FieldIncludeFilter = f =>
            (f.IsDefined(typeof(NonSerializedAttribute), true) != true);

        static Cloneable()
        {
            _TargetType = typeof(T);
        }

        #region Helper Methods

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

        private static IEnumerable<FieldInfo> GetFields(Type type, Predicate<FieldInfo> includeFilter)
        {
            Type currentType = type;
            while (currentType != null)
            {
                foreach (FieldInfo info in currentType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (includeFilter == null)
                        yield return info;
                    else if (includeFilter(info))
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
            if (inputType.IsDefined(typeof(NonSerializedAttribute), true))
                return true;

            if (typeof(Delegate).IsAssignableFrom(inputType))
                return true;

            if (inputType == typeof(IntPtr))
                return true;

            return false;
        }

        private static bool IsCloneablePrimitive(Type inputType)
        {
            if (Array.Exists(_FixedCloneablePrimitive, x => x == inputType))
                return true;

            if (inputType.IsPrimitive)
                return true;

            return false;
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

        #endregion

        #region CanClone

        public static bool CanClone()
        {
            if (_CanClone.HasValue != true)
                _CanClone = CanClone(_TargetType);

            return _CanClone.Value;            
        }

        private static bool CanClone(Type type)
        {
            if (IsNotCloneable(type))
                return false;

            if (IsCloneablePrimitive(type))
                return true;

            if (type.IsArray)
                return CanClone(GetUnderlyingArrayType(type));

            Predicate<FieldInfo> includeFilter = _FieldIncludeFilter.And(f => f.FieldType != type);

            foreach (FieldInfo field in GetFields(type, includeFilter))
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
                var canClone = canCloneMethod.ToFunc<bool>();

                if (canClone())
                    continue;
                else
                    return false;
            }

            return true;
        }

        #endregion

        #region CanShallowClone

        public static bool CanShallowClone()
        {
            if (_CanShallowClone.HasValue != true)
                _CanShallowClone = CanShallowClone(_TargetType);

            return _CanShallowClone.Value;            
        }

        private static bool CanShallowClone(Type type)
        {
            if (IsNotCloneable(type))
                return false;

            Predicate<FieldInfo> includeFilter = _FieldIncludeFilter.And(f => f.FieldType != type);

            foreach (FieldInfo field in GetFields(type, includeFilter))
            {
                if (IsNotCloneable(field.FieldType))
                    return false;
            }

            return true;
        }

        #endregion

        #region Clone Methods

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

        public static T ShallowClone(T source)
        {
            if (source == null)
                return default(T);

            if (_ShallowCloneHandler == null)
            {
                if (CanShallowClone())
                {
                    if (IsCloneablePrimitive(_TargetType))
                        _ShallowCloneHandler = s => s;
                    else if (_TargetType.IsArray)
                    {
                        Array sourceArray = source as Array;
                        return (T)(object)sourceArray.Clone();
                    }
                    else
                        _ShallowCloneHandler = BuildShallowCloneHandler(_TargetType);
                }
                else
                    _ShallowCloneHandler = s => { throw new InvalidOperationException("This type cannot be cloned."); };
            }

            return _ShallowCloneHandler(source);
        }

        #endregion

        #region Build Handler Methods

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

            foreach (FieldInfo field in GetFields(type, _FieldIncludeFilter))
            {
                if (IsCloneablePrimitive(field.FieldType) || 
                    (field.FieldType.IsValueType != true && field.IsDefined(typeof(CloneReferenceOnlyAttribute), true)))
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
            MethodInfo getTypeFromHandlerMethod = GetMethod(typeof(Type), "GetTypeFromHandle", typeof(RuntimeTypeHandle));
            MethodInfo getSafeUninitializedObjectMethod = GetMethod(typeof(FormatterServices), "GetSafeUninitializedObject", typeof(Type));

            DynamicMethod cloneMethod = new DynamicMethod(string.Format("Cloneable<{0}>_Clone", type.Name), type, new Type[] { type }, type, true);
            ILGenerator gen = cloneMethod.GetILGenerator();

            gen.DeclareLocal(type);

            gen.Emit(OpCodes.Ldtoken, type);
            gen.Emit(OpCodes.Call, getTypeFromHandlerMethod);
            gen.Emit(OpCodes.Call, getSafeUninitializedObjectMethod);
            gen.Emit(OpCodes.Castclass, type);
            gen.Emit(OpCodes.Stloc_0);

            foreach (FieldInfo field in GetFields(type, _FieldIncludeFilter))
            {
                gen.Emit(OpCodes.Ldloc_0);
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldfld, field);
                gen.Emit(OpCodes.Stfld, field);
            }

            gen.Emit(OpCodes.Ldloc_0);
            gen.Emit(OpCodes.Ret);

            return cloneMethod.ToFunc<T, T>();
        }

        #endregion
    }
}
