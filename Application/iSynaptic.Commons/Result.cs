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
    public struct Result<T, TObservation> : IResult<T, TObservation>, IEquatable<Result<T, TObservation>>, IEquatable<Maybe<T>>, IEquatable<T>
    {
        public static readonly Result<T, TObservation> NoValue;

        private readonly Maybe<T> _Maybe;
        private readonly Outcome<TObservation> _Outcome;

        private readonly Func<Result<T, TObservation>> _Computation;

        public Result(T value)
            : this(new Maybe<T>(value), Outcome<TObservation>.Success)
        {
        }

        public Result(Maybe<T> maybe, Outcome<TObservation> outcome)
            : this()
        {
            _Maybe = maybe.Run();
            _Outcome = outcome.Run();
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

        private Maybe<T> Maybe
        {
            get
            {
                if (_Computation != null)
                {
                    var self = this;
                    return new Maybe<T>(() => self._Computation().Maybe);
                }

                return _Maybe;
            }
        }

        private Outcome<TObservation> Outcome
        {
            get
            {
                if(_Computation != null)
                {
                    var self = this;
                    return new Outcome<TObservation>(() => self._Computation().Outcome);
                }

                return _Outcome;
            }
        }

        public T Value { get { return Maybe.Value; } }
        public bool HasValue { get { return Maybe.HasValue; } }

        public bool WasSuccessful { get { return Outcome.WasSuccessful; } }
        public IEnumerable<TObservation> Observations { get { return Outcome.Observations; } }

        object IMaybe.Value { get { return ((IMaybe)Maybe).Value; } }
        IEnumerable<object> IOutcome.Observations { get { return ((IOutcome)Outcome).Observations; }}

        #region Equality Implementation

        public bool Equals(T other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        public bool Equals(T other, IEqualityComparer<T> comparer)
        {
            return Maybe.Equals(other, comparer);
        }

        public bool Equals(Maybe<T> other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        public bool Equals(Maybe<T> other, IEqualityComparer<T> comparer)
        {
            return Maybe.Equals(other, comparer);
        }

        public bool Equals(Result<T, TObservation> other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        public bool Equals(Result<T, TObservation> other, IEqualityComparer<T> comparer)
        {
            Guard.NotNull(comparer, "comparer");

            return Maybe.Equals(other.Maybe, comparer) &&
                   Outcome.WasSuccessful == other.Outcome.WasSuccessful;
        }

        public override bool Equals(object obj)
        {
            if (obj is Result<T, TObservation>)
                return Equals((Result<T, TObservation>)obj);

            if (obj is Maybe<T>)
                return Equals((Maybe<T>) obj);

            if (obj is T)
                return Equals((T) obj);

            return false;
        }

        public override int GetHashCode()
        {
            return GetHashCode(EqualityComparer<T>.Default);
        }

        public int GetHashCode(IEqualityComparer<T> comparer)
        {
            Guard.NotNull(comparer, "comparer");

            return Maybe.GetHashCode(comparer) ^
                   Outcome.WasSuccessful.GetHashCode();
        }

        #endregion

        #region Equality Operators

        public static bool operator ==(Result<T, TObservation> left, Result<T, TObservation> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Result<T, TObservation> left, Result<T, TObservation> right)
        {
            return !(left == right);
        }

        public static bool operator ==(Result<T, TObservation> left, Maybe<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Result<T, TObservation> left, Maybe<T> right)
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

        public static bool operator ==(Maybe<T> left, Result<T, TObservation> right)
        {
            return right.Equals(left);
        }

        public static bool operator !=(Maybe<T> left, Result<T, TObservation> right)
        {
            return !(left == right);
        }

        #endregion

        public Maybe<T> ToMaybe()
        {
            return Maybe;
        }

        public Outcome<TObservation> ToOutcome()
        {
            return Outcome;
        }

        public static explicit operator T(Result<T, TObservation> value)
        {
            return value.Value;
        }

        public static implicit operator bool(Result<T, TObservation> result)
        {
            return result.WasSuccessful;
        }

        public static implicit operator Result<T, TObservation>(Result<T, Unit> result)
        {
            return new Result<T, TObservation>(result.ToMaybe(), new Outcome<TObservation>());
        }

        public static implicit operator Result<T, TObservation>(Result<Unit, TObservation> result)
        {
            return new Result<T, TObservation>(new Maybe<T>(), result.ToOutcome());
        }

        public static implicit operator Result<T, TObservation>(Result<Unit, Unit> result)
        {
            return new Result<T, TObservation>();
        }

    }

    public static class Result
    {
        public static Result<Unit, Unit> NoValue
        {
            get { return new Result<Unit, Unit>(); }
        }

        public static Result<T, Unit> Return<T>(T value)
        {
            return new Result<T, Unit>(value);
        }

        public static Result<Unit, Unit> Success()
        {
            return new Result<Unit, Unit>();
        }

        public static Result<Unit, TObservation> Success<TObservation>(TObservation observation)
        {
            return new Result<Unit, TObservation>(new Maybe<Unit>(), Outcome.Success(observation));
        }

        public static Result<Unit, TObservation> Success<TObservation>(params TObservation[] observations)
        {
            return Success((IEnumerable<TObservation>) observations);
        }

        public static Result<Unit, TObservation> Success<TObservation>(IEnumerable<TObservation> observations)
        {
            return new Result<Unit, TObservation>(new Maybe<Unit>(), Outcome.Success(observations));
        }

        public static Result<Unit, Unit> Failure()
        {
            return new Result<Unit, Unit>(new Maybe<Unit>(), Outcome.Failure());
        }

        public static Result<Unit, TObservation> Failure<TObservation>(TObservation observation)
        {
            return new Result<Unit, TObservation>(new Maybe<Unit>(), Outcome.Failure(observation));
        }

        public static Result<Unit, TObservation> Failure<TObservation>(params TObservation[] observations)
        {
            return Failure((IEnumerable<TObservation>)observations);
        }

        public static Result<Unit, TObservation> Failure<TObservation>(IEnumerable<TObservation> observations)
        {
            return new Result<Unit, TObservation>(new Maybe<Unit>(), Outcome.Failure(observations));
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
                var selectedResult = self.ToMaybe().Select(selector).Value;
                return new Result<TResult, TObservation>(selectedResult.ToMaybe(), self.ToOutcome().Combine(selectedResult.ToOutcome()));
            });
        }

        public static Result<TResult, TObservation> Select<T, TResult, TObservation>(this Result<T, TObservation> @this, Func<T, TResult> selector)
        {
            Guard.NotNull(selector, "selector");

            var self = @this;
            return new Result<TResult, TObservation>(() =>
            {
                var selectedResult = self.ToMaybe().Select(selector);
                return new Result<TResult, TObservation>(selectedResult, self.ToOutcome());
            });
        }

        public static Result<T, TObservation> Where<T, TObservation>(this Result<T, TObservation> @this, Func<T, bool> predicate)
        {
            Guard.NotNull(predicate, "predicate");
            var self = @this;

            return new Result<T, TObservation>(() => new Result<T, TObservation>(self.ToMaybe().Where(predicate), self.ToOutcome()));
        }

        public static Result<T, TObservation> Observe<T, TObservation>(this Result<T, Unit> @this, TObservation observation)
        {
            var self = @this;
            return new Result<T, TObservation>(() => new Result<T, TObservation>(self.ToMaybe(), self.ToOutcome().Observe(observation)));
        }

        public static Result<T, TObservation> Observe<T, TObservation>(this Result<T, TObservation> @this, TObservation observation)
        {
            var self = @this;
            return new Result<T, TObservation>(() => new Result<T, TObservation>(self.ToMaybe(), self.ToOutcome().Observe(observation)));
        }

        public static Result<T, TObservation> Observe<T, TObservation>(this Result<T, Unit> @this, Func<Maybe<T>, TObservation> selector)
        {
            Guard.NotNull(selector, "selector");

            var self = @this;
            return new Result<T, TObservation>(() => new Result<T, TObservation>(self.ToMaybe(), self.ToOutcome().Observe(selector(self.ToMaybe()))));
        }

        public static Result<T, TObservation> Observe<T, TObservation>(this Result<T, TObservation> @this, Func<Maybe<T>, TObservation> selector)
        {
            Guard.NotNull(selector, "selector");

            var self = @this;
            return new Result<T, TObservation>(() => new Result<T, TObservation>(self.ToMaybe(), self.ToOutcome().Observe(selector(self.ToMaybe()))));
        }

        public static Result<T, TObservation> ObserveMany<T, TObservation>(this Result<T, Unit> @this, params TObservation[] observations)
        {
            return ObserveMany(@this, (IEnumerable<TObservation>)observations);
        }

        public static Result<T, TObservation> ObserveMany<T, TObservation>(this Result<T, Unit> @this, IEnumerable<TObservation> observations)
        {
            var self = @this;
            return new Result<T, TObservation>(() => new Result<T, TObservation>(self.ToMaybe(), self.ToOutcome().ObserveMany(observations)));
        }

        public static Result<T, TObservation> ObserveMany<T, TObservation>(this Result<T, TObservation> @this, params TObservation[] observations)
        {
            return ObserveMany(@this, (IEnumerable<TObservation>)observations);
        }

        public static Result<T, TObservation> ObserveMany<T, TObservation>(this Result<T, TObservation> @this, IEnumerable<TObservation> observations)
        {
            var self = @this;
            return new Result<T, TObservation>(() => new Result<T, TObservation>(self.ToMaybe(), self.ToOutcome().ObserveMany(observations)));
        }

        public static Result<T, TObservation> ObserveMany<T, TObservation>(this Result<T, Unit> @this, Func<Maybe<T>, IEnumerable<TObservation>> selector)
        {
            Guard.NotNull(selector, "selector");

            var self = @this;
            return new Result<T, TObservation>(() => new Result<T, TObservation>(self.ToMaybe(), self.ToOutcome().ObserveMany(selector(self.ToMaybe()))));
        }

        public static Result<T, TObservation> ObserveMany<T, TObservation>(this Result<T, TObservation> @this, Func<Maybe<T>, IEnumerable<TObservation>> selector)
        {
            Guard.NotNull(selector, "selector");

            var self = @this;
            return new Result<T, TObservation>(() => new Result<T, TObservation>(self.ToMaybe(), self.ToOutcome().ObserveMany(selector(self.ToMaybe()))));
        }

        public static Result<T, TResult> Inform<T, TObservation, TResult>(this Result<T, TObservation> @this, Func<TObservation, TResult> selector)
        {
            Guard.NotNull(selector, "selector");

            var self = @this;
            return new Result<T, TResult>(() => new Result<T, TResult>(self.ToMaybe(), self.ToOutcome().Inform(selector)));
        }

        public static Result<T, TResult> InformMany<T, TObservation, TResult>(this Result<T, TObservation> @this, Func<TObservation, Outcome<TResult>> selector)
        {
            Guard.NotNull(selector, "selector");

            var self = @this;
            return new Result<T, TResult>(() => new Result<T, TResult>(self.ToMaybe(), self.ToOutcome().InformMany(selector)));
        }

        public static Result<T, TObservation> Notice<T, TObservation>(this Result<T, TObservation> @this, Func<TObservation, bool> predicate)
        {
            Guard.NotNull(predicate, "predicate");

            var self = @this;
            return new Result<T, TObservation>(() => new Result<T, TObservation>(self.ToMaybe(), self.ToOutcome().Notice(predicate)));
        }

        public static Result<T, TObservation> Ignore<T, TObservation>(this Result<T, TObservation> @this, Func<TObservation, bool> predicate)
        {
            Guard.NotNull(predicate, "predicate");

            var self = @this;
            return new Result<T, TObservation>(() => new Result<T, TObservation>(self.ToMaybe(), self.ToOutcome().Ignore(predicate)));
        }

        public static Result<T, TObservation> Combine<T, TObservation>(this Result<T, TObservation> @this, Outcome<TObservation> other)
        {
            return Combine(@this, new[] { other });
        }

        public static Result<T, TObservation> Combine<T, TObservation>(this Result<T, TObservation> @this, params Outcome<TObservation>[] outcomes)
        {
            return Combine(@this, (IEnumerable<Outcome<TObservation>>)outcomes);
        }

        public static Result<T, TObservation> Combine<T, TObservation>(this Result<T, TObservation> @this, IEnumerable<Outcome<TObservation>> outcomes)
        {
            Guard.NotNull(outcomes, "outcomes");
            var self = @this;

            return new Result<T, TObservation>(() => new Result<T, TObservation>(self.ToMaybe(), self.ToOutcome().Combine(outcomes)));
        }

        public static Result<T, TObservation> FailIf<T, TObservation>(this Result<T, TObservation> @this, bool predicate)
        {
            return @this.FailIf(() => predicate);
        }

        public static Result<T, TObservation> FailIf<T, TObservation>(this Result<T, TObservation> @this, Func<bool> predicate)
        {
            Guard.NotNull(predicate, "predicate");
            var self = @this;

            return new Result<T, TObservation>(() => new Result<T, TObservation>(self.ToMaybe(), self.ToOutcome().FailIf(predicate)));
        }

        public static Result<T, TObservation> FailIf<T, TObservation>(this Result<T, Unit> @this, bool predicate, TObservation failureObservation)
        {
            return @this.FailIf(predicate, () => failureObservation);
        }

        public static Result<T, TObservation> FailIf<T, TObservation>(this Result<T, TObservation> @this, bool predicate, TObservation failureObservation)
        {
            return @this.FailIf(predicate, () => failureObservation);
        }

        public static Result<T, TObservation> FailIf<T, TObservation>(this Result<T, Unit> @this, bool predicate, Func<TObservation> failureObservation)
        {
            return @this.FailIf(() => predicate, failureObservation);
        }

        public static Result<T, TObservation> FailIf<T, TObservation>(this Result<T, TObservation> @this, bool predicate, Func<TObservation> failureObservation)
        {
            return @this.FailIf(() => predicate, failureObservation);
        }

        public static Result<T, TObservation> FailIf<T, TObservation>(this Result<T, Unit> @this, Func<bool> predicate, Func<TObservation> failureObservation)
        {
            Guard.NotNull(predicate, "predicate");
            Guard.NotNull(failureObservation, "failureObservation");

            var self = @this;

            return new Result<T, TObservation>(() => new Result<T, TObservation>(self.ToMaybe(), self.ToOutcome().FailIf(predicate, failureObservation)));
        }

        public static Result<T, TObservation> FailIf<T, TObservation>(this Result<T, TObservation> @this, Func<bool> predicate, Func<TObservation> failureObservation)
        {
            Guard.NotNull(predicate, "predicate");
            Guard.NotNull(failureObservation, "failureObservation");

            var self = @this;

            return new Result<T, TObservation>(() => new Result<T, TObservation>(self.ToMaybe(), self.ToOutcome().FailIf(predicate, failureObservation)));
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

        public static Result<T, TObservation> OfType<T, TObservation>(this IResult @this)
        {
            if (@this == null)
                return NoValue;

            if (@this is Result<T, TObservation>)
                return (Result<T, TObservation>)@this;

            var self = @this;
            return new Result<T, TObservation>(() => new Result<T, TObservation>(self.AsMaybe().OfType<T>(), self.AsOutcome().OfType<TObservation>()));
        }

        public static Result<T, TObservation> AsResult<T, TObservation>(this IResult<T, TObservation> value)
        {
            return value.OfType<T, TObservation>();
        }

        public static Result<object, object> AsResult(this IResult value)
        {
            return value.OfType<object, object>();
        }
    }
}
