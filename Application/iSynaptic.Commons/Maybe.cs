// The MIT License
// 
// Copyright (c) 2011 Jordan E. Terrell
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using iSynaptic.Commons.Collections.Generic;
using iSynaptic.Commons.Linq;

namespace iSynaptic.Commons
{
    // Implementation of the Maybe monad. http://en.wikipedia.org/wiki/Monad_%28functional_programming%29#Maybe_monad
    // Thanks to Brian Beckman for his suggestions and assistance.
    // Don't Fear the Monad! http://channel9.msdn.com/shows/Going+Deep/Brian-Beckman-Dont-fear-the-Monads/
    public struct Maybe<T> : IMaybe<T>, IEquatable<Maybe<T>>, IEquatable<T>
    {
        public static readonly Maybe<T> NoValue = new Maybe<T>();
        public static readonly Maybe<T> Default = new Maybe<T>(default(T));

        private readonly T _Value;
        private readonly bool _HasValue;
        private readonly Exception _Exception;

        private readonly Func<Maybe<T>> _Computation;

        public Maybe(T value)
            : this()
        {
            _Value = value;
            _HasValue = true;
        }

        public Maybe(Func<T> computation)
            : this(() => new Maybe<T>(computation()))
        {
            Guard.NotNull(computation, "computation");
        }

        public Maybe(Func<Maybe<T>> computation)
            : this()
        {
            var cachedComputation = Guard.NotNull(computation, "computation");
            var memoizedResult = default(Maybe<T>);
            var resultComputed = false;

            _Computation = () =>
            {
                if (resultComputed)
                    return memoizedResult;

                memoizedResult = cachedComputation();
                resultComputed = true;
                cachedComputation = null;

                return memoizedResult;
            };
        }

        public Maybe(Exception exception)
            : this()
        {
            _Exception = Guard.NotNull(exception, "exception");
        }

        public T Value
        {
            get
            {
                if(_Computation != null)
                    return _Computation().Value;

                if(_HasValue)
                    return _Value;

                if (_Exception != null)
                    _Exception.ThrowAsInnerExceptionIfNeeded();

                throw new InvalidOperationException("No value can be computed.");
            }
        }

        object IMaybe.Value
        {
            get { return Value; }
        }

        public bool HasValue
        {
            get
            {
                if(_Computation == null)
                    return _HasValue;

                return _Computation().HasValue;
            }
        }
        public Exception Exception
        {
            get
            {
                if(_Computation == null)
                    return _Exception;

                return _Computation().Exception;
            }
        }

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

            if (obj is T)
                return Equals(new Maybe<T>((T) obj));

            return false;
        }

        public override int GetHashCode()
        {
            return GetHashCode(EqualityComparer<T>.Default);
        }

        public int GetHashCode(IEqualityComparer<T> comparer)
        {
            Guard.NotNull(comparer, "comparer");

            if (Exception != null)
                return Exception.GetHashCode();

            if (HasValue != true)
                return -1;

            return comparer.GetHashCode(Value);
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

        public static explicit operator T(Maybe<T> value)
        {
            return value.Value;
        }
    }

    public static class Maybe
    {
        #region Defer Operator

        public static Maybe<T> Defer<T>(Func<T> computation)
        {
            return new Maybe<T>(computation);
        }

        public static Maybe<T> Defer<T>(Func<Maybe<T>> computation)
        {
            return new Maybe<T>(computation);
        }

        #endregion

        #region If Operator

        public static Maybe<T> If<T>(bool predicate, Maybe<T> thenValue)
        {
            return predicate ? thenValue : Maybe<T>.NoValue;
        }

        public static Maybe<T> If<T>(bool predicate, Maybe<T> thenValue, Maybe<T> elseValue)
        {
            return predicate ? thenValue : elseValue;
        }

        public static Maybe<T> If<T>(Func<bool> predicate, Maybe<T> thenValue)
        {
            Guard.NotNull(predicate, "predicate");
            return new Maybe<T>(() => predicate() ? thenValue : Maybe<T>.NoValue);
        }

        public static Maybe<T> If<T>(Func<bool> predicate, Maybe<T> thenValue, Maybe<T> elseValue)
        {
            Guard.NotNull(predicate, "predicate");
            return new Maybe<T>(() => predicate() ? thenValue : elseValue);
        }

        #endregion

        #region NotNull Operator

        public static Maybe<T> NotNull<T>(T value) where T : class
        {
            return value != null ? new Maybe<T>(value) : Maybe<T>.NoValue;
        }

        public static Maybe<T> NotNull<T>(Func<T> computation) where T : class
        {
            Guard.NotNull(computation, "computation");
            return new Maybe<T>(() =>
            {
                var result = computation();
                return result != null ? new Maybe<T>(result) : Maybe<T>.NoValue;
            });
        }

        public static Maybe<T> NotNull<T>(T? value) where T : struct
        {
            return value.HasValue ? new Maybe<T>(value.Value) : Maybe<T>.NoValue;
        }

        public static Maybe<T> NotNull<T>(Func<T?> computation) where T : struct
        {
            Guard.NotNull(computation, "computation");
            return new Maybe<T>(() =>
            {
                var result = computation();
                return result.HasValue ? new Maybe<T>(result.Value) : Maybe<T>.NoValue;
            });
        }

        public static Maybe<T> NotNull<T>(this Maybe<T> @this) where T : class
        {
            return @this.Bind(x => x != null ? new Maybe<T>(x) : Maybe<T>.NoValue);
        }

        public static Maybe<T> NotNull<T>(this Maybe<T?> @this) where T : struct
        {
            return @this.Bind(x => x.HasValue ? new Maybe<T>(x.Value) : Maybe<T>.NoValue);
        }

        public static Maybe<T> NotNull<T, TResult>(this Maybe<T> @this, Func<T, TResult> selector) where TResult : class
        {
            Guard.NotNull(selector, "selector");
            return @this.Bind(x => selector(x) != null ? new Maybe<T>(x) : Maybe<T>.NoValue);
        }

        public static Maybe<T> NotNull<T, TResult>(this Maybe<T> @this, Func<T, TResult?> selector) where TResult : struct
        {
            Guard.NotNull(selector, "selector");
            return @this.Bind(x => selector(x).HasValue ? new Maybe<T>(x) : Maybe<T>.NoValue);
        }

        #endregion

        #region Using Operator

        public static Maybe<T> Using<T, TResource>(Func<TResource> resourceFactory, Func<TResource, Maybe<T>> selector) where TResource : IDisposable
        {
            Guard.NotNull(resourceFactory, "resourceFactory");
            Guard.NotNull(selector, "selector");

            return new Maybe<T>(() =>
            {
                using (var resource = resourceFactory())
                    return selector(resource);
            });
        }

        public static Maybe<TResult> Using<T, TResource, TResult>(this Maybe<T> @this, Func<T, TResource> resourceSelector, Func<TResource, Maybe<TResult>> selector) where TResource : IDisposable
        {
            Guard.NotNull(resourceSelector, "resourceSelector");
            Guard.NotNull(selector, "selector");

            return @this.Bind(x =>
            {
                using (var resource = resourceSelector(x))
                    return selector(resource);
            });
        }

        #endregion

        #region Coalesce Operator

        public static Maybe<TResult> Coalesce<T, TResult>(this Maybe<T> @this, Func<T, TResult> selector) where TResult : class
        {
            return @this.Coalesce(selector, Maybe<TResult>.NoValue);
        }

        public static Maybe<TResult> Coalesce<T, TResult>(this Maybe<T> @this, Func<T, TResult?> selector) where TResult : struct
        {
            return @this.Coalesce(selector, Maybe<TResult>.NoValue);
        }

        public static Maybe<TResult> Coalesce<T, TResult>(this Maybe<T> @this, Func<T, TResult> selector, Maybe<TResult> valueIfNull) where TResult : class
        {
            Guard.NotNull(selector, "selector");

            return @this.Bind(x =>
            {
                var result = selector(x);
                return result != null ? new Maybe<TResult>(result) : valueIfNull;
            });
        }

        public static Maybe<TResult> Coalesce<T, TResult>(this Maybe<T> @this, Func<T, TResult?> selector, Maybe<TResult> valueIfNull) where TResult : struct
        {
            Guard.NotNull(selector, "selector");

            return @this.Bind(x =>
            {
                var result = selector(x);
                return result.HasValue ? new Maybe<TResult>(result.Value) : valueIfNull;
            });
        }

        #endregion

        #region ValueOrDefault Operator

        public static T ValueOrDefault<T>(this Maybe<T> @this, Func<T> @default)
        {
            if (@this.Exception == null && @this.HasValue != true)
                return @default();

            return @this.Value;
        }

        public static T ValueOrDefault<T>(this Maybe<T> @this, T @default)
        {
            if (@this.Exception == null && @this.HasValue != true)
                return @default;

            return @this.Value;
        }

        public static T ValueOrDefault<T>(this Maybe<T> @this)
        {
            return @this.ValueOrDefault(default(T));
        }

        #endregion

        #region Or Operator

        public static Maybe<T> Or<T>(this Maybe<T> @this, T value)
        {
            var self = @this;
            return new Maybe<T>(() => self.Exception == null && self.HasValue != true ? new Maybe<T>(value) : self);
        }

        public static Maybe<T> Or<T>(this Maybe<T> @this, Func<T> valueFactory)
        {
            Guard.NotNull(valueFactory, "valueFactory");

            var self = @this;
            return new Maybe<T>(() => self.Exception == null && self.HasValue != true ? new Maybe<T>(valueFactory()) : self);
        }

        public static Maybe<T> Or<T>(this Maybe<T> @this, Func<Maybe<T>> valueFactory)
        {
            Guard.NotNull(valueFactory, "valueFactory");

            var self = @this;
            return new Maybe<T>(() => self.Exception == null && self.HasValue != true ? valueFactory() : self);
        }

        public static Maybe<T> Or<T>(this Maybe<T> @this, Maybe<T> other)
        {
            var self = @this;
            return new Maybe<T>(() => self.Exception == null && self.HasValue != true ? other : self);
        }

        #endregion

        #region With Operator

        public static Maybe<T> With<T, TSelected>(this Maybe<T> @this, Func<T, TSelected> selector, Action<TSelected> action)
        {
            Guard.NotNull(selector, "selector");
            Guard.NotNull(action, "action");

            var self = @this;

            return new Maybe<T>(() =>
            {
                if (self.HasValue)
                    action(selector(self.Value));

                return self;
            });
        }

        public static Maybe<T> With<T, TSelected>(this Maybe<T> @this, Func<T, Maybe<TSelected>> selector, Action<TSelected> action)
        {
            Guard.NotNull(selector, "selector");
            Guard.NotNull(action, "action");

            var self = @this;

            return new Maybe<T>(() =>
            {
                if (self.HasValue)
                {
                    var selected = selector(self.Value);
                    if (selected.HasValue)
                        action(selected.Value);
                }

                return self;
            });
        }

        #endregion

        #region When Operator

        public static Maybe<T> When<T>(this Maybe<T> @this, T value, Action<T> action)
        {
            Guard.NotNull(action, "action");

            var self = @this;
            return new Maybe<T>(() =>
            {
                if (self.HasValue && EqualityComparer<T>.Default.Equals(self.Value, value))
                    action(self.Value);

                return self;
            });
        }

        public static Maybe<T> When<T>(this Maybe<T> @this, T value, Maybe<T> newValue)
        {
            var self = @this;
            return new Maybe<T>(() =>
            {
                if (self.HasValue && EqualityComparer<T>.Default.Equals(self.Value, value))
                    return newValue;

                return self;
            });
        }

        public static Maybe<T> When<T>(this Maybe<T> @this, Func<T, bool> predicate, Action<T> action)
        {
            Guard.NotNull(predicate, "predicate");
            Guard.NotNull(action, "action");

            var self = @this;
            return new Maybe<T>(() =>
            {
                if (self.HasValue && predicate(self.Value))
                    action(self.Value);

                return self;
            });
        }

        public static Maybe<T> When<T>(this Maybe<T> @this, Func<T, bool> predicate, Maybe<T> newValue)
        {
            Guard.NotNull(predicate, "predicate");

            var self = @this;
            return new Maybe<T>(() =>
            {
                if (self.HasValue && predicate(self.Value))
                    return newValue;

                return self;
            });
        }

        #endregion

        #region Catch Operator

        public static Maybe<T> Catch<T>(this Maybe<T> @this)
        {
            var self = @this;
            return new Maybe<T>(() =>
            {
                try
                {
                    return self.HasValue ? self : self;
                }
                catch (Exception ex)
                {
                    return new Maybe<T>(ex);
                }
            });
        }

        public static Maybe<T> Catch<T>(this Maybe<T> @this, Func<Exception, bool> exceptionPredicate)
        {
            Guard.NotNull(exceptionPredicate, "exceptionPredicate");

            var self = @this;
            return new Maybe<T>(() =>
            {
                try
                {
                    return self.HasValue ? self : self;
                }
                catch (Exception ex)
                {
                    if(exceptionPredicate(ex))
                        return new Maybe<T>(ex);

                    throw;
                }
            });
        }

        #endregion

        #region Suppress Operator

        public static Maybe<T> Suppress<T>(this Maybe<T> @this)
        {
            var self = @this;
            return new Maybe<T>(() => self.Exception != null ? Maybe<T>.NoValue : self);
        }

        public static Maybe<T> Suppress<T>(this Maybe<T> @this, T value)
        {
            var self = @this;
            return new Maybe<T>(() => self.Exception != null ? new Maybe<T>(value) : self);
        }

        public static Maybe<T> Suppress<T>(this Maybe<T> @this, Func<Exception, T> valueFactory)
        {
            Guard.NotNull(valueFactory, "valueFactory");

            var self = @this;
            return new Maybe<T>(() => self.Exception != null ? new Maybe<T>(valueFactory(self.Exception)) : self);
        }

        #endregion

        #region Join Operator

        public static Maybe<Tuple<T, U>> Join<T, U>(this Maybe<T> @this, Maybe<U> other)
        {
            var self = @this;
            return new Maybe<Tuple<T, U>>(() =>
            {
                if (self.HasValue && other.HasValue)
                    return new Maybe<Tuple<T, U>>(Tuple.Create(self.Value, other.Value));

                if (self.Exception != null || other.Exception != null)
                    return new Maybe<Tuple<T, U>>(self.Exception ?? other.Exception);

                return Maybe<Tuple<T, U>>.NoValue;
            });
        }

        public static Maybe<TResult> Join<T, U, TResult>(this Maybe<T> @this, Maybe<U> other, Func<T, U, TResult> selector)
        {
            Guard.NotNull(selector, "selector");

            var self = @this;
            return new Maybe<TResult>(() =>
            {
                if (self.HasValue && other.HasValue)
                return new Maybe<TResult>(selector(self.Value, other.Value));

                if (self.Exception != null || other.Exception != null)
                    return new Maybe<TResult>(self.Exception ?? other.Exception);
                
                return Maybe<TResult>.NoValue;
            });
        }

        public static Maybe<TResult> Join<T, U, TResult>(this Maybe<T> @this, Maybe<U> other, Func<T, U, Maybe<TResult>> selector)
        {
            Guard.NotNull(selector, "selector");

            var self = @this;
            return new Maybe<TResult>(() =>
            {
                if (self.HasValue && other.HasValue)
                    return selector(self.Value, other.Value);

                if (self.Exception != null || other.Exception != null)
                    return new Maybe<TResult>(self.Exception ?? other.Exception);

                return Maybe<TResult>.NoValue;
            });
        }

        #endregion

        #region ThrowOnNoValue Operator

        public static Maybe<T> ThrowOnNoValue<T>(this Maybe<T> @this, Exception exception)
        {
            Guard.NotNull(exception, "exception");

            var self = @this;
            return new Maybe<T>(() =>
            {
                if (self.Exception == null && self.HasValue != true)
                    exception.ThrowAsInnerExceptionIfNeeded();

                return self;
            });
        }

        public static Maybe<T> ThrowOnNoValue<T>(this Maybe<T> @this, Func<Exception> exceptionFactory)
        {
            Guard.NotNull(exceptionFactory, "exceptionFactory");

            var self = @this;
            return new Maybe<T>(() =>
            {
                if (self.Exception == null && self.HasValue != true)
                    exceptionFactory().ThrowAsInnerExceptionIfNeeded();

                return self;
            });
        }

        #endregion

        #region ThrowOnException Operator

        public static Maybe<T> ThrowOnException<T>(this Maybe<T> @this)
        {
            var self = @this;
            return new Maybe<T>(() => 
            {
                if(self.Exception != null)
                    self.Exception.ThrowAsInnerExceptionIfNeeded();

                return self;
            });
        }

        public static Maybe<T> ThrowOnException<T>(this Maybe<T> @this, Type exceptionType)
        {
            Guard.NotNull(exceptionType, "exceptionType");

            var self = @this;
            return new Maybe<T>(() =>
            {
                if (self.Exception != null && exceptionType.IsAssignableFrom(self.Exception.GetType()))
                    self.Exception.ThrowAsInnerExceptionIfNeeded();

                return self;
            });
        }

        public static Maybe<T> ThrowOnException<T>(this Maybe<T> @this, Func<Exception, Exception> exceptionSelector)
        {
            Guard.NotNull(exceptionSelector, "exceptionSelector");

            var self = @this;
            return new Maybe<T>(() =>
            {
                if (self.Exception != null)
                {
                    var ex = exceptionSelector(self.Exception);
                    if(ex != null)
                        ex.ThrowAsInnerExceptionIfNeeded();
                }

                return self;
            });
        }

        #endregion

        #region ThrowOn Operator

        public static Maybe<T> ThrowOn<T>(this Maybe<T> @this, T value, Exception exception)
        {
            Guard.NotNull(exception, "exception");

            var self = @this;
            return new Maybe<T>(() =>
            {
                if(self.HasValue && EqualityComparer<T>.Default.Equals(self.Value, value))
                    exception.ThrowAsInnerExceptionIfNeeded();

                return self;
            });
        }

        public static Maybe<T> ThrowOn<T>(this Maybe<T> @this, Func<Maybe<T>, Exception> exceptionSelector)
        {
            Guard.NotNull(exceptionSelector, "exceptionSelector");

            var self = @this;
            return new Maybe<T>(() =>
            {
                var ex = exceptionSelector(self);
                if(ex != null)
                    ex.ThrowAsInnerExceptionIfNeeded();

                return self;
            });
        }

        #endregion

        #region SelectMany Operator
        
        // This is an alias of Bind, and exists only to satisfy C#'s LINQ comprehension syntax.
        // The name "SelectMany" is confusing as there is only one value to "select".
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Maybe<TResult> SelectMany<T, TResult>(this Maybe<T> @this, Func<T, Maybe<TResult>> selector)
        {
            Guard.NotNull(selector, "selector");
            return @this.Bind(selector);
        }

        // This operator is implemented only to satisfy C#'s LINQ comprehension syntax. 
        // The name "SelectMany" is confusing as there is only one value to "select".
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Maybe<TResult> SelectMany<T, TIntermediate, TResult>(this Maybe<T> @this, Func<T, Maybe<TIntermediate>> selector, Func<T, TIntermediate, TResult> combiner)
        {
            Guard.NotNull(selector, "selector");
            Guard.NotNull(combiner, "combiner");
            return @this.SelectMany(x => selector(x).SelectMany(y => combiner(x, y).ToMaybe()));
        }

        #endregion

        #region ToEnumerable Operator

        public static IEnumerable<T> ToEnumerable<T>(this Maybe<T> @this)
        {
            return ToEnumerable(new[] {@this});
        }

        public static IEnumerable<T> ToEnumerable<T>(params Maybe<T>[] values)
        {
            Guard.NotNull(values, "values");
            return Maybe<T>.NoValue.ToEnumerable(values);
        }

        public static IEnumerable<T> ToEnumerable<T>(this Maybe<T> @this, IEnumerable<Maybe<T>> others)
        {
            Guard.NotNull(others, "others");

            return new[] {@this}
                .Concat(others)
                .Squash();
        }

        #endregion

        #region Unwrap Operator

        public static Maybe<T> Unwrap<T>(this IMaybe<IMaybe<T>> @this)
        {
            return @this.AsMaybe().Select(x => x.AsMaybe()).Unwrap();
        }

        public static Maybe<T> Unwrap<T>(this Maybe<Maybe<T>> @this)
        {
            return @this.SelectMaybe(x => x);
        }

        #endregion

        public static Maybe<TResult> Bind<T, TResult>(this Maybe<T> @this, Func<T, Maybe<TResult>> selector)
        {
            Guard.NotNull(selector, "selector");

            var self = @this;

            return new Maybe<TResult>(() =>
            {
                if (self.Exception != null)
                    return new Maybe<TResult>(self.Exception);

                if (self.HasValue != true)
                    return Maybe<TResult>.NoValue;

                return selector(self.Value);
            });
        }

        public static Maybe<TResult> Select<T, TResult>(this Maybe<T> @this, Func<T, TResult> selector)
        {
            Guard.NotNull(selector, "selector");
            return @this.Bind(x => selector(x).ToMaybe());
        }

        public static Maybe<TResult> Express<T, TResult>(this Maybe<T> @this, Func<Maybe<T>, Maybe<TResult>> func)
        {
            Guard.NotNull(func, "func");

            var self = @this;
            return new Maybe<TResult>(() => func(self));
        }

        public static T Extract<T>(this Maybe<T> @this)
        {
            return @this.Value;
        }

        public static Maybe<T> Return<T>(T value)
        {
            return new Maybe<T>(value);
        }

        public static Maybe<T> Exception<T>(Exception exception)
        {
            Guard.NotNull(exception, "exception");
            return new Maybe<T>(exception);
        }

        public static Maybe<T> Throw<T>(Exception exception)
        {
            Guard.NotNull(exception, "exception");
            return new Maybe<T>((Func<T>)(() => { throw exception; }));
        }

        // This is an alias of Bind and SelectMany.  Since SelectMany doesn't make sense (because there is at most one value),
        // the name SelectMaybe communicates better than Bind or SelectMany the semantics of the function.
        public static Maybe<TResult> SelectMaybe<T, TResult>(this Maybe<T> @this, Func<T, Maybe<TResult>> selector)
        {
            Guard.NotNull(selector, "selector");
            return @this.Bind(selector);
        }

        public static Maybe<T> Finally<T>(this Maybe<T> @this, Action finallyAction)
        {
            Guard.NotNull(finallyAction, "finallyAction");

            return @this.Express(x =>
            {
                try
                {
                    return x.Run();
                }
                finally
                {
                    finallyAction();
                }
            });
        }

        public static Maybe<T> OnValue<T>(this Maybe<T> @this, Action<T> action)
        {
            Guard.NotNull(action, "action");
            return @this.Select(x =>
            {
                action(x);
                return x;
            });
        }

        public static Maybe<T> OnNoValue<T>(this Maybe<T> @this, Action action)
        {
            Guard.NotNull(action, "action");

            return @this.Express(x =>
            {
                if (x.Exception == null && x.HasValue != true)
                    action();

                return x;
            });
        }

        public static Maybe<T> OnException<T>(this Maybe<T> @this, Action<Exception> handler)
        {
            Guard.NotNull(handler, "handler");
            return @this.Express(x =>
            {
                Exception exception = null;

                try
                {
                    exception = x.Exception;
                }
                catch(Exception ex)
                {
                    handler(ex);
                    throw;
                }

                if (exception != null)
                    handler(exception);

                return x;
            });
        }

        public static Maybe<T> Where<T>(this Maybe<T> @this, Func<T, bool> predicate)
        {
            Guard.NotNull(predicate, "predicate");
            return @this.SelectMaybe(x => predicate(x) ? x.ToMaybe() : Maybe<T>.NoValue);
        }

        public static Maybe<T> Unless<T>(this Maybe<T> @this, Func<T, bool> predicate)
        {
            Guard.NotNull(predicate, "predicate");
            return @this.SelectMaybe(x => predicate(x) ? Maybe<T>.NoValue : x.ToMaybe());
        }

        public static Maybe<T> Assign<T>(this Maybe<T> @this, ref T target)
        {
            if (@this.HasValue)
                target = @this.Value;

            return @this;
        }

        public static Maybe<T> Run<T>(this Maybe<T> @this, Action<T> action = null)
        {
            // Calling HasValue forces evaluation of the Maybe<T>.
            // Returning @this in either code path allows additional
            // operators to be invoked that are lazily evaluated.
            return @this
                .When(x => action != null, @this.Express(x => x.OnValue(action)))
                .HasValue ? @this : @this;
        }

        public static Maybe<T> RunAsync<T>(this Maybe<T> @this, Action<T> action = null, CancellationToken cancellationToken = default(CancellationToken), TaskCreationOptions taskCreationOptions = TaskCreationOptions.None, TaskScheduler taskScheduler = default(TaskScheduler))
        {
            var task = Task.Factory.StartNew(() => @this.Run(action), cancellationToken, taskCreationOptions,
                                             taskScheduler ?? TaskScheduler.Current);

            return Return(task)
                .OnValue(t => t.Wait(cancellationToken))
                .SelectMaybe(t => t.IsCanceled ? Maybe<T>.NoValue : t.Result);
        }

        public static Maybe<T> Synchronize<T>(this Maybe<T> @this)
        {
            return Synchronize(@this, new object());
        }

        public static Maybe<T> Synchronize<T>(this Maybe<T> @this, object gate)
        {
            Guard.NotNull(gate, "gate");

            var self = @this;
            return new Maybe<T>(() =>
            {
                lock (gate)
                {
                    return self.Run();
                }
            });
        }

        public static Maybe<TResult> Cast<TResult>(this IMaybe @this)
        {
            if(@this == null)
                return Maybe<TResult>.NoValue;

            if(@this is Maybe<TResult>)
                return (Maybe<TResult>) @this;

            return new Maybe<TResult>(() =>
            {
                if (@this.Exception != null)
                    return new Maybe<TResult>(@this.Exception);

                if (@this.HasValue != true)
                    return Maybe<TResult>.NoValue;

                if (!(@this.Value is TResult))
                    return new Maybe<TResult>(new InvalidCastException());

                return ((TResult) @this.Value).ToMaybe();
            });
        }

        public static Maybe<TResult> OfType<TResult>(this IMaybe @this)
        {
            if (@this == null)
                return Maybe<TResult>.NoValue;

            if (@this is Maybe<TResult>)
                return (Maybe<TResult>) @this;

            return new Maybe<TResult>(() =>
            {
                if (@this.Exception != null)
                    return new Maybe<TResult>(@this.Exception);

                if (@this.HasValue != true)
                    return Maybe<TResult>.NoValue;

                if (@this.Value is TResult)
                    return ((TResult) @this.Value).ToMaybe();

                return Maybe<TResult>.NoValue;
            });
        }

        public static T? ToNullable<T>(this Maybe<T> @this) where T : struct
        {
            return @this.Select(x => (T?)x)
                .Or((T?)null)
                .ValueOrDefault();
        }

        // Conventionally, in LINQ, the monadic "return" operator is written "To...,"
        // as in "ToList," "ToArray," etc. These are synonyms for Return.
        public static Maybe<T> ToMaybe<T>(this T value)
        {
            return Return(value);
        }

        public static Maybe<T> AsMaybe<T>(this IMaybe<T> value)
        {
            return value.Cast<T>();
        }

        public static Maybe<object> AsMaybe(this IMaybe value)
        {
            return value.Cast<object>();
        }
    }
}
