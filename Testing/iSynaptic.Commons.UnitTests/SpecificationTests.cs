using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Framework;

using iSynaptic.Commons.Extensions;
using iSynaptic.Commons.Extensions.ObjectExtensions;

namespace iSynaptic.Commons.UnitTests
{
    [TestFixture]
    public class SpecificationTests : BaseTestFixture
    {
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

            AssertThrows<ArgumentNullException>(instanceFunc.Curry(null).ToAction());
            AssertThrows<ArgumentNullException>(staticFunc.Curry(null, gtFive).ToAction());
            AssertThrows<ArgumentNullException>(staticFunc.Curry(gtFive, null).ToAction());
            AssertThrows<ArgumentNullException>(operatorFunc.Curry(null, gtFive).ToAction());
            AssertThrows<ArgumentNullException>(operatorFunc.Curry(gtFive, null).ToAction());
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

            AssertThrows<ArgumentNullException>(instanceFunc.Curry(null).ToAction());
            AssertThrows<ArgumentNullException>(staticFunc.Curry(null, gtFive).ToAction());
            AssertThrows<ArgumentNullException>(staticFunc.Curry(gtFive, null).ToAction());
            AssertThrows<ArgumentNullException>(operatorFunc.Curry(null, gtFive).ToAction());
            AssertThrows<ArgumentNullException>(operatorFunc.Curry(gtFive, null).ToAction());
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

            AssertThrows<ArgumentNullException>(instanceFunc.Curry(null).ToAction());
            AssertThrows<ArgumentNullException>(staticFunc.Curry(null, gtFive).ToAction());
            AssertThrows<ArgumentNullException>(staticFunc.Curry(gtFive, null).ToAction());
            AssertThrows<ArgumentNullException>(operatorFunc.Curry(null, gtFive).ToAction());
            AssertThrows<ArgumentNullException>(operatorFunc.Curry(gtFive, null).ToAction());
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

            AssertThrows<ArgumentNullException>(notFunc.Curry(null).ToAction());
        }

        [Test]
        public void DoubleNotUnwraps()
        {
            var gtFive = new GreaterThanFiveSpecification().Not().Not();

            Assert.IsTrue(gtFive.GetType() == typeof(GreaterThanFiveSpecification));
            Assert.IsFalse(gtFive.GetType().Name == "NotSpecification");
        }

        [Test]
        [Ignore]
        public void MeetsSpecification()
        {
            int[] values = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        }

        [Test]
        [Ignore]
        public void FailsSpecification()
        {
        }

        [Test]
        [Ignore]
        public void IsSatisfiedBySingleItem()
        {
        }

        [Test]
        [Ignore]
        public void IsSatisfiedByParams()
        {
        }

        [Test]
        [Ignore]
        public void IsSatisfiedByIEnumerable()
        {
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

