namespace iSynaptic.Commons.Transactions
{
    public class Transactional<T> : TransactionalBase<T>
    {
        public Transactional()
            : this(default(T))
        {
        }

        public Transactional(T current) : base(current)
        {
        }
    }
}
