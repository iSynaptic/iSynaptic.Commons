using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using System.Linq;

using iSynaptic.Commons.Extensions;
using iSynaptic.Commons.Extensions.ObjectExtensions;

namespace iSynaptic.Commons.UnitTests
{
    [TestFixture]
    public class SpecificationTests : BaseTestFixture
    {
        [Test]
        public void ImplicitConversionFromFunc()
        {
            Func<int, bool> func = i => i > 5;

            Specification<int> spec = func;

            Assert.IsTrue(spec.IsSatisfiedBy(6));
            Assert.IsFalse(spec.IsSatisfiedBy(4));

            spec = (Func<int, bool>)null;
            Assert.IsNull(spec);
        }

        [Test]
        public void ImplicitConversionFromPredicate()
        {
            Predicate<int> predicate = i => i > 5;

            Specification<int> spec = predicate;

            Assert.IsTrue(spec.IsSatisfiedBy(6));
            Assert.IsFalse(spec.IsSatisfiedBy(4));

            spec = (Predicate<int>)null;
            Assert.IsNull(spec);
        }

        [Test]
        public void ImplicitConversionToFunc()
        {
            var spec = new GreaterThanFiveSpecification();

            Func<int, bool> func = spec;

            Assert.IsTrue(func(6));
            Assert.IsFalse(func(4));

            func = (Specification<int>)null;
            Assert.IsNull(func);
        }

        [Test]
        public void ImplicitRoundtripFromFuncToSpecificationAndBack()
        {
            Func<int, bool> func = x => x > 5;
            Specification<int> spec = func;

            Func<int, bool> roundTripFunc = spec;

            Assert.IsTrue(object.ReferenceEquals(func, roundTripFunc));
        }

        [Test]
        public void ImplicitRoundtripFromSpecificationToFuncAndBack()
        {
            var spec = new GreaterThanFiveSpecification();
            Func<int, bool> func = spec;

            Specification<int> roundTripSpec = func;

            Assert.IsTrue(object.ReferenceEquals(spec, roundTripSpec));
        }

        [Test]
        public void ImplicitConversionToPredicate()
        {
            var spec = new GreaterThanFiveSpecification();

            Predicate<int> predicate = spec;

            Assert.IsTrue(predicate(6));
            Assert.IsFalse(predicate(4));

            predicate = (Specification<int>)null;
            Assert.IsNull(predicate);
        }

        [Test]
        public void And()
        {
            var gtFive = new GreaterThanFiveSpecification();
            var ltSeven = new LessThanSevenSpecification();

            var andSpec = gtFive.And(ltSeven);
            var staticAndSpec = Specification<int>.And(gtFive, ltSeven);
            var operatorSpec = gtFive && ltSeven;

            Assert.IsTrue(andSpec.IsSatisfiedBy(6));
            Assert.IsTrue(staticAndSpec.IsSatisfiedBy(6));
            Assert.IsTrue(operatorSpec.IsSatisfiedBy(6));

            Assert.IsFalse(andSpec.IsSatisfiedBy(4));
            Assert.IsFalse(staticAndSpec.IsSatisfiedBy(4));
            Assert.IsFalse(operatorSpec.IsSatisfiedBy(4));

            Assert.IsFalse(andSpec.IsSatisfiedBy(8));
            Assert.IsFalse(staticAndSpec.IsSatisfiedBy(8));
            Assert.IsFalse(operatorSpec.IsSatisfiedBy(8));

            Func<Specification<int>, Specification<int>> instanceFunc = gtFive.And;
            Func<Specification<int>, Specification<int>, Specification<int>> staticFunc = Specification<int>.And;
            var operatorFunc = gtFive.GetFunc<Specification<int>, Specification<int>, Specification<int>>("op_BitwiseAnd");

            Assert.Throws<ArgumentNullException>(instanceFunc.Curry(null).AsTestDelegate());
            Assert.Throws<ArgumentNullException>(staticFunc.Curry(null, gtFive).AsTestDelegate());
            Assert.Throws<ArgumentNullException>(staticFunc.Curry(gtFive, null).AsTestDelegate());
            Assert.Throws<ArgumentNullException>(operatorFunc.Curry(null, gtFive).AsTestDelegate());
            Assert.Throws<ArgumentNullException>(operatorFunc.Curry(gtFive, null).AsTestDelegate());
        }

        [Test]
        public void Or()
        {
            var gtFive = new GreaterThanFiveSpecification();
            var ltSeven = new LessThanSevenSpecification();

            var orMethod = gtFive.Or(ltSeven);
            var staticOrMethod = Specification<int>.Or(gtFive, ltSeven);
            var operatorOr = gtFive || ltSeven;

            Assert.IsTrue(orMethod.IsSatisfiedBy(6));
            Assert.IsTrue(staticOrMethod.IsSatisfiedBy(6));
            Assert.IsTrue(operatorOr.IsSatisfiedBy(6));

            Assert.IsTrue(orMethod.IsSatisfiedBy(4));
            Assert.IsTrue(staticOrMethod.IsSatisfiedBy(4));
            Assert.IsTrue(operatorOr.IsSatisfiedBy(4));

            Assert.IsTrue(orMethod.IsSatisfiedBy(8));
            Assert.IsTrue(staticOrMethod.IsSatisfiedBy(8));
            Assert.IsTrue(operatorOr.IsSatisfiedBy(8));

            Func<Specification<int>, Specification<int>> instanceFunc = gtFive.Or;
            Func<Specification<int>, Specification<int>, Specification<int>> staticFunc = Specification<int>.Or;
            var operatorFunc = gtFive.GetFunc<Specification<int>, Specification<int>, Specification<int>>("op_BitwiseOr");

            Assert.Throws<ArgumentNullException>(instanceFunc.Curry(null).AsTestDelegate());
            Assert.Throws<ArgumentNullException>(staticFunc.Curry(null, gtFive).AsTestDelegate());
            Assert.Throws<ArgumentNullException>(staticFunc.Curry(gtFive, null).AsTestDelegate());
            Assert.Throws<ArgumentNullException>(operatorFunc.Curry(null, gtFive).AsTestDelegate());
            Assert.Throws<ArgumentNullException>(operatorFunc.Curry(gtFive, null).AsTestDelegate());
        }

        [Test]
        public void XOr()
        {
            var gtFive = new GreaterThanFiveSpecification();
            var ltSeven = new LessThanSevenSpecification();

            var xorMethod = gtFive.XOr(ltSeven);
            var staticXOrMethod = Specification<int>.XOr(gtFive, ltSeven);
            var operatorXOr = gtFive ^ ltSeven;

            Assert.IsFalse(xorMethod.IsSatisfiedBy(6));
            Assert.IsFalse(staticXOrMethod.IsSatisfiedBy(6));
            Assert.IsFalse(operatorXOr.IsSatisfiedBy(6));

            Assert.IsTrue(xorMethod.IsSatisfiedBy(4));
            Assert.IsTrue(staticXOrMethod.IsSatisfiedBy(4));
            Assert.IsTrue(operatorXOr.IsSatisfiedBy(4));

            Assert.IsTrue(xorMethod.IsSatisfiedBy(8));
            Assert.IsTrue(staticXOrMethod.IsSatisfiedBy(8));
            Assert.IsTrue(operatorXOr.IsSatisfiedBy(8));

            Func<Specification<int>, Specification<int>> instanceFunc = gtFive.XOr;
            Func<Specification<int>, Specification<int>, Specification<int>> staticFunc = Specification<int>.XOr;
            var operatorFunc = gtFive.GetFunc<Specification<int>, Specification<int>, Specification<int>>("op_ExclusiveOr");

            Assert.Throws<ArgumentNullException>(instanceFunc.Curry(null).AsTestDelegate());
            Assert.Throws<ArgumentNullException>(staticFunc.Curry(null, gtFive).AsTestDelegate());
            Assert.Throws<ArgumentNullException>(staticFunc.Curry(gtFive, null).AsTestDelegate());
            Assert.Throws<ArgumentNullException>(operatorFunc.Curry(null, gtFive).AsTestDelegate());
            Assert.Throws<ArgumentNullException>(operatorFunc.Curry(gtFive, null).AsTestDelegate());
        }

        [Test]
        public void Not()
        {
            var notGtFive = new GreaterThanFiveSpecification().Not();
            var gtFive = notGtFive.Not();

            Assert.IsTrue(notGtFive.IsSatisfiedBy(4));
            Assert.IsFalse(gtFive.IsSatisfiedBy(4));

            Assert.IsFalse((!notGtFive).IsSatisfiedBy(4));
            Assert.IsTrue((!gtFive).IsSatisfiedBy(4));

            Func<Specification<int>, Specification<int>> notFunc = GreaterThanFiveSpecification.Not;

            Assert.Throws<ArgumentNullException>(notFunc.Curry(null).AsTestDelegate());
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

            Assert.IsTrue(gtFive.MeetsSpecifcation(values).SequenceEqual(new int[] { 6, 7, 8, 9 }));
            Assert.IsTrue(ltSeven.MeetsSpecifcation(values).SequenceEqual(new int[] { 0, 1, 2, 3, 4, 5, 6, }));
            Assert.IsTrue((gtFive && ltSeven).MeetsSpecifcation(values).SequenceEqual(new int[] { 6 }));
        }

        [Test]
        public void FailsSpecification()
        {
            int[] values = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var gtFive = new GreaterThanFiveSpecification();
            var ltSeven = new LessThanSevenSpecification();

            Assert.IsTrue(gtFive.FailsSpecification(values).SequenceEqual(new int[] { 0, 1, 2, 3, 4, 5 }));
            Assert.IsTrue(ltSeven.FailsSpecification(values).SequenceEqual(new int[] { 7, 8, 9 }));
            Assert.IsTrue((gtFive && ltSeven).FailsSpecification(values).SequenceEqual(new int[] { 0, 1, 2, 3, 4, 5, 7, 8, 9 }));
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
            Specification<int> spec = predicate;

            Predicate<int> unwrapedPredicate = spec;

            Assert.IsTrue(object.ReferenceEquals(predicate, unwrapedPredicate));
        }

        [Test]
        public void UnwrapsSpecificationFromPredicate()
        {
            var gtFive = new GreaterThanFiveSpecification();
            Predicate<int> predicate = gtFive;

            Specification<int> unwrapped = predicate;

            Assert.IsAssignableFrom(typeof(GreaterThanFiveSpecification), unwrapped);
            Assert.IsTrue(object.ReferenceEquals(gtFive, unwrapped));
        }
    }

    public class GreaterThanFiveSpecification : Specification<int>
    {
        public override bool IsSatisfiedBy(int subject)
        {
            return subject > 5;
        }
    }

    public class LessThanSevenSpecification : Specification<int>
    {
        public override bool IsSatisfiedBy(int subject)
        {
            return subject < 7;
        }
    }
}

