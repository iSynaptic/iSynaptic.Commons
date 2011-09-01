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
    public struct Outcome<T>
    {
        public static readonly Outcome<T> Success = new Outcome<T>(true);
        public static readonly Outcome<T> Failure = new Outcome<T>(false);

        private readonly bool _IsFailure;
        private readonly T[] _Observations;
        private readonly Func<Outcome<T>> _Computation;

        public Outcome(bool isSuccess) : this()
        {
            _IsFailure = !isSuccess;
        }

        public Outcome(bool isSuccess, T observation) : this()
        {
            _IsFailure = !isSuccess;
            _Observations = new[] { observation };
        }

        public Outcome(bool isSuccess, T[] observations)
            : this()
        {
            _IsFailure = !isSuccess;
            _Observations = Guard.NotNull(observations, "observations");
        }

        public Outcome(Func<Outcome<T>> computation)
            : this()
        {
            var cachedComputation = Guard.NotNull(computation, "computation");
            var memoizedResult = default(Outcome<T>);
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

        public IEnumerable<T> Observations
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

        public bool WasSuccessful
        {
            get
            {
                if (_Computation != null)
                    return _Computation().WasSuccessful;

                return !_IsFailure;
            }
        }

        public static implicit operator bool(Outcome<T> outcome)
        {
            return outcome.WasSuccessful;
        }

        public static bool operator true(Outcome<T> outcome)
        {
            return false;
        }

        public static bool operator false(Outcome<T> outcome)
        {
            return false;
        }
    }

    public static class Outcome
    {
        public static Outcome<T> Success<T>(T observation)
        {
            return new Outcome<T>(true, observation);
        }

        public static Outcome<T> Failure<T>(T observation)
        {
            return new Outcome<T>(false, observation);
        }

        public static Outcome<U> Bind<T, U>(this Outcome<T> @this, Func<T, Outcome<U>> selector)
        {
            Guard.NotNull(selector, "selector");

            var self = @this;
            return new Outcome<U>(() =>
            {
                var outcome = self.Observations
                    .Select(selector)
                    .Aggregate(
                    new {Success = true, Observations = Enumerable.Empty<U>()}, (ag, obs) => new 
                    {
                        Success = ag.Success && obs.WasSuccessful,
                        Observations = ag.Observations.Concat(obs.Observations)
                    });

                return new Outcome<U>(outcome.Success, outcome.Observations.ToArray());
            });
        }

        public static Outcome<U> SelectMany<T, U>(this Outcome<T> @this, Func<T, Outcome<U>> selector)
        {
            Guard.NotNull(selector, "selector");
            return Bind(@this, selector);
        }

        public static Outcome<U> Select<T, U>(this Outcome<T> @this, Func<T, U> selector)
        {
            Guard.NotNull(selector, "selector");
            return @this.SelectMany(t => new Outcome<U>(@this.WasSuccessful, selector(t)));
        }

        public static Outcome<T> Where<T>(this Outcome<T> @this, Func<T, bool> predicate)
        {
            Guard.NotNull(predicate, "predicate");
            return @this.SelectMany(t => predicate(t) ? new Outcome<T>(@this.WasSuccessful, t) : new Outcome<T>(@this.WasSuccessful));
        }

        public static Outcome<T> Combine<T>(IEnumerable<Outcome<T>> outcomes)
        {
            Guard.NotNull(outcomes, "outcomes");
            return new Outcome<T>(() =>
            {
                var outcome = outcomes
                    .Aggregate(
                    new { Success = true, Observations = Enumerable.Empty<T>() }, (ag, obs) => new
                    {
                        Success = ag.Success && obs.WasSuccessful,
                        Observations = ag.Observations.Concat(obs.Observations)
                    });

                return new Outcome<T>(outcome.Success, outcome.Observations.ToArray());
            });
        }

        public static Outcome<T> Combine<T>(params Outcome<T>[] outcomes)
        {
            Guard.NotNull(outcomes, "outcomes");
            return Combine((IEnumerable<Outcome<T>>)outcomes);
        }
    }
}
