namespace iSynaptic.Commons.Testing
{
    public interface ITestBehavior
    {
        void BeforeTest(object testFixture);
        void AfterTest(object testFixture);

        bool ShouldOverrideFixtureLevelTestBehavior(ITestBehavior behavior);
    }
}
