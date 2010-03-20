using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using System.Linq;

using iSynaptic.Commons.Extensions;
using iSynaptic.Commons.Extensions.ObjectExtensions;
using iSynaptic.Commons.Testing.NUnit;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class SpecificationTests : NUnitBaseTestFixture
    {
        [Test]
        public void ConversionFromFunc()
        {
            Func<int, bool> func = i => i > 5;

            ISpecification<int> spec = func.ToSpecification();

            Assert.IsTrue(spec.IsSatisfiedBy(6));
            Assert.IsFalse(spec.IsSatisfiedBy(4));

            spec = ((Func<int, bool>)null).ToSpecification();
            Assert.IsNull(spec);
        }

        [Test]
        public void ConversionFromPredicate()
        {
            Predicate<int> predicate = i => i > 5;

            ISpecification<int> spec = predicate.ToSpecification();

            Assert.IsTrue(spec.IsSatisfiedBy(6));
            Assert.IsFalse(spec.IsSatisfiedBy(4));

            spec = ((Predicate<int>)null).ToSpecification();
            Assert.IsNull(spec);
        }

        [Test]
        public void ConversionToFunc()
        {
            var spec = new GreaterThanFiveSpecification();

            Func<int, bool> func = spec.ToFunc();

            Assert.IsTrue(func(6));
            Assert.IsFalse(func(4));

            func = ((ISpecification<int>)null).ToFunc();
            Assert.IsNull(func);
        }

        [Test]
        public void RoundtripFromFuncToSpecificationAndBack()
        {
            Func<int, bool> func = x => x > 5;
            ISpecification<int> spec = func.ToSpecification();

            Func<int, bool> roundTripFunc = spec.ToFunc();

            Assert.IsTrue(object.ReferenceEquals(func, roundTripFunc));
        }

        [Test]
        public void RoundtripFromSpecificationToFuncAndBack()
        {
            var spec = new GreaterThanFiveSpecification();
            Func<int, bool> func = spec.ToFunc();

            ISpecification<int> roundTripSpec = func.ToSpecification();

            Assert.IsTrue(object.ReferenceEquals(spec, roundTripSpec));
        }

        [Test]
        public void ConversionToPredicate()
        {
            var spec = new GreaterThanFiveSpecification();

            Predicate<int> predicate = spec.ToPredicate();

            Assert.IsTrue(predicate(6));
            Assert.IsFalse(predicate(4));

            predicate = ((ISpecification<int>)null).ToPredicate();
            Assert.IsNull(predicate);
        }

        [Test]
        public void And()
        {
            var gtFive = new GreaterThanFiveSpecification();
            var ltSeven = new LessThanSevenSpecification();

            var andSpec = gtFive.And(ltSeven);

            Assert.IsTrue(andSpec.IsSatisfiedBy(6));
            Assert.IsFalse(andSpec.IsSatisfiedBy(4));
            Assert.IsFalse(andSpec.IsSatisfiedBy(8));

            Assert.Throws<ArgumentNullException>(() => Specification.And<int>(null, gtFive));
            Assert.Throws<ArgumentNullException>(() => Specification.And<int>(gtFive, null));
        }

        [Test]
        public void Or()
        {
            var gtFive = new GreaterThanFiveSpecification();
            var ltSeven = new LessThanSevenSpecification();

            var orMethod = gtFive.Or(ltSeven);

            Assert.IsTrue(orMethod.IsSatisfiedBy(6));
            Assert.IsTrue(orMethod.IsSatisfiedBy(4));
            Assert.IsTrue(orMethod.IsSatisfiedBy(8));

            Assert.Throws<ArgumentNullException>(() => Specification.Or<int>(null, gtFive));
            Assert.Throws<ArgumentNullException>(() => Specification.Or<int>(gtFive, null));
        }

        [Test]
        public void XOr()
        {
            var gtFive = new GreaterThanFiveSpecification();
            var ltSeven = new LessThanSevenSpecification();

            var xorMethod = gtFive.XOr(ltSeven);

            Assert.IsFalse(xorMethod.IsSatisfiedBy(6));
            Assert.IsTrue(xorMethod.IsSatisfiedBy(4));
            Assert.IsTrue(xorMethod.IsSatisfiedBy(8));

            Assert.Throws<ArgumentNullException>(() => Specification.XOr<int>(null, gtFive));
            Assert.Throws<ArgumentNullException>(() => Specification.XOr<int>(gtFive, null));
        }

        [Test]
        public void Not()
        {
            var notGtFive = new GreaterThanFiveSpecification().Not();
            var gtFive = notGtFive.Not();

            Assert.IsTrue(notGtFive.IsSatisfiedBy(4));
            Assert.IsFalse(gtFive.IsSatisfiedBy(4));

            Assert.IsFalse(notGtFive.Not().IsSatisfiedBy(4));
            Assert.IsTrue(gtFive.Not().IsSatisfiedBy(4));

            Assert.Throws<ArgumentNullException>(() => Specification.Not<int>(null));
        }

        [Test]
        public void DoubleNotUnwraps()
        {
            var gtFive = new GreaterThanFiveSpecification().Not().Not();

            Assert.IsTrue(gtFive.GetType() == typeof(GreaterThanFiveSpecification));
            Assert.IsFalse(gtFive.GetType().Name == "NotSpecification");
        }

        [Test]
        public void MeetsSpecification()
        {
            int[] values = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var gtFive = new GreaterThanFiveSpecification();
            var ltSeven = new LessThanSevenSpecification();


            Assert.IsTrue(values.MeetsSpecifcation(gtFive).SequenceEqual(new int[] { 6, 7, 8, 9 }));
            Assert.IsTrue(values.MeetsSpecifcation(ltSeven).SequenceEqual(new int[] { 0, 1, 2, 3, 4, 5, 6, }));
            Assert.IsTrue(values.MeetsSpecifcation(gtFive.And(ltSeven)).SequenceEqual(new int[] { 6 }));
        }

        [Test]
        public void FailsSpecification()
        {
            int[] values = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var gtFive = new GreaterThanFiveSpecification();
            var ltSeven = new LessThanSevenSpecification();

            Assert.IsTrue(values.FailsSpecification(gtFive).SequenceEqual(new int[] { 0, 1, 2, 3, 4, 5 }));
            Assert.IsTrue(values.FailsSpecification(ltSeven).SequenceEqual(new int[] { 7, 8, 9 }));
            Assert.IsTrue(values.FailsSpecification(gtFive.And(ltSeven)).SequenceEqual(new int[] { 0, 1, 2, 3, 4, 5, 7, 8, 9 }));
        }

        [Test]
        public void IsSatisfiedBySingleItem()
        {
            var gtFive = new GreaterThanFiveSpecification();

            Assert.IsTrue(gtFive.IsSatisfiedBy(6));
            Assert.IsFalse(gtFive.IsSatisfiedBy(4));
        }

        [Test]
        public void IsSatisfiedByParams()
        {
            var gtFive = new GreaterThanFiveSpecification();

            Assert.IsTrue(gtFive.IsSatisfiedBy(6, 7));
            Assert.IsFalse(gtFive.IsSatisfiedBy(3, 4));
        }

        [Test]
        public void IsSatisfiedByIEnumerable()
        {
            var gtFive = new GreaterThanFiveSpecification();
            var valuesGreaterThanFive = new List<int> { 6, 7, 8, 9 };
            var valuesLessThanFive = new List<int> { 1, 2, 3, 4 };

            Assert.IsTrue(gtFive.IsSatisfiedBy(valuesGreaterThanFive));
            Assert.IsFalse(gtFive.IsSatisfiedBy(valuesLessThanFive));

        }

        [Test]
        public void UnwrapsPredicateSpecification()
        {
            Predicate<int> predicate = val => val > 5;
            ISpecification<int> spec = predicate.ToSpecification();

            Predicate<int> unwrapedPredicate = spec.ToPredicate();

            Assert.IsTrue(object.ReferenceEquals(predicate, unwrapedPredicate));
        }

        [Test]
        public void UnwrapsSpecificationFromPredicate()
        {
            var gtFive = new GreaterThanFiveSpecification();
            Predicate<int> predicate = gtFive.ToPredicate();

            ISpecification<int> unwrapped = predicate.ToSpecification();

            Assert.IsAssignableFrom(typeof(GreaterThanFiveSpecification), unwrapped);
            Assert.IsTrue(object.ReferenceEquals(gtFive, unwrapped));
        }
    }

    public class GreaterThanFiveSpecification : ISpecification<int>
    {
        public bool IsSatisfiedBy(int candidate)
        {
            return candidate > 5;
        }
    }

    public class LessThanSevenSpecification : ISpecification<int>
    {
        public bool IsSatisfiedBy(int candidate)
        {
            return candidate < 7;
        }
    }
}

