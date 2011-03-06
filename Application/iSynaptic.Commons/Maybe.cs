using System;
using System.Collections.Generic;
using System.Threading;

namespace iSynaptic.Commons
{
    public struct Maybe<T> : IMaybe<T>, IEquatable<Maybe<T>>, IEquatable<T>
    {
        internal struct MaybeResult
        {
            public T Value;
            public bool HasValue;
            public Exception Exception;
        }

        public static readonly Maybe<T> NoValue = new Maybe<T>();
        public static readonly Maybe<T> Default = new Maybe<T>(default(T));

        private readonly Func<MaybeResult> _Computation;

        public Maybe(Func<T> computation) : this()
        {
            _Computation = Default.Bind(x => computation()).Computation;
        }

        public Maybe(T value) : this()
        {
            _Computation = () => new MaybeResult {Value = value, HasValue = true};
        }

        internal Maybe(Func<MaybeResult> computation) : this()
        {
            _Computation = computation;
        }

        internal Func<MaybeResult> Computation
        {
            get { return _Computation ?? (() => new MaybeResult()); }
        }

        public T Value
        {
            get
            {
                var result = Computation();

                if(result.Exception != null)
                    result.Exception.Rethrow();

                if(result.HasValue != true)
                    throw new InvalidOperationException("No value can be provided.");

                return result.Value;
            }
        }

        public bool HasValue { get { return Computation().HasValue; } }
        public Exception Exception { get { return Computation().Exception; } }

        public bool Equals(T other)
        {
            return Equals(new Maybe<T>(other));
        }

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
                return -1;

            if (Value == null)
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

        public static bool operator ==(Maybe<T> left, T right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Maybe<T> left, T right)
        {
            return !(left == right);
        }

        public static bool operator ==(T left, Maybe<T> right)
        {
            return right.Equals(left);
        }

        public static bool operator !=(T left, Maybe<T> right)
        {
            return !(left == right);
        }

        public Maybe<TResult> Bind<TResult>(Func<T, TResult> func)
        {
            return Bind(x => new Maybe<TResult>(func(x)));
        }

        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> func)
        {
            var computation = Computation;

            Func<Maybe<TResult>.MaybeResult> boundComputation = () =>
            {
                var result = computation();

                if (result.Exception != null)
                    return new Maybe<TResult>.MaybeResult { Exception = result.Exception };

                if (result.HasValue != true)
                    return new Maybe<TResult>.MaybeResult();

                try
                {
                    return func(result.Value).Computation();
                }
                catch (Exception ex)
                {
                    return new Maybe<TResult>.MaybeResult { Exception = ex };
                }
            };

            return new Maybe<TResult>(boundComputation.Memoize());
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

    public static class Maybe
    {
        public static Maybe<T> NotNull<T>(T value) where T : class
        {
            return Value(value).NotNull();
        }

        public static Maybe<T> NotNull<T>(T? value) where T : struct
        {
            return Value(value)
                .NotNull()
                .Select(x => x.Value);
        }

        public static Maybe<T> NotNull<T>(Func<T> computation) where T : class
        {
            return Value(computation)
                .NotNull();
        }

        public static Maybe<T> NotNull<T>(Func<T?> computation) where T : struct
        {
            return Value(computation)
                .NotNull()
                .Select(x => x.Value);
        }

        public static Maybe<T> Value<T>(T value)
        {
            return new Maybe<T>(value);
        }

        public static Maybe<T> Value<T>(Func<T> computation)
        {
            return new Maybe<T>(computation);
        }

        public static Maybe<TResult> Select<T, TResult>(this Maybe<T> self, Func<T, TResult> selector)
        {
            Guard.NotNull(selector, "selector");
            return self.Bind(selector);
        }

        public static Maybe<TResult> Select<T, TResult>(this Maybe<T> self, Func<T, Maybe<TResult>> selector)
        {
            Guard.NotNull(selector, "selector");
            return self.Bind(selector);
        }

        public static Maybe<T> NotNull<T>(this Maybe<T> self) where T : class
        {
            return self.NotNull(x => x);
        }

        public static Maybe<T?> NotNull<T>(this Maybe<T?> self) where T : struct
        {
            return self.NotNull(x => x);
        }

        public static Maybe<TResult> NotNull<T, TResult>(this Maybe<T> self, Func<T, TResult> selector) where TResult : class
        {
            Guard.NotNull(selector, "selector");
            return self
                .Select(selector)
                .Where(x => x != null);
        }

        public static Maybe<TResult?> NotNull<T, TResult>(this Maybe<T> self, Func<T, TResult?> selector) where TResult : struct
        {
            Guard.NotNull(selector, "selector");
            return self
                .Select(selector)
                .Where(x => x.HasValue);
        }

        public static Maybe<T> Where<T>(this Maybe<T> self, Func<T, bool> predicate)
        {
            Guard.NotNull(predicate, "predicate");
            return self.Bind(x => predicate(x) ? x : Maybe<T>.NoValue);
        }

        public static Maybe<T> Unless<T>(this Maybe<T> self, Func<T, bool> predicate)
        {
            Guard.NotNull(predicate, "predicate");
            return self.Bind(x => predicate(x) ? Maybe<T>.NoValue : x);
        }

        public static T Return<T>(this Maybe<T> self)
        {
            return self.Return(default(T));
        }

        public static T Return<T>(this Maybe<T> self, T @default)
        {
            return self
                .OnException(@default)
                .OnNoValue(@default)
                .Value;
        }

        public static Maybe<T> Do<T>(this Maybe<T> self, Action<T> action)
        {
            Guard.NotNull(action, "action");
            return self.Bind(x =>
            {
                action(x);
                return x;
            });
        }

        public static Maybe<T> Assign<T>(this Maybe<T> self, ref T target)
        {
            if (self.HasValue)
                target = self.Value;

            return self;
        }

        public static Maybe<T> OnNoValue<T>(this Maybe<T> self, T value)
        {
            return self.OnNoValue(() => value);
        }

        public static Maybe<T> OnNoValue<T>(this Maybe<T> self, Func<T> valueFactory)
        {
            return self.OnNoValue(() => Value(valueFactory));
        }

        public static Maybe<T> OnNoValue<T>(this Maybe<T> self, Func<Maybe<T>> valueFactory)
        {
            Guard.NotNull(valueFactory, "valueFactory");
            return self.When(x => x.HasValue != true, x => valueFactory());
        }

        public static Maybe<T> OnException<T>(this Maybe<T> self, T value)
        {
            return self.OnException(x => Value(value));
        }

        public static Maybe<T> OnException<T>(this Maybe<T> self, Action<Exception> handler)
        {
            Guard.NotNull(handler, "handler");
            return self.When(x => x.Exception != null, x => { handler(x.Exception); return x; });
        }

        public static Maybe<T> OnException<T>(this Maybe<T> self, Func<Exception, Maybe<T>> handler)
        {
            Guard.NotNull(handler, "handler");
            return self.When(x => x.Exception != null, x => handler(x.Exception));
        }

        public static Maybe<T> ThrowIfException<T>(this Maybe<T> self)
        {
            return self.ThrowIfException(typeof(Exception));
        }

        public static Maybe<T> ThrowIfException<T>(this Maybe<T> self, Type exceptionType)
        {
            Guard.NotNull(exceptionType, "exceptionType");

            return new Maybe<T>(() => Value(self)
                                            .Where(y => y.Exception != null)
                                            .Where(y => exceptionType.IsAssignableFrom(y.GetType()))
                                            .Value.Value);
        }

        public static Maybe<T> With<T, TSelected>(this Maybe<T> self, Func<T, TSelected> selector, Action<TSelected> action)
        {
            Guard.NotNull(selector, "selector");
            Guard.NotNull(action, "action");

            return With(self, x => Value(selector(x)), action);
        }

        public static Maybe<T> With<T, TSelected>(this Maybe<T> self, Func<T, Maybe<TSelected>> selector, Action<TSelected> action)
        {
            Guard.NotNull(selector, "selector");
            Guard.NotNull(action, "action");

            return self.Bind(x =>
            {
                selector(x)
                    .Do(action)
                    .Return();

                return x;
            });
        }

        public static Maybe<T> When<T>(this Maybe<T> self, Func<Maybe<T>, bool> predicate, Func<Maybe<T>, Maybe<T>> computation)
        {
            Guard.NotNull(predicate, "predicate");
            Guard.NotNull(computation, "computation");

            return Value(self)
                    .Select(predicate)
                    .Select(y => y ? computation(self) : self);
        }

        public static Maybe<T> When<T>(this Maybe<T> self, Maybe<T> value, Func<T, Maybe<T>> computation)
        {
            Guard.NotNull(computation, "computation");
            return self.When(x => x.Equals(value), x => x.Bind(computation));
        }

        public static Maybe<T> When<T>(this Maybe<T> self, Maybe<T> value, Action<T> action)
        {
            Guard.NotNull(action, "action");
            return self.When(value, x => { action(x); return x; });
        }

        public static Maybe<T> When<T>(this Maybe<T> self, Func<Maybe<T>, bool> predicate, Maybe<T> result)
        {
            Guard.NotNull(predicate, "predicate");
            return self.When(predicate, x => result);
        }

        public static Maybe<T> When<T>(this Maybe<T> self, Maybe<T> value, Maybe<T> result)
        {
            return self.When(value, x => result);
        }

        public static Maybe<T> Run<T>(this Maybe<T> self)
        {
            return self.HasValue ? self : self;
        }

        public static Maybe<T> ToThreadSafe<T>(this Maybe<T> self)
        {
            if(self.Computation != null)
                return new Maybe<T>(self.Computation.Synchronize());

            return self;
        }
    }
}
