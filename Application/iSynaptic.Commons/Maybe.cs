using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public sealed class Maybe<T>
    {
        public static readonly Maybe<T> NoValue = new Maybe<T>(default(T)) { HasValue = false };

        private T _Value = default(T);

        public Maybe(T value)
        {
            HasValue = true;
            Value = value;
        }

        public bool HasValue { get; private set; }
        public T Value
        {
            get
            {
                if(HasValue != true)
                    throw new InvalidOperationException("Maybe object must have a value.");

                return _Value;
            }
            private set { _Value = value; }
        }
    }
}
