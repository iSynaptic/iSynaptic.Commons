namespace iSynaptic.Commons.AOP
{
    public interface IUnitOfWork<in T> : IEnlistmentScope<T>
    {
        void Complete();
    }
}