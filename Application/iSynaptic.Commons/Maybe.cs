using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public struct Maybe<T> : IEquatable<Maybe<T>>
    {
        public static readonly Maybe<T> NoValue = new Maybe<T>(default(T)) { _HasValue = false };

        private T _Value;
        private bool _HasValue;

        public Maybe(T value)
        {
            _HasValue = true;
            _Value = value;
        }

        public bool HasValue { get { return _HasValue; } }
        public T Value
        {
            get
            {
                if(HasValue != true)
                    throw new InvalidOperationException("Maybe object must have a value.");

                return _Value;
            }
        }

        public bool Equals(Maybe<T> other)
        {
            if (HasValue != true)
                return other.HasValue == HasValue;

            if (other.HasValue != true)
                return false;

            if (Value is IEquatable<T>)
                return ((IEquatable<T>) Value).Equals(other.Value);

            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            if (obj is Maybe<T>)
                return Equals((Maybe<T>) obj);

            return false;
        }

        public override int GetHashCode()
        {
            if (HasValue != true)
                return 0;

            return Value.GetHashCode();
        }

        public static bool operator ==(Maybe<T> left, Maybe<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Maybe<T> left, Maybe<T> right)
        {
            return !(left == right);
        }
    }
}
