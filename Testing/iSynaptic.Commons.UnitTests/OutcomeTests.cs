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
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class OutcomeTests
    {
        [Test]
        public void WasSuccessful_WithDefaultValue_IsTrue()
        {
            var outcome = default(Outcome<string>);
            Assert.IsTrue(outcome.WasSuccessful);
        }

        [Test]
        public void WasSuccessful_WithSuccessField_IsTrue()
        {
            var outcome = Outcome<string>.Success;
            Assert.IsTrue(outcome.WasSuccessful);
        }

        [Test]
        public void WasSuccessful_WithFailureField_IsFalse()
        {
            var outcome = Outcome<string>.Failure;
            Assert.IsFalse(outcome.WasSuccessful);
        }

        [Test]
        public void OutcomeCanBeTreatedAsABool()
        {
            Assert.IsTrue(Outcome<string>.Success);
            Assert.IsFalse(Outcome<string>.Failure);
        }

        [Test]
        public void Bind_ViaSelect_PropigatesFailureObservations()
        {
            Outcome<Observation> outcome = Outcome.Failure(new Observation { Code = 42, Message = "Bad stuff happened.", Type = ObservationType.Error });
            Outcome<string> stringOutcome = outcome.Select(x => x.Message);

            Assert.IsFalse(stringOutcome.WasSuccessful);

            var observations = stringOutcome.Observations.ToList();
            Assert.AreEqual(1, observations.Count);
            Assert.AreEqual("Bad stuff happened.", observations[0]);
        }

        [Test]
        public void Bind_ViaSelect_PropigatesSuccessObservations()
        {
            Outcome<Observation> outcome = Outcome.Success(new Observation { Code = 0, Message = "Greetings from Outcome!", Type = ObservationType.Info });
            Outcome<string> stringOutcome = outcome.Select(x => x.Message);

            Assert.IsTrue(stringOutcome.WasSuccessful);

            var observations = stringOutcome.Observations.ToList();
            Assert.AreEqual(1, observations.Count);
            Assert.AreEqual("Greetings from Outcome!", observations[0]);
        }

        [Test]
        public void Where_FiltersOutUndesireableOutcomes()
        {
            var outcome1 = Outcome.Failure(new Observation { Code = 42, Message = "Really bad stuff happened.", Type = ObservationType.Fatal });
            var outcome2 = Outcome.Failure(new Observation { Code = 7, Message = "Bad stuff happened.", Type = ObservationType.Error });
            var outcome3 = Outcome.Failure(new Observation { Code = 6, Message = "Things could be better", Type = ObservationType.Warning });
            var outcome4 = Outcome.Success(new Observation { Code = 1, Message = "Meh!", Type = ObservationType.Warning });
            var outcome5 = Outcome.Success(new Observation { Code = 1, Message = "Hello, World!", Type = ObservationType.Info });

            Outcome<string> totalOutcome = Outcome.Combine(outcome1, outcome2, outcome3, outcome4, outcome5)
                .Where(x => x.Type >= ObservationType.Error)
                .Select(x => x.Message);

            Assert.IsFalse(totalOutcome.WasSuccessful);
            var failures = totalOutcome.Observations.ToList();

            Assert.AreEqual(2, failures.Count);
        }

        [Test]
        public void Combine_WithAllSuccess_ResultsInSuccess()
        {
            var outcome1 = Outcome<string>.Success;
            var outcome2 = Outcome<string>.Success;
            var outcome3 = Outcome<string>.Success;

            var totalOutcome = Outcome.Combine(outcome1, outcome2, outcome3);

            Assert.IsTrue(totalOutcome.WasSuccessful);
            Assert.IsFalse(totalOutcome.Observations.Any());
        }

        [Test]
        public void Combine_WithAnyFailure_ResultsInFailure()
        {
            var outcome1 = Outcome<string>.Success;
            var outcome2 = Outcome.Failure("Bad stuff happened.");
            var outcome3 = Outcome<string>.Success;

            Outcome<string> totalOutcome = Outcome.Combine(outcome1, outcome2, outcome3);

            Assert.IsFalse(totalOutcome.WasSuccessful);

            var failures = totalOutcome.Observations.ToList();
            Assert.AreEqual(1, failures.Count);
            Assert.AreEqual("Bad stuff happened.", failures[0]);
        }
    }

    public class Observation
    {
        public string Message { get; set; }
        public int Code { get; set; }
        public ObservationType Type { get; set; }
    }

    public enum ObservationType
    {
        Info,
        Warning, 
        Error,
        Fatal
    }
}
