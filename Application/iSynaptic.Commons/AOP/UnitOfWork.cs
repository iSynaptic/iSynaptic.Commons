using System;
using System.Collections.Generic;

namespace iSynaptic.Commons.AOP
{
    public abstract class UnitOfWork<TItem, TUnitOfWork> : EnlistmentScope<TItem, TUnitOfWork>, IUnitOfWork<TItem>
        where TUnitOfWork : EnlistmentScope<TItem, TUnitOfWork>, IUnitOfWork<TItem>
    {
        protected UnitOfWork() : this(ScopeNesting.Allowed)
        {
        }

        protected UnitOfWork(ScopeNesting nesting) : base(ScopeBounds.Thread, nesting)
        {
        }

        protected abstract void Process(IEnumerable<TItem> items);

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
