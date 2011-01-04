using System;
using System.Collections.Generic;
using System.Threading;
using System.Transactions;
using iSynaptic.Commons.Runtime.Serialization;

namespace iSynaptic.Commons.Transactions
{
    [CloneReferenceOnly]
    public abstract class TransactionalBase<T> : ITransactional<T>
    {
        #region EnlistmentManager
        
        private class EnlistmentManager : IEnlistmentNotification
        {
            private readonly string _Id = null;
            private readonly TransactionalBase<T> _Transactional = null;
               
            public EnlistmentManager(string id, TransactionalBase<T> transactional)
            {
                _Id = id;
                _Transactional = transactional;
            }

            public void Commit(Enlistment enlistment)
            {
                var value = _Transactional.GetTransactionValue(_Id);
                _Transactional.SetCurrentValue(value.Value.Value);
                _Transactional.ClearTransactionValue(_Id);

                _Transactional._Lock.Exit();
                enlistment.Done();
            }

            public void InDoubt(Enlistment enlistment)
            {
                _Transactional.Values.Remove(_Id);
                _Transactional._Lock.Exit();

                enlistment.Done();
            }

            public void Prepare(PreparingEnlistment preparingEnlistment)
            {
                bool lockTaken = false;

                _Transactional._Lock.Enter(ref lockTaken);

                var value = _Transactional.GetTransactionValue(_Id).Value;
                var originalValue = _Transactional.GetCurrentValue();
                
                if (value.Key != originalValue.Key)
                {
                    if(lockTaken)
                        _Transactional._Lock.Exit();

                    preparingEnlistment.ForceRollback(new TransactionalConcurrencyException());
                    return;
                }

                preparingEnlistment.Prepared();
            }

            public void Rollback(Enlistment enlistment)
            {
                _Transactional.ClearTransactionValue(_Id);
                _Transactional._Lock.Exit();

                enlistment.Done();
            }
        }

        #endregion

        private SpinLock _Lock = new SpinLock(enableThreadOwnerTracking: false);

        private KeyValuePair<Guid, T> _CurrentValue = default(KeyValuePair<Guid, T>);
        private Dictionary<string, KeyValuePair<Guid, T>> _Values = null;

        public TransactionalBase(T current)
        {
            if (Cloneable<T>.CanClone() != true)
                throw new InvalidOperationException("Underlying type cannot be cloned.");

            SetCurrentValue(current);
        }

        protected virtual KeyValuePair<Guid, T> GetCurrentValue()
        {
            return _CurrentValue;
        }

        protected virtual KeyValuePair<Guid, T>? GetTransactionValue()
        {
            return GetTransactionValue(GetTransactionIdentifier());
        }

        protected virtual KeyValuePair<Guid, T>? GetTransactionValue(string transactionId)
        {
            if (Values.ContainsKey(transactionId))
                return Values[transactionId];

            return null;
        }

        protected virtual void SetCurrentValue(T value)
        {
            _CurrentValue = CreatePair(value);
        }

        protected virtual void SetTransactionValue(T value)
        {
            string transactionId = GetTransactionIdentifier();

            var pair = GetTransactionValue();
            if (pair.HasValue != true)
            {
                var currentValue = GetCurrentValue();
                Values[transactionId] = CreatePair(currentValue.Key, value);

                Enlist();
            }
            else
                Values[transactionId] = CreatePair(pair.Value.Key, value);
        }

        protected virtual void ClearTransactionValue(string transactionId)
        {
            Values.Remove(transactionId);
        }

        protected virtual KeyValuePair<Guid, T> CopyCurrentValue()
        {
            return _CurrentValue.Clone();
        }

        protected virtual string GetTransactionIdentifier()
        {
            return Transaction.Current.TransactionInformation.LocalIdentifier;
        }

        private T GetValue()
        {
            if (Transaction.Current != null)
            {
                var value = GetTransactionValue();
                if(value.HasValue != true)
                {
                    var newValue = CopyCurrentValue();
                    SetTransactionValue(newValue.Value);
                }
                
                return GetTransactionValue().Value.Value;
            }

            return GetCurrentValue().Value;
        }

        private void SetValue(T value)
        {
            if (Transaction.Current != null)
                SetTransactionValue(value);
            else
                SetCurrentValue(value);
        }

        private void Enlist()
        {
            Transaction.Current.EnlistVolatile(new EnlistmentManager(GetTransactionIdentifier(), this), EnlistmentOptions.None);
        }

        protected static KeyValuePair<Guid, T> CreatePair(T value)
        {
            return CreatePair(Guid.NewGuid(), value);
        }

        protected static KeyValuePair<Guid, T> CreatePair(Guid id, T value)
        {
            return new KeyValuePair<Guid, T>(id, value);
        }

        public T Value
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        protected Dictionary<string, KeyValuePair<Guid, T>> Values
        {
            get
            {
                if(_Values == null)
                {
                    bool lockTaken = false;
                    _Lock.Enter(ref lockTaken);

                    if(_Values == null)
                        _Values = new Dictionary<string, KeyValuePair<Guid, T>>();

                    if(lockTaken)
                        _Lock.Exit();   
                }

                return _Values;
            }
        }
    }
}
