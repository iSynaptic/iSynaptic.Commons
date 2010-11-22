using NUnit.Framework;

namespace iSynaptic.Commons.Testing.NUnit
{
    public abstract class NUnitBaseTestFixture : BaseTestFixture
    {
        [TestFixtureSetUp]
        protected override void BeforeTestFixture() { base.BeforeTestFixture(); }

        [TestFixtureTearDown]
        protected override void AfterTestFixture() { base.AfterTestFixture(); }

        [SetUp]
        protected override void BeforeTest() { base.BeforeTest(); }

        [TearDown]
        protected override void AfterTest() { base.AfterTest(); }
    }
}
