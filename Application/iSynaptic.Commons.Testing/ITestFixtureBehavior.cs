namespace iSynaptic.Commons.Testing
{
    public interface ITestFixtureBehavior
    {
        void BeforeTestFixture(object testFixture);
        void AfterTestFixture(object testFixture);
    }
}
