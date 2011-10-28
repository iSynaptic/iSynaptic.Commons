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
            _HasValue = value != null;

            _Observations = observations;
        }

        public Result(T value, IEnumerable<TObservation> observations)
            : this()
        {
            _Value = value;
            _HasValue = value != null;

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
                return _Computation != null 
                    ? _Computation().HasValue 
                    : _HasValue;
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
                return 0;

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
        public static Result<T, TObservation> Return<T, TObservation>(T value)
        {
            return new Result<T, TObservation>(value);
        }

        public static Result<TResult, TObservation> Bind<T, TResult, TObservation>(this Result<T, TObservation> @this, Func<T, Result<TResult, TObservation>> selector)
        {
            return SelectResult(@this, selector);
        }

        public static Result<TResult, TObservation> SelectResult<T, TResult, TObservation>(this Result<T, TObservation> @this, Func<T, Result<TResult, TObservation>> selector)
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

        public static Result<TResult, TObservation> Select<T, TResult, TObservation>(this Result<T, TObservation> @this, Func<T, TResult> selector)
        {
            Guard.NotNull(selector, "selector");

            var self = @this;
            return self.SelectResult(x => new Result<TResult, TObservation>(selector(x), self.Observations));
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

        public static Result<T, TObservation> Observe<T, TObservation>(this Result<T, TObservation> @this, TObservation observation)
        {
            var self = @this;
            return new Result<T, TObservation>(() => self.HasValue
                ? new Result<T, TObservation>(self.Value, self.Observations.Concat(new []{observation}))
                : new Result<T, TObservation>(self.Observations.Concat(new[] { observation })));
        }

        public static Result<T, TObservation> Observe<T, TObservation>(this Result<T, TObservation> @this, Func<Maybe<T>, TObservation> selector)
        {
            Guard.NotNull(selector, "selector");

            var self = @this;
            return new Result<T, TObservation>(() => self.HasValue
                ? new Result<T, TObservation>(self.Value, self.Observations.Concat(new []{selector(new Maybe<T>(self.Value))}))
                : new Result<T, TObservation>(self.Observations.Concat(new[] { selector(Maybe<T>.NoValue) })));
        }

        public static Result<T, TObservation> ObserveMany<T, TObservation>(this Result<T, TObservation> @this, IEnumerable<TObservation> observations)
        {
            Guard.NotNull(observations, "observations");

            var self = @this;
            return new Result<T, TObservation>(() => self.HasValue
                ? new Result<T, TObservation>(self.Value, self.Observations.Concat(observations))
                : new Result<T, TObservation>(self.Observations.Concat(observations)));
        }

        public static Result<T, TObservation> ObserveMany<T, TObservation>(this Result<T, TObservation> @this, Func<Maybe<T>, IEnumerable<TObservation>> selector)
        {
            Guard.NotNull(selector, "selector");

            var self = @this;
            return new Result<T, TObservation>(() => self.HasValue
                ? new Result<T, TObservation>(self.Value, self.Observations.Concat(selector(new Maybe<T>(self.Value))))
                : new Result<T, TObservation>(self.Observations.Concat(selector(Maybe<T>.NoValue))));
        }

        public static Result<T, TResult> Inform<T, TObservation, TResult>(this Result<T, TObservation> @this, Func<TObservation, TResult> selector)
        {
            Guard.NotNull(selector, "selector");

            var self = @this;
            return new Result<T, TResult>(() => self.HasValue
                ? new Result<T, TResult>(self.Value, self.Observations.Select(selector))
                : new Result<T, TResult>(self.Observations.Select(selector)));
        }

        public static Result<T, TResult> InformMany<T, TObservation, TResult>(this Result<T, TObservation> @this, Func<TObservation, IEnumerable<TResult>> selector)
        {
            Guard.NotNull(selector, "selector");

            var self = @this;
            return new Result<T, TResult>(() => self.HasValue
                ? new Result<T, TResult>(self.Value, self.Observations.SelectMany(selector))
                : new Result<T, TResult>(self.Observations.SelectMany(selector)));
        }

        public static Result<T, TObservation> Notice<T, TObservation>(this Result<T, TObservation> @this, Func<TObservation, bool> predicate)
        {
            Guard.NotNull(predicate, "selector");

            var self = @this;
            return new Result<T, TObservation>(() => self.HasValue
                ? new Result<T, TObservation>(self.Value, self.Observations.Where(predicate))
                : new Result<T, TObservation>(self.Observations.Where(predicate)));
        }

        public static Result<T, TObservation> Ignore<T, TObservation>(this Result<T, TObservation> @this, Func<TObservation, bool> predicate)
        {
            Guard.NotNull(predicate, "selector");

            var self = @this;
            return new Result<T, TObservation>(() => self.HasValue
                ? new Result<T, TObservation>(self.Value, self.Observations.Where(x => !predicate(x)))
                : new Result<T, TObservation>(self.Observations.Where(x => !predicate(x))));
        }

        public static Result<T, TObservation> Run<T, TObservation>(this Result<T, TObservation> @this, Action<T> action = null)
        {
            // Getting HasValue forces evaluation
            if (@this.HasValue && action != null)
                action(@this.Value);

            return @this;
        }

        public static Result<T, TObservation> ToResult<T, TObservation>(this T value)
        {
            return new Result<T, TObservation>(value);
        }
    }
}
