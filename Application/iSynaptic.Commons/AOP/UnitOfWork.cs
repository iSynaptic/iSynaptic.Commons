using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.AOP
{
    public abstract class UnitOfWork<T, U> : EnlistmentScope<T, U>, IUnitOfWork<T>
        where U : EnlistmentScope<T, U>, IUnitOfWork<T>
    {
        public UnitOfWork() : this(ScopeNesting.Allowed)
        {
        }

        public UnitOfWork(ScopeNesting nesting) : base(ScopeBounds.Thread, nesting)
        {
        }

        protected abstract void Process(IEnumerable<T> items);

        public void Complete()
        {
            if(Disposed)
                throw new ObjectDisposedException(GetType().Name);

            if(Completed)
                throw new InvalidOperationException("Unit of work has already been completed.");

            Completed = true;
            Process(Items);
        }

        protected bool Completed { get; private set; }
    }
}
