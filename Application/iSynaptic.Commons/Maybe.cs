using System;
using System.Collections.Generic;

namespace iSynaptic.Commons
{
    public struct Maybe<T> : IMaybe<T>, IEquatable<Maybe<T>>
    {
        public static readonly Maybe<T> NoValue = new Maybe<T>();
        public static readonly Maybe<T> Default = new Maybe<T>(default(T));

        private readonly T _Value;
        private readonly bool _HasValue;
        private readonly Exception _Exception;

        public Maybe(T value)
        {
            _Value = value;
            _HasValue = true;
            _Exception = null;
        }

        internal Maybe(Exception exception)
        {
            _Value = default(T);
            _HasValue = false;
            _Exception = exception;
        }

        public T Value
        {
            get
            {
                if(Exception != null)
                    Exception.Rethrow();

                if(HasValue != true)
                    throw new InvalidOperationException("No value can be provided.");

                return _Value;
            }
        }

        public bool HasValue { get { return _HasValue; } }
        public Exception Exception { get { return _Exception; } }

        public bool Equals(Maybe<T> other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        public bool Equals(Maybe<T> other, IEqualityComparer<T> comparer)
        {
            Guard.NotNull(comparer, "comparer");

            if (Exception != null)
                return other.Exception != null && other.Exception == Exception;

            if (other.Exception != null)
                return false;

            if (!HasValue)
                return !other.HasValue;

            if (!other.HasValue)
                return false;

            return comparer.Equals(Value, other.Value);
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
            if (Exception != null)
                return Exception.GetHashCode();

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

        public Maybe<TResult> Bind<TResult>(Func<T, TResult> func)
        {
            return Bind(x => new Maybe<TResult>(func(x)));
        }

        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> func)
        {
            if (Exception != null)
                return new Maybe<TResult>(Exception);

            if (HasValue != true)
                return Maybe<TResult>.NoValue;

            try
            {
                return func(Value);
            }
            catch (Exception ex)
            {
                return new Maybe<TResult>(ex);
            }
        }

        public static implicit operator Maybe<T>(T value)
        {
            return new Maybe<T>(value);
        }

        public static explicit operator T(Maybe<T> value)
        {
            return value.Value;
        }
    }
}
