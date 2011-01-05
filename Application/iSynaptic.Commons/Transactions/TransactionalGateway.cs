using System;
using iSynaptic.Commons.Runtime.Serialization;

namespace iSynaptic.Commons.Transactions
{
    public class TransactionalGateway<T> : TransactionalBase<T> where T : class
    {
        public TransactionalGateway(T value) : base(value)
        {
        }

        protected override void SetCurrentValue(T value)
        {
            Guard.NotNull(value, "value");

            var currentValue = GetCurrentValue();
            if(currentValue.Value != null)
            {
                var newValue = value.CloneTo(currentValue.Value);
                base.SetCurrentValue(newValue);
            }
            else
                base.SetCurrentValue(value);
        }

        protected override void SetTransactionValue(T value)
        {
            Guard.NotNull(value, "value");

            var currentValue = GetTransactionValue();
            if (currentValue.HasValue)
            {
                var newValue = value.CloneTo(currentValue.Value.Value);
                base.SetTransactionValue(newValue);
            }
            else
                base.SetTransactionValue(value);
        }
    }
}
