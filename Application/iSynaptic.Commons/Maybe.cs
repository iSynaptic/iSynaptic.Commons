using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace iSynaptic.Commons
{
    public struct Maybe<T> : IMaybe<T>, IMaybe, IEquatable<Maybe<T>>, IEquatable<T>
    {
        private struct MaybeResult
        {
            public T Value;
            public bool HasValue;
            public Exception Exception;
        }

        public static readonly Maybe<T> NoValue = new Maybe<T>();
        public static readonly Maybe<T> Default = new Maybe<T>(default(T));

        private readonly MaybeResult? _Value;
        private readonly Func<MaybeResult> _Computation;

        public Maybe(Func<T> computation)
            : this()
        {
            Guard.NotNull(computation, "computation");
            _Computation = Default.Bind(x => computation())._Computation;
        }

        public Maybe(T value)
            : this()
        {
            _Value = new MaybeResult { Value = value, HasValue = true };
        }

        public Maybe(Exception exception)
            : this()
        {
            Guard.NotNull(exception, "exception");
            _Value = new MaybeResult { Exception = exception };
        }

        private Maybe(Func<MaybeResult> computation)
            : this()
        {
            Guard.NotNull(computation, "computation");
            _Computation = computation;
        }

        public static Maybe<T> Unsafe(Func<Maybe<T>> unsafeComputation)
        {
            Guard.NotNull(unsafeComputation, "unsafeComputation");

            Func<MaybeResult> finalComputation =
                () => unsafeComputation().ComputeResult();

            return new Maybe<T>(finalComputation.Memoize());
        }

        private MaybeResult ComputeResult()
        {
            if (_Value.HasValue)
                return _Value.Value;

            if (_Computation != null)
                return _Computation();

            return default(MaybeResult);
        }

        public T Value
        {
            get
            {
                var result = ComputeResult();

                if (result.Exception != null)
                    result.Exception.ThrowPreservingCallStack();

                if (result.HasValue != true)
                    throw new InvalidOperationException("No value can be provided.");

                return result.Value;
            }
        }

        object IMaybe.Value
        {
            get { return Value; }
        }

        public bool HasValue { get { return ComputeResult().HasValue; } }
        public Exception Exception { get { return ComputeResult().Exception; } }

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

            return other.HasValue && comparer.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            if (obj is Maybe<T>)
                return Equals((Maybe<T>)obj);

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
            Guard.NotNull(func, "func");
            return Bind(x => new Maybe<TResult>(func(x)));
        }

        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> func)
        {
            Guard.NotNull(func, "func");

            Func<MaybeResult> resultComputer = ComputeResult;

            Func<Maybe<TResult>.MaybeResult> boundComputation = () =>
            {
                var result = resultComputer();

                if (result.Exception != null)
                    return new Maybe<TResult>.MaybeResult { Exception = result.Exception };

                if (result.HasValue != true)
                    return new Maybe<TResult>.MaybeResult();

                try
                {
                    return func(result.Value).ComputeResult();
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
        #region Value Operator

        public static Maybe<T> Value<T>(T value)
        {
            return new Maybe<T>(value);
        }

        public static Maybe<T> Value<T>(Func<T> computation)
        {
            Guard.NotNull(computation, "computation");
            return new Maybe<T>(computation);
        }

        #endregion

        #region NotNull Operator

        public static Maybe<T> NotNull<T>(T value) where T : class
        {
            return Value(value).NotNull();
        }

        public static Maybe<T> NotNull<T>(Func<T> computation) where T : class
        {
            return Value(computation).NotNull();
        }

        public static Maybe<T> NotNull<T>(T? value) where T : struct
        {
            return Value(value).NotNull();
        }

        public static Maybe<T> NotNull<T>(Func<T?> computation) where T : struct
        {
            return Value(computation).NotNull();
        }

        public static Maybe<T> NotNull<T>(this Maybe<T> self) where T : class
        {
            return self.NotNull(x => x);
        }

        public static Maybe<T> NotNull<T>(this Maybe<T?> self) where T : struct
        {
            return self.Where(x => x.HasValue).Select(x => x.Value);
        }

        public static Maybe<T> NotNull<T, TResult>(this Maybe<T> self, Func<T, TResult> selector) where TResult : class
        {
            Guard.NotNull(selector, "selector");
            return self.Where(x => selector(x) != null);
        }

        public static Maybe<T> NotNull<T, TResult>(this Maybe<T> self, Func<T, TResult?> selector) where TResult : struct
        {
            Guard.NotNull(selector, "selector");
            return self.Where(x => selector(x).HasValue);
        }

        #endregion

        #region Using Operator

        public static Maybe<T> Using<T, TResource>(TResource resource, Func<TResource, Maybe<T>> selector) where TResource : IDisposable
        {
            Guard.NotNull(resource, "resource");
            Guard.NotNull(selector, "selector");

            return Using(() => resource, selector);
        }

        public static Maybe<T> Using<T, TResource>(Func<TResource> resourceFactory, Func<TResource, Maybe<T>> selector) where TResource : IDisposable
        {
            Guard.NotNull(resourceFactory, "resourceFactory");
            Guard.NotNull(selector, "selector");

            return Maybe<TResource>.Default
                .Using(x => resourceFactory(), selector);
        }

        public static Maybe<TResult> Using<T, TResource, TResult>(this Maybe<T> self, Func<T, TResource> resourceSelector, Func<TResource, Maybe<TResult>> selector) where TResource : IDisposable
        {
            Guard.NotNull(resourceSelector, "resourceSelector");
            Guard.NotNull(selector, "selector");

            return self.Select(x =>
            {
                using (var resource = resourceSelector(x))
                    return selector(resource);
            });
        }

        #endregion

        #region Select Operator

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

        #endregion

        #region Coalesce Operator

        public static Maybe<TResult> Coalesce<T, TResult>(this Maybe<T> self, Func<T, TResult> selector) where TResult : class
        {
            Guard.NotNull(selector, "selector");
            return self.Coalesce(selector, () => Maybe<TResult>.NoValue);
        }

        public static Maybe<TResult> Coalesce<T, TResult>(this Maybe<T> self, Func<T, TResult?> selector) where TResult : struct
        {
            Guard.NotNull(selector, "selector");
            return self.Coalesce(selector, () => Maybe<TResult>.NoValue);
        }

        public static Maybe<TResult> Coalesce<T, TResult>(this Maybe<T> self, Func<T, TResult> selector, Func<Maybe<TResult>> valueIfNullFactory) where TResult : class
        {
            Guard.NotNull(selector, "selector");
            Guard.NotNull(valueIfNullFactory, "valueIfNullFactory");

            return self
                .Select(selector)
                .NotNull()
                .Or(valueIfNullFactory);
        }

        public static Maybe<TResult> Coalesce<T, TResult>(this Maybe<T> self, Func<T, TResult?> selector, Func<Maybe<TResult>> valueIfNullFactory) where TResult : struct
        {
            Guard.NotNull(selector, "selector");
            Guard.NotNull(valueIfNullFactory, "valueIfNullFactory");

            return self
                .Select(selector)
                .NotNull()
                .Or(valueIfNullFactory);
        }

        #endregion

        #region Extract Operator

        public static T Extract<T>(this Maybe<T> self)
        {
            return self.Extract(default(T));
        }

        public static T Extract<T>(this Maybe<T> self, T @default)
        {
            return self.Extract(() => @default);
        }

        public static T Extract<T>(this Maybe<T> self, Func<T> @default)
        {
            return self.Or(@default)
                .Value;
        }

        #endregion

        #region Or Operator

        public static Maybe<T> Or<T>(this Maybe<T> self, T value)
        {
            return self.Or(() => value);
        }

        public static Maybe<T> Or<T>(this Maybe<T> self, Maybe<T> other)
        {
            return self.Or(() => other);
        }

        public static Maybe<T> Or<T>(this Maybe<T> self, Func<T> valueFactory)
        {
            Guard.NotNull(valueFactory, "valueFactory");
            return self.Or(() => Value(valueFactory));
        }

        public static Maybe<T> Or<T>(this Maybe<T> self, Func<Maybe<T>> valueFactory)
        {
            Guard.NotNull(valueFactory, "valueFactory");
            return Maybe<T>.Unsafe(() => self.Exception == null && self.HasValue != true ? valueFactory() : self);
        }

        #endregion

        #region With Operator

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
                    .OnValue(action)
                    .ThrowOnException()
                    .Run();

                return x;
            });
        }

        #endregion

        #region When Operator

        public static Maybe<T> When<T>(this Maybe<T> self, T value, T newValue)
        {
            return self.When(value, x => newValue);
        }

        public static Maybe<T> When<T>(this Maybe<T> self, T value, Action<T> action)
        {
            Guard.NotNull(action, "action");
            return self.When(x => x.Equals(value), x => { action(x); return x; });
        }

        public static Maybe<T> When<T>(this Maybe<T> self, T value, Func<T, Maybe<T>> computation)
        {
            Guard.NotNull(computation, "computation");
            return self.When(x => x.Equals(value), computation);
        }

        public static Maybe<T> When<T>(this Maybe<T> self, Func<T, bool> predicate, Action<T> action)
        {
            Guard.NotNull(predicate, "predicate");
            Guard.NotNull(action, "action");

            return self.When(predicate, x => { action(x); return self; });
        }

        public static Maybe<T> When<T>(this Maybe<T> self, Func<T, bool> predicate, Func<T, Maybe<T>> computation)
        {
            Guard.NotNull(predicate, "predicate");
            Guard.NotNull(computation, "computation");

            return self
                    .Where(predicate)
                    .Select(computation)
                    .Or(self);
        }

        #endregion

        #region Join Operator

        public static Maybe<Tuple<T, U>> Join<T, U>(this Maybe<T> self, Maybe<U> other)
        {
            return self.Join(other, Tuple.Create);
        }

        public static Maybe<TResult> Join<T, U, TResult>(this Maybe<T> self, Maybe<U> other, Func<T, U, TResult> selector)
        {
            return self.Select(t => other.Select(r => selector(t, r)));
        }

        #endregion

        #region ThrowOn Operator

        public static Maybe<T> ThrowOn<T>(this Maybe<T> self, T value, Exception exception)
        {
            return self.ThrowOn(value, () => exception);
        }

        public static Maybe<T> ThrowOn<T>(this Maybe<T> self, T value, Func<Exception> exceptionFactory)
        {
            return self.ThrowOn(x => x.Equals(value), exceptionFactory);
        }

        public static Maybe<T> ThrowOn<T>(this Maybe<T> self, Func<T, bool> predicate, Func<Exception> exceptionFactory)
        {
            return self.When(predicate, x => { throw exceptionFactory(); })
                .ThrowOnException();
        }

        #endregion

        #region ThrowOnNoValue Operator

        public static Maybe<T> ThrowOnNoValue<T>(this Maybe<T> self, Exception exception)
        {
            Guard.NotNull(exception, "exception");
            return self.ThrowOnNoValue(() => exception);
        }

        public static Maybe<T> ThrowOnNoValue<T>(this Maybe<T> self, Func<Exception> exceptionFactory)
        {
            Guard.NotNull(exceptionFactory, "exceptionFactory");
            return self
                .OnNoValue(() => { throw exceptionFactory(); })
                .ThrowOnException();
        }

        #endregion

        #region ThrowOnException Operator

        public static Maybe<T> ThrowOnException<T>(this Maybe<T> self)
        {
            return self.ThrowOnException(typeof(Exception));
        }

        public static Maybe<T> ThrowOnException<T>(this Maybe<T> self, Type exceptionType)
        {
            Guard.NotNull(exceptionType, "exceptionType");
            return self.ThrowOnException(x => exceptionType.IsAssignableFrom(x.GetType()));
        }

        public static Maybe<T> ThrowOnException<T>(this Maybe<T> self, Func<Exception, bool> predicate)
        {
            Guard.NotNull(predicate, "predicate");

            Func<Maybe<T>> boundComputation = () =>
            {
                if (self.Exception != null && predicate(self.Exception))
                    self.Exception.ThrowPreservingCallStack();

                return self;
            };

            return Maybe<T>.Unsafe(boundComputation);
        }

        #endregion

        #region OnException Operator

        public static Maybe<T> OnException<T>(this Maybe<T> self, T value)
        {
            return self.OnException(() => value);
        }

        public static Maybe<T> OnException<T>(this Maybe<T> self, Func<T> valueFactory)
        {
            return self.OnException(x => Value(valueFactory()));
        }

        public static Maybe<T> OnException<T>(this Maybe<T> self, Action<Exception> handler)
        {
            Guard.NotNull(handler, "handler");
            return self.OnException(x => { handler(x); return new Maybe<T>(x); });
        }

        public static Maybe<T> OnException<T>(this Maybe<T> self, Func<Exception, Maybe<T>> handler)
        {
            Guard.NotNull(handler, "handler");
            return Maybe<T>.Unsafe(() => self.Exception != null ? Value(() => handler(self.Exception)).Select(x => x) : self);
        }

        #endregion

        public static Maybe<T> OnValue<T>(this Maybe<T> self, Action<T> action)
        {
            Guard.NotNull(action, "action");
            return self.Bind(x =>
            {
                action(x);
                return x;
            });
        }

        public static Maybe<T> OnNoValue<T>(this Maybe<T> self, Action action)
        {
            Guard.NotNull(action, "action");

            return Maybe<T>.Unsafe(() =>
            {
                if (self.Exception == null && self.HasValue != true)
                    action();

                return self;
            });
        }

        public static Maybe<T> Where<T>(this Maybe<T> self, Func<T, bool> predicate)
        {
            Guard.NotNull(predicate, "predicate");
            return self.Bind(x => predicate(x) ? x : Maybe<T>.NoValue);
        }

        public static Maybe<T> Unless<T>(this Maybe<T> self, Func<T, bool> predicate)
        {
            Guard.NotNull(predicate, "predicate");
            return self.Where(x => !predicate(x));
        }

        public static Maybe<T> Assign<T>(this Maybe<T> self, ref T target)
        {
            if (self.HasValue)
                target = self.Value;

            return self;
        }

        public static Maybe<T> Run<T>(this Maybe<T> self, Action<T> action = null)
        {
            return self
                .When(x => action != null, x => self.OnValue(action))
                .HasValue ? self : self;
        }

        public static Maybe<T> RunAsync<T>(this Maybe<T> self, Action<T> action = null, CancellationToken cancellationToken = default(CancellationToken), TaskCreationOptions taskCreationOptions = TaskCreationOptions.None, TaskScheduler taskScheduler = default(TaskScheduler))
        {
            var task = Task.Factory.StartNew(() => self.Run(action), cancellationToken, taskCreationOptions, taskScheduler ?? TaskScheduler.Default);
            return Maybe<T>.Unsafe(() =>
            {
                try
                {
                    return task.Result;
                }
                catch (AggregateException ex)
                {
                    ex.InnerException.ThrowPreservingCallStack();
                    return Maybe<T>.NoValue;
                }
            });
        }

        public static Maybe<T> Synchronize<T>(this Maybe<T> self)
        {
            return SynchronizeOn(self, new object());
        }

        public static Maybe<T> SynchronizeOn<T>(this Maybe<T> self, object synchronizeOn)
        {
            Guard.NotNull(synchronizeOn, "synchronizeOn");

            Func<Maybe<T>> synchronizedComputation = () => self.Run();
            synchronizedComputation = synchronizedComputation.SynchronizeOn(() => true, synchronizeOn);

            return Maybe<T>.Unsafe(synchronizedComputation);
        }

        public static Maybe<TResult> Cast<TResult>(this IMaybe self)
        {
            Guard.NotNull(self, "self");

            return Maybe<TResult>.Unsafe(() => 
            {
                if (self.Exception != null)
                    return new Maybe<TResult>(self.Exception);

                if (self.HasValue != true)
                    return Maybe<TResult>.NoValue;

                return (TResult) self.Value;
            });
        }

        public static Maybe<TResult> OfType<TResult>(this IMaybe self)
        {
            Guard.NotNull(self, "self");

            return Maybe<TResult>.Unsafe(() =>
            {
                if (self.Exception != null)
                    return new Maybe<TResult>(self.Exception);

                if (self.HasValue != true)
                    return Maybe<TResult>.NoValue;

                if(self.Value is TResult)
                    return (TResult)self.Value;

                return Maybe<TResult>.NoValue;
            });
        }

        public static T? ToNullable<T>(this Maybe<T> self) where T : struct
        {
            return self.Select(x => (T?)x)
                .Or((T?)null)
                .Extract();
        }
    }
}
