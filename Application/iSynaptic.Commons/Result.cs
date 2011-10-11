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

namespace iSynaptic.Commons
{
    public struct Result<T, TObservation> : IEquatable<Result<T, TObservation>>, IEquatable<T>
    {
        public static readonly Result<T, TObservation> NoValue = new Result<T, TObservation>();
        public static readonly Result<T, TObservation> Default = new Result<T, TObservation>(default(T));

        private readonly T _Value;
        private readonly bool _HasValue;
        private readonly TObservation[] _Observations;

        private readonly Func<Result<T, TObservation>> _Computation;

        public Result(IEnumerable<TObservation> observations)
            : this()
        {
            _HasValue = false;
            _Observations = observations != null
                                ? observations.ToArray()
                                : null;
        }

        public Result(T value, params TObservation[] observations)
            : this()
        {
            _Value = value;
            _HasValue = true;
            _Observations = observations;
        }

        public Result(T value, IEnumerable<TObservation> observations)
            : this()
        {
            _Value = value;
            _HasValue = true;

            _Observations = observations != null
                                ? observations.ToArray()
                                : null;
        }

        public Result(Func<T> computation)
            : this(() => new Result<T, TObservation>(computation()))
        {
            Guard.NotNull(computation, "computation");
        }

        public Result(Func<Result<T, TObservation>> computation)
            : this()
        {
            var cachedComputation = Guard.NotNull(computation, "computation");
            var memoizedResult = default(Result<T, TObservation>);
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

        public T Value
        {
            get
            {
                if (_Computation != null)
                    return _Computation().Value;

                if (_HasValue)
                    return _Value;

                throw new InvalidOperationException("No value can be computed.");
            }
        }

        public bool HasValue
        {
            get
            {
                if (_Computation == null)
                    return _HasValue;

                return _Computation().HasValue;
            }
        }

        public IEnumerable<TObservation> Observations
        {
            get
            {
                if (_Computation != null)
                {
                    foreach (var observation in _Computation().Observations)
                        yield return observation;
                }

                if (_Observations != null)
                {
                    foreach (var observation in _Observations)
                        yield return observation;
                }
            }
        }

        public bool Equals(T other)
        {
            return Equals(new Result<T, TObservation>(other));
        }

        public bool Equals(Result<T, TObservation> other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        public bool Equals(Result<T, TObservation> other, IEqualityComparer<T> comparer)
        {
            Guard.NotNull(comparer, "comparer");

            if (!HasValue)
                return !other.HasValue;

            return other.HasValue && comparer.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            if (obj is Result<T, TObservation>)
                return Equals((Result<T, TObservation>)obj);

            if (obj is T)
                return Equals(new Result<T, TObservation>((T)obj));

            return false;
        }

        public override int GetHashCode()
        {
            return GetHashCode(EqualityComparer<T>.Default);
        }

        public int GetHashCode(IEqualityComparer<T> comparer)
        {
            Guard.NotNull(comparer, "comparer");

            if (HasValue != true)
                return -1;

            return comparer.GetHashCode(Value);
        }

        public static bool operator ==(Result<T, TObservation> left, Result<T, TObservation> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Result<T, TObservation> left, Result<T, TObservation> right)
        {
            return !(left == right);
        }

        public static bool operator ==(Result<T, TObservation> left, T right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Result<T, TObservation> left, T right)
        {
            return !(left == right);
        }

        public static bool operator ==(T left, Result<T, TObservation> right)
        {
            return right.Equals(left);
        }

        public static bool operator !=(T left, Result<T, TObservation> right)
        {
            return !(left == right);
        }

        public static explicit operator T(Result<T, TObservation> value)
        {
            return value.Value;
        }
    }

    public static class Result
    {
        public static Result<T, Unit> Return<T>(T value)
        {
            return new Result<T, Unit>(value);
        }

        public static Result<T, TObservation> Return<T, TObservation>(T value)
        {
            return new Result<T, TObservation>(value);
        }

        public static Result<TResult, TObservation> Bind<T, TResult, TObservation>(this Result<T, TObservation> @this, Func<T, Result<TResult, TObservation>> selector)
        {
            Guard.NotNull(selector, "selector");

            var self = @this;

            return new Result<TResult, TObservation>(() =>
            {
                if (self.HasValue)
                {
                    var selectedResult = selector(self.Value);
                    var combinedObservations = self.Observations.Concat(selectedResult.Observations);

                    if(selectedResult.HasValue)
                        return new Result<TResult, TObservation>(selectedResult.Value, combinedObservations);
                    
                    return new Result<TResult, TObservation>(combinedObservations);
                }

                return new Result<TResult, TObservation>(self.Observations);
            });
        }

        public static Result<TResult, Unit> Bind<T, TResult>(this Result<T, Unit> @this, Func<T, Result<TResult, Unit>> selector)
        {
            Guard.NotNull(selector, "selector");

            var self = @this;
            return new Result<TResult, Unit>(() => self.HasValue ? selector(self.Value) : Result<TResult, Unit>.NoValue);
        }

        #region SelectMany Operator

        // This is an alias of Bind, and exists only to satisfy C#'s LINQ comprehension syntax.
        // The name "SelectMany" is confusing as there is only one value to "select".
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Result<TResult, TObservation> SelectMany<T, TResult, TObservation>(this Result<T, TObservation> @this, Func<T, Result<TResult, TObservation>> selector)
        {
            return Bind(@this, selector);
        }

        // This operator is implemented only to satisfy C#'s LINQ comprehension syntax. 
        // The name "SelectMany" is confusing as there is only one value to "select".
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Result<TResult, TObservation> SelectMany<T, TIntermediate, TResult, TObservation>(this Result<T, TObservation> @this, Func<T, Result<TIntermediate, TObservation>> selector, Func<T, TIntermediate, TResult> combiner)
        {
            Guard.NotNull(selector, "selector");
            Guard.NotNull(combiner, "combiner");

            return @this.SelectMany(x => selector(x).Select(y => combiner(x, y)));
        }

        #endregion

        public static Result<TResult, TObservation> SelectResult<T, TResult, TObservation>(this Result<T, TObservation> @this, Func<T, Result<TResult, TObservation>> selector)
        {
            return Bind(@this, selector);
        }

        public static Result<TResult, Unit> SelectResult<T, TResult>(this Result<T, Unit> @this, Func<T, Result<TResult, Unit>> selector)
        {
            return Bind(@this, selector);
        }

        public static Result<TResult, TObservation> Select<T, TResult, TObservation>(this Result<T, TObservation> @this, Func<T, TResult> selector)
        {
            Guard.NotNull(selector, "selector");

            var self = @this;
            return self.Bind(x => new Result<TResult, TObservation>(selector(x), self.Observations));
        }

        public static Result<T, TObservation> Where<T, TObservation>(this Result<T, TObservation> @this, Func<T, bool> predicate)
        {
            Guard.NotNull(predicate, "predicate");
            var self = @this;

            return new Result<T, TObservation>(() =>
            {
                if (self.HasValue)
                {
                    var value = self.Value;

                    if (predicate(value))
                        return self;
                }

                return new Result<T, TObservation>(self.Observations);
            });
        }

        public static Result<T, Unit> ToResult<T>(this T value)
        {
            return new Result<T, Unit>(value);
        }
    }
}
