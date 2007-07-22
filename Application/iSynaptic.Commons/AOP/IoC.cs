using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons
{
    public abstract partial class IoC : NestedScope<IoC>
    {
        protected IoC() : this(ScopeBounds.Thread)
        {
        }

        protected IoC(ScopeBounds bounds) : base(bounds)
        {
        }

        #region Static Methods

        public static void AddDependency<T>(T existing)
        {
            CheckCurrentScope();
            InternalCurrent.InternalAddDependency<T>(existing);
        }

        public static void AddDependency<T>(T existing, string id)
        {
            CheckCurrentScope();
            InternalCurrent.InternalAddDependency<T>(existing, id);
        }

        public static void RemoveDependency<T>()
        {
            CheckCurrentScope();
            InternalCurrent.InternalRemoveDependency<T>();
        }

        public static void RemoveDependency<T>(string id)
        {
            CheckCurrentScope();
            InternalCurrent.InternalRemoveDependency<T>(id);
        }

        public static void AddTypeMapping<TI, TO>()
        {
            CheckCurrentScope();
            InternalCurrent.InternalAddTypeMapping<TI, TO>();
        }

        public static T Resolve<T>()
        {
            CheckCurrentScope();
            return InternalCurrent.InternalResolve<T>();
        }

        public static T Resolve<T>(T existing)
        {
            CheckCurrentScope();
            return InternalCurrent.InternalResolve<T>(existing);
        }

        public static T Resolve<T>(T existing, string id)
        {
            CheckCurrentScope();
            return InternalCurrent.InternalResolve<T>(existing, id);
        }

        public static T Resolve<T, P>(T existing, string id, P policyInformation)
        {
            CheckCurrentScope();
            return InternalCurrent.InternalResolve<T, P>(existing, id, policyInformation);
        }

        public static void AddDependency(Type type, object existing)
        {
            CheckCurrentScope();
            InternalCurrent.InternalAddDependency(type, existing);
        }

        public static void AddDependency(Type type, object existing, string id)
        {
            CheckCurrentScope();
            InternalCurrent.InternalAddDependency(type, existing, id);
        }

        public static void RemoveDependency(Type type)
        {
            CheckCurrentScope();
            InternalCurrent.InternalRemoveDependency(type);
        }

        public static void RemoveDependency(Type type, string id)
        {
            CheckCurrentScope();
            InternalCurrent.InternalRemoveDependency(type, id);
        }

        public static void AddTypeMapping(Type inputType, Type outputType)
        {
            CheckCurrentScope();
            InternalCurrent.InternalAddTypeMapping(inputType, outputType);
        }

        public static void AddTypeMapping(Type inputType, Type outputType, string id)
        {
            CheckCurrentScope();
            InternalCurrent.InternalAddTypeMapping(inputType, outputType, id);
        }

        public static object Resolve(Type type)
        {
            CheckCurrentScope();
            return InternalCurrent.InternalResolve(type);
        }

        public static object Resolve(Type type, object existing)
        {
            CheckCurrentScope();
            return InternalCurrent.InternalResolve(type, existing);
        }

        public static object Resolve(Type type, object existing, string id)
        {
            CheckCurrentScope();
            return InternalCurrent.InternalResolve(type, existing, id);
        }

        public static object Resolve(Type type, object existing, string id, object policyInformation)
        {
            CheckCurrentScope();
            return InternalCurrent.InternalResolve(type, existing, id, policyInformation);
        }

        private static void CheckCurrentScope()
        {
            if (InternalCurrent == null)
                throw new ApplicationException("No IoC scope is currently active.");
        }

        #endregion

        #region Instance Methods

        public void InternalAddDependency<T>(T existing)
        {
            InternalAddDependency(typeof(T), existing);
        }

        public void InternalAddDependency<T>(T existing, string id)
        {
            InternalAddDependency(typeof(T), existing, id);
        }

        public void InternalAddTypeMapping<TI, TO>()
        {
            InternalAddTypeMapping(typeof(TI), typeof(TO));
        }

        public void InternalRemoveDependency<T>()
        {
            InternalRemoveDependency(typeof(T));
        }

        public void InternalRemoveDependency<T>(string id)
        {
            InternalRemoveDependency(typeof(T), id);
        }

        public T InternalResolve<T>()
        {
            return (T)InternalResolve(typeof(T));
        }

        public T InternalResolve<T>(T existing)
        {
            return (T)InternalResolve(typeof(T), existing);
        }

        public T InternalResolve<T>(T existing, string id)
        {
            return (T)InternalResolve(typeof(T), existing, id);
        }

        public T InternalResolve<T, P>(T existing, string id, P policyInformation)
        {
            return (T)InternalResolve(typeof(T), existing, id, policyInformation);
        }

        public abstract void InternalAddDependency(Type type, object existing);
        public abstract void InternalAddDependency(Type type, object existing, string id);
        public abstract void InternalRemoveDependency(Type type);
        public abstract void InternalRemoveDependency(Type type, string id);
        public abstract void InternalAddTypeMapping(Type inputType, Type outputType);
        public abstract void InternalAddTypeMapping(Type inputType, Type outputType, string id);
        public abstract object InternalResolve(Type type);
        public abstract object InternalResolve(Type type, object existing);
        public abstract object InternalResolve(Type type, object existing, string id);
        public abstract object InternalResolve(Type type, object existing, string id, object policyInformation);

        #endregion
    }
}
