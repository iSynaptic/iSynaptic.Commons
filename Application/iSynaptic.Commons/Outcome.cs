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
using System.Linq;

namespace iSynaptic.Commons
{
    public struct Outcome<TObservation>
    {
        public static readonly Outcome<TObservation> Success = new Outcome<TObservation>(true);
        public static readonly Outcome<TObservation> Failure = new Outcome<TObservation>(false);

        private readonly bool _IsFailure;
        private readonly TObservation[] _Observations;
        private readonly Func<Outcome<TObservation>> _Computation;

        public Outcome(bool isSuccess) : this()
        {
            _IsFailure = !isSuccess;
        }

        public Outcome(bool isSuccess, TObservation observation) : this()
        {
            _IsFailure = !isSuccess;
            _Observations = new[] { observation };
        }

        public Outcome(bool isSuccess, IEnumerable<TObservation> observations)
            : this()
        {
            _IsFailure = !isSuccess;
            _Observations = Guard.NotNull(observations, "observations").ToArray();
        }

        public Outcome(Func<Outcome<TObservation>> computation)
            : this()
        {
            var cachedComputation = Guard.NotNull(computation, "computation");
            var memoizedResult = default(Outcome<TObservation>);
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

        public IEnumerable<TObservation> Observations
        {
            get
            {
                if (_Computation != null)
                {
                    foreach (var observation in _Computation().Observations)
                        yield return observation;
                }
                else if (_Observations != null)
                {
                    foreach (var observation in _Observations)
                        yield return observation;
                }
            }
        }

        public bool WasSuccessful
        {
            get
            {
                if (_Computation != null)
                    return _Computation().WasSuccessful;

                return !_IsFailure;
            }
        }

        public static Outcome<TObservation> operator&(Outcome<TObservation> left, Outcome<TObservation> right)
        {
            return new Outcome<TObservation>(() => new Outcome<TObservation>(left.WasSuccessful & right.WasSuccessful, left.Observations.Concat(right.Observations)));
        }

        public static implicit operator bool(Outcome<TObservation> outcome)
        {
            return outcome.WasSuccessful;
        }
    }

    public static class Outcome
    {
        public static Outcome<TObservation> Return<TObservation>(TObservation value)
        {
            return Success(value);
        }

        public static Outcome<TResult> Bind<TObservation, TResult>(this Outcome<TObservation> @this, Func<TObservation, Outcome<TResult>> selector)
        {
            return InformMany(@this, selector);
        }

        public static Outcome<TResult> Let<TObservation, TResult>(this Outcome<TObservation> @this, Func<Outcome<TObservation>, Outcome<TResult>> selector)
        {
            Guard.NotNull(selector, "selector");
            var self = @this;

            return new Outcome<TResult>(() => selector(self));
        }

        public static Outcome<TResult> Inform<TObservation, TResult>(this Outcome<TObservation> @this, Func<TObservation, TResult> selector)
        {
            Guard.NotNull(selector, "selector");
            return @this.InformMany(t => new Outcome<TResult>(@this.WasSuccessful, selector(t)));
        }

        public static Outcome<TResult> InformMany<TObservation, TResult>(this Outcome<TObservation> @this, Func<TObservation, Outcome<TResult>> selector)
        {
            Guard.NotNull(selector, "selector");

            var self = @this;
            return new Outcome<TResult>(() =>
            {
                var outcomes = self.Observations
                    .Select(selector)
                    .ToArray();

                return new Outcome<TResult>(outcomes.All(x => x.WasSuccessful), outcomes.SelectMany(x => x.Observations));
            });
        }

        public static Outcome<TObservation> Success<TObservation>(TObservation observation)
        {
            return new Outcome<TObservation>(true, observation);
        }

        public static Outcome<TObservation> Failure<TObservation>(TObservation observation)
        {
            return new Outcome<TObservation>(false, observation);
        }

        public static Outcome<TObservation> Notice<TObservation>(this Outcome<TObservation> @this, Func<TObservation, bool> predicate)
        {
            Guard.NotNull(predicate, "predicate");
            return @this.InformMany(t => predicate(t) ? new Outcome<TObservation>(@this.WasSuccessful, t) : new Outcome<TObservation>(@this.WasSuccessful));
        }

        public static Outcome<TObservation> Ignore<TObservation>(this Outcome<TObservation> @this, Func<TObservation, bool> predicate)
        {
            Guard.NotNull(predicate, "predicate");
            return @this.InformMany(t => predicate(t) ? new Outcome<TObservation>(@this.WasSuccessful) : new Outcome<TObservation>(@this.WasSuccessful, t));
        }

        public static Outcome<TObservation> Observe<TObservation>(this Outcome<TObservation> @this, TObservation observation)
        {
            var self = @this;
            return new Outcome<TObservation>(() => new Outcome<TObservation>(self.WasSuccessful, self.Observations.Concat(new[]{observation})));
        }

        public static Outcome<TObservation> Observe<TObservation>(this Outcome<TObservation> @this, Func<bool, TObservation> selector)
        {
            Guard.NotNull(selector, "selector");
            var self = @this;

            return new Outcome<TObservation>(() => new Outcome<TObservation>(self.WasSuccessful, self.Observations.Concat(new[] { selector(self.WasSuccessful) })));
        }

        public static Outcome<TObservation> ObserveMany<TObservation>(this Outcome<TObservation> @this, IEnumerable<TObservation> observations)
        {
            Guard.NotNull(observations, "observations");

            var self = @this;
            return new Outcome<TObservation>(() => new Outcome<TObservation>(self.WasSuccessful, self.Observations.Concat(observations)));
        }

        public static Outcome<TObservation> ObserveMany<TObservation>(this Outcome<TObservation> @this, Func<bool, IEnumerable<TObservation>> selector)
        {
            Guard.NotNull(selector, "selector");
            var self = @this;

            return new Outcome<TObservation>(() => new Outcome<TObservation>(self.WasSuccessful, self.Observations.Concat(selector(self.WasSuccessful))));
        }

        public static Outcome<TObservation> FailIf<TObservation>(this Outcome<TObservation> @this, bool predicate)
        {
            return @this.FailIf(() => predicate);
        }

        public static Outcome<TObservation> FailIf<TObservation>(this Outcome<TObservation> @this, Func<bool> predicate)
        {
            Guard.NotNull(predicate, "predicate");
            var self = @this;

            return new Outcome<TObservation>(() => new Outcome<TObservation>(self.WasSuccessful && !predicate(), self.Observations));
        }

        public static Outcome<TObservation> FailIf<TObservation>(this Outcome<TObservation> @this, bool predicate, TObservation failureObservation)
        {
            return @this.FailIf(predicate, () => failureObservation);
        }

        public static Outcome<TObservation> FailIf<TObservation>(this Outcome<TObservation> @this, bool predicate, Func<TObservation> failureObservation)
        {
            return @this.FailIf(() => predicate, failureObservation);
        }

        public static Outcome<TObservation> FailIf<TObservation>(this Outcome<TObservation> @this, Func<bool> predicate, Func<TObservation> failureObservation)
        {
            Guard.NotNull(predicate, "predicate");
            Guard.NotNull(failureObservation, "failureObservation");

            var self = @this;

            return new Outcome<TObservation>(() =>
            {
                var failed = predicate();
                var observations = failed
                    ? self.Observations.Concat(new[] { failureObservation() })
                    : self.Observations;

                return new Outcome<TObservation>(self.WasSuccessful && !failed, observations);
            });
        }

        public static Outcome<TObservation> Run<TObservation>(this Outcome<TObservation> @this)
        {
            // Getting WasSuccessful forces evaluation
            return @this.WasSuccessful
                ? @this
                : @this;
        }

        public static Outcome<TObservation> Combine<TObservation>(params Outcome<TObservation>[] outcomes)
        {
            Guard.NotNull(outcomes, "outcomes");
            return Combine((IEnumerable<Outcome<TObservation>>)outcomes);
        }

        public static Outcome<TObservation> Combine<TObservation>(this Outcome<TObservation> @this, Outcome<TObservation> other)
        {
            return Combine(new[] {@this, other});
        }

        public static Outcome<TObservation> Combine<TObservation>(this Outcome<TObservation> @this, params Outcome<TObservation>[] outcomes)
        {
            Guard.NotNull(outcomes, "outcomes");
            return Combine(new[]{@this}.Concat(outcomes));
        }

        public static Outcome<TObservation> Combine<TObservation>(this IEnumerable<Outcome<TObservation>> outcomes)
        {
            Guard.NotNull(outcomes, "outcomes");
            return new Outcome<TObservation>(() =>
            {
                var cachedOutcomes = outcomes.ToArray();
                return new Outcome<TObservation>(cachedOutcomes.All(x => x.WasSuccessful), cachedOutcomes.SelectMany(x => x.Observations));
            });
        }
    }
}
