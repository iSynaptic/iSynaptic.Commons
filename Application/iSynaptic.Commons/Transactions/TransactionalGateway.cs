using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iSynaptic.Commons.Runtime.Serialization;
using iSynaptic.Commons.Threading;

namespace iSynaptic.Commons.Transactions
{
    public class TransactionalGateway<T> : TransactionalBase<T> where T : class
    {
        public TransactionalGateway(T underlying) : base(underlying)
        {
            if (underlying == null)
                throw new ArgumentNullException("underlying");
        }

        protected override void SetCurrentValue(T value)
        {
            if(value == null)
                throw new ArgumentNullException("value");

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
            if (value == null)
                throw new ArgumentNullException("value");

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
