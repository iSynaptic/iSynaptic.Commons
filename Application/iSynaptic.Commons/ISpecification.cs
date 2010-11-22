namespace iSynaptic.Commons
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T candidate);
    }
}
