using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace iSynaptic.Commons.Transactions
{
    public class Transactional<T> where T : class, ITransactional<T>
    {
        private class EnlistmentManager : IEnlistmentNotification
        {
            private string _Id = null;
            private Transactional<T> _Transactional = null;

            public EnlistmentManager(string id, Transactional<T> transactional)
            {
                if (transactional == null)
                    throw new ArgumentNullException("transactional");

                _Id = id;
                _Transactional = transactional;
            }

            public void Commit(Enlistment enlistment)
            {
                KeyValuePair<Guid, T> value = _Transactional.Values[_Id];

                lock (_Transactional._SyncLock)
                {
                    if (_Transactional._CurrentValue.Key != value.Key)
                        throw new TransactionalConcurrencyException();

                    _Transactional._CurrentValue = _Transactional.CreatePair(value.Value);
                }

                enlistment.Done();
            }

            public void InDoubt(Enlistment enlistment)
            {
                enlistment.Done();
            }

            public void Prepare(PreparingEnlistment preparingEnlistment)
            {
                preparingEnlistment.Prepared();
            }

            public void Rollback(Enlistment enlistment)
            {
                _Transactional.Values.Remove(_Id);
                enlistment.Done();
            }
        }

        private object _SyncLock = new object();
        private KeyValuePair<Guid, T> _CurrentValue = default(KeyValuePair<Guid, T>);
        private Dictionary<string, KeyValuePair<Guid, T>> _Values = null;

        public Transactional() : this(null)
        {
        }

        public Transactional(T current)
        {
            _CurrentValue = CreatePair(current);
        }

        private T GetValue()
        {
            if (Transaction.Current != null)
            {
                Transaction transaction = Transaction.Current;
                string id = transaction.TransactionInformation.LocalIdentifier;

                if (Values.ContainsKey(id) != true)
                {
                    T newValue = null;

                    if (_CurrentValue.Value != null)
                        newValue = _CurrentValue.Value.Duplicate();

                    Values.Add(id, new KeyValuePair<Guid, T>(_CurrentValue.Key, newValue));

                    Enlist(id);
                }

                return Values[id].Value;
            }

            return _CurrentValue.Value;
        }

        private void SetValue(T value)
        {
            if (Transaction.Current != null)
            {
                Transaction transaction = Transaction.Current;
                string id = transaction.TransactionInformation.LocalIdentifier;

                if (Values.ContainsKey(id) != true)
                {
                    if (value != null)
                        Values.Add(id, CreatePair(value));

                    Enlist(id);
                }
                else
                    Values[id] = new KeyValuePair<Guid, T>(Values[id].Key, value);
            }

            if (Transaction.Current == null)
            {
                lock (_SyncLock)
                {
                    _CurrentValue = CreatePair(value);
                }
            }
        }

        private void Enlist(string id)
        {
            Transaction.Current.EnlistVolatile(new EnlistmentManager(id, this), EnlistmentOptions.None);
        }

        private KeyValuePair<Guid, T> CreatePair(T value)
        {
            return new KeyValuePair<Guid, T>(Guid.NewGuid(), value);
        }

        public T Value
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        private Dictionary<string, KeyValuePair<Guid, T>> Values
        {
            get { return _Values ?? (_Values = new Dictionary<string, KeyValuePair<Guid, T>>()); }
        }
    }
}
