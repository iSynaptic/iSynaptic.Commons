using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;

namespace iSynaptic.Commons.Runtime.Serialization
{
    public static class Cloneable<T>
    {
        private static Type _TargetType = null;
        private static Func<T, T> _CloneHandler = null;

        static Cloneable()
        {
            _TargetType = typeof(T);
        }

        public static bool CanClone()
        {
            return CanClone(_TargetType);
        }

        private static bool CanClone(Type type)
        {
            if (IsNotCloneable(type))
                return false;

            if (IsValueObject(type))
                return true;

            if (type.IsArray)
                return CanClone(GetUnderlyingArrayType(type));

            foreach (FieldInfo field in GetFields(type))
            {
                if (IsNotCloneable(field.FieldType))
                    return false;

                if (IsValueObject(field.FieldType))
                    continue;

                if (type.IsArray)
                {
                    if (CanClone(GetUnderlyingArrayType(field.FieldType)))
                        continue;
                    else
                        return false;
                }

                Type fieldClonableType = typeof(Cloneable<>).MakeGenericType(field.FieldType);
                MethodInfo canCloneMethod = fieldClonableType.GetMethod("CanClone", BindingFlags.Public | BindingFlags.Static);
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

            while(currentType.IsArray)
                currentType = currentType.GetElementType();

            return currentType;
        }

        private static IEnumerable<FieldInfo> GetFields(Type type)
        {
            Type currentType = type;
            while (currentType != null)
            {
                foreach (FieldInfo info in currentType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    yield return info;

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

        private static bool IsValueObject(Type inputType)
        {
            if (inputType == typeof(string))
                return true;

            if (inputType == typeof(DateTime))
                return true;

            if (inputType == typeof(TimeSpan))
                return true;

            if (inputType == typeof(Guid))
                return true;

            if (inputType == typeof(decimal))
                return true;

            if (inputType.IsPrimitive)
                return true;

            return false;
        }

        private static Func<T, T> BuildCloneHandler(Type type)
        {
            MethodInfo getTypeFromHandlerMethod = typeof(Type).GetMethod(
               "GetTypeFromHandle",
               BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
               null,
               new Type[]{
                    typeof(RuntimeTypeHandle)
                    },
               null
               );

            MethodInfo getSafeUninitializedObjectMethod = typeof(FormatterServices).GetMethod(
                "GetSafeUninitializedObject",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[]{
                    typeof(Type)
                    },
                null
                );

            DynamicMethod cloneMethod = new DynamicMethod(string.Format("Cloneable<{0}>_Clone", type.Name), type, new Type[] { type }, type, true);
            ILGenerator gen = cloneMethod.GetILGenerator();

            LocalBuilder clonedObj = gen.DeclareLocal(typeof(T));
            
            gen.Emit(OpCodes.Ldtoken, typeof(T));
            gen.Emit(OpCodes.Call, getTypeFromHandlerMethod);
            gen.Emit(OpCodes.Call, getSafeUninitializedObjectMethod);
            gen.Emit(OpCodes.Castclass, typeof(T));
            gen.Emit(OpCodes.Stloc_0);

            foreach (FieldInfo field in GetFields(type))
            {
                if (IsValueObject(field.FieldType))
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

                    MethodInfo childCloneMethod = typeof(Cloneable<>).MakeGenericType(field.FieldType).GetMethod(
                        "Clone",
                        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                        null,
                        new Type[]{
                            field.FieldType
                            },
                        null
                        );

                    gen.Emit(OpCodes.Call, childCloneMethod);
                    gen.Emit(OpCodes.Stfld, field);
                }
            }

            gen.Emit(OpCodes.Ldloc_0);
            gen.Emit(OpCodes.Ret);

            return (Func<T, T>) cloneMethod.CreateDelegate(typeof(Func<T, T>));
        }

        private static Func<T, T> BuildArrayCloneHandler(Type itemType)
        {
            Type clonableType = typeof(Cloneable<T>);
            MethodInfo info = clonableType.GetMethod("ArrayClone", BindingFlags.NonPublic | BindingFlags.Static);

            info = info.MakeGenericMethod(itemType);

            return (Func<T, T>) Delegate.CreateDelegate(typeof(Func<T, T>), info);
        }

        public static T Clone(T source)
        {
            if (_CloneHandler == null)
            {
                if (CanClone())
                {
                    if (IsValueObject(_TargetType))
                        _CloneHandler = s => s;
                    else if (_TargetType.IsArray)
                        _CloneHandler = BuildArrayCloneHandler(_TargetType.GetElementType());
                    else
                        _CloneHandler = BuildCloneHandler(_TargetType);
                }
                else
                    _CloneHandler = s => { throw new InvalidOperationException("This type cannot be cloned."); };
            }

            return _CloneHandler(source);
        }

        private static T ArrayClone<U>(T source)
        {
            Array sourceArray = (Array)(object)source;
            ArrayIndex index = new ArrayIndex(sourceArray);

            Array destArray = (Array) sourceArray.Clone();

            if (sourceArray.Length <= 0)
                return (T) (object) destArray;

            while(true)
            {
                U item = (U)sourceArray.GetValue(index);
                destArray.SetValue(Cloneable<U>.Clone(item), index);

                if (index.CanIncrement())
                    index.Increment();
                else
                    break;
            }

            return (T)(object)destArray;
        }
    }
}
