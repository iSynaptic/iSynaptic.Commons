using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace iSynaptic.Commons.Runtime.Serialization
{
    public static class Cloneable<T>
    {
        private static Type _CloneType = null;
        private static Func<T, T> _CloneHandler = null;

        static Cloneable()
        {
            _CloneType = typeof(T);
        }

        public static bool CanClone()
        {
            if (IsNotCloneable(_CloneType))
                return false;

            if (IsPrimative(_CloneType))
                return true;

            foreach (FieldInfo field in GetFields())
            {
                if (IsNotCloneable(field.FieldType))
                    return false;

                if (IsPrimative(field.FieldType))
                    continue;

                Type fieldClonableType = typeof(Cloneable<>).MakeGenericType(field.FieldType);
                MethodInfo canCloneMethod = fieldClonableType.GetMethod("CanClone", BindingFlags.NonPublic | BindingFlags.Static);
                Func<bool> canClone = (Func<bool>) Delegate.CreateDelegate(typeof(Func<bool>), canCloneMethod);

                if (canClone())
                    continue;
                else
                    return false;
            }

            return true;
        }

        private static IEnumerable<FieldInfo> GetFields()
        {
            Type currentType = _CloneType;
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
            if (inputType == typeof(IntPtr))
                return true;

            return false;
        }

        private static bool IsPrimative(Type inputType)
        {
            if (inputType == typeof(string))
                return true;

            if (inputType == typeof(DateTime))
                return true;

            if (inputType == typeof(Guid))
                return true;

            if (inputType.IsPrimitive)
                return true;

            return false;
        }

        private static Func<T, T> BuildCloneHandler()
        {
            if (IsPrimative(_CloneType))
                return s => s;

            throw new NotImplementedException();
        }

        public static T Clone(T source)
        {
            if (_CloneHandler == null && CanClone())
                _CloneHandler = BuildCloneHandler();
            else
                _CloneHandler = s => { throw new InvalidOperationException("This type cannot be cloned."); };

            return _CloneHandler(source);
        }
    }
}
