using System;
using System.Collections.Generic;
using System.Linq;

namespace iSynaptic.Commons.AOP
{
    public class DisposableContext : EnlistmentScope<IDisposable, DisposableContext>
    {
        public DisposableContext() : this(ScopeBounds.Thread, ScopeNesting.Allowed)
        {
        }

        public DisposableContext(ScopeBounds bounds, ScopeNesting nesting) : base(bounds, nesting)
        {
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (Parent != null)
                {
                    Parent.Enlist(Items);
                    Items.Clear();
                }
                else
                {
                    var exceptions = new List<Exception>();
                    
                    Action<IDisposable> dispose = d => d.Dispose();
                    dispose = dispose.CatchExceptions(exceptions);

                    Items
                        .ToList()
                        .ForEach(dispose);

                    Items.Clear();

                    if (exceptions.Count > 0)
                        throw new AggregateException("Exception(s) occured during disposal.", exceptions);
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        public static DisposableContext Current
        {
            get { return GetCurrentScope(); }
        }
    }
}
