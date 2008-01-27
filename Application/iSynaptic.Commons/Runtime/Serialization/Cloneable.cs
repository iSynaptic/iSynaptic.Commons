using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;

using iSynaptic.Commons.Extensions;
using System.Security.Permissions;

namespace iSynaptic.Commons.Runtime.Serialization
{
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

        private static Type GetNullableUnderlyingType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return type.GetGenericArguments()[0];

            return null;
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

        private static bool IsCloneablePrimitive(Type inputType)
        {
            if (Array.Exists(_CloneablePrimitives, x => x == inputType))
                return true;

            if (inputType.IsPrimitive)
                return true;

            Type nullableUnderlying = GetNullableUnderlyingType(inputType);
            
            if (nullableUnderlying != null)
                return IsCloneablePrimitive(nullableUnderlying);

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

        #region CanClone

        public static bool CanClone()
        {
            if (_CanClone.HasValue != true)
                _CanClone = CanClone(_TargetType);

            return _CanClone.Value;            
        }

        private static bool CanClone(Type type)
        {
            Type typeToCheck = GetNullableUnderlyingType(type) ?? type;

            if (IsNotCloneable(typeToCheck))
                return false;

            if (IsCloneablePrimitive(typeToCheck))
                return true;

            if (typeToCheck.IsArray)
                return CanClone(GetUnderlyingArrayType(typeToCheck));

            Predicate<FieldInfo> includeFilter = _FieldIncludeFilter.And(f => f.FieldType != typeToCheck);

            foreach (FieldInfo field in GetFields(typeToCheck, includeFilter))
            {
                Type fieldType = GetNullableUnderlyingType(field.FieldType) ?? field.FieldType;

                if (IsNotCloneable(fieldType))
                    return false;

                if (IsCloneablePrimitive(fieldType))
                    continue;

                if (fieldType.IsArray)
                {
                    if (CanClone(GetUnderlyingArrayType(fieldType)))
                        continue;
                    else
                        return false;
                }

                Type fieldClonableType = typeof(Cloneable<>).MakeGenericType(fieldType);
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
            Type typeToCheck = GetNullableUnderlyingType(type) ?? type;

            if (IsNotCloneable(typeToCheck))
                return false;

            Predicate<FieldInfo> includeFilter = _FieldIncludeFilter.And(f => f.FieldType != typeToCheck);

            foreach (FieldInfo field in GetFields(typeToCheck, includeFilter))
            {
                Type fieldType = GetNullableUnderlyingType(field.FieldType) ?? field.FieldType;

                if (IsNotCloneable(fieldType))
                    return false;

                if (fieldType.IsArray)
                {
                    if (CanClone(GetUnderlyingArrayType(fieldType)))
                        continue;
                    else
                        return false;
                }

            }

            return true;
        }

        #endregion

        #region Clone Methods

        [ReflectionPermission(SecurityAction.Demand, ReflectionEmit = true)]
        public static T Clone(T source)
        {
            Dictionary<object, object> map = new Dictionary<object, object>();
            return Clone(source, map);
        }

        private static T Clone(T source, IDictionary<object, object> map)
        {
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
            if (_ShallowCloneHandler == null)
            {
                if (CanShallowClone())
                {
                    if (IsCloneablePrimitive(_TargetType))
                        _ShallowCloneHandler = s => s;
                    else if (_TargetType.IsArray)
                    {
                        if (source == null)
                            return default(T);

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

            if(isValueType != true)
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
    }
}
