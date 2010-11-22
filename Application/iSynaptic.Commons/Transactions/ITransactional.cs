using iSynaptic.Commons.Runtime.Serialization;

namespace iSynaptic.Commons.Transactions
{
    [CloneReferenceOnly]
    public interface ITransactional<T>
    {
        T Value { get; set; }
    }
}
