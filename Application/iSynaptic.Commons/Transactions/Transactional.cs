using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.Threading;

using iSynaptic.Commons.Runtime.Serialization;

namespace iSynaptic.Commons.Transactions
{
    public class Transactional<T>
    {
        #region EnlistmentManager
        
        private class EnlistmentManager : IEnlistmentNotification
        {
            private string _Id = null;
            private Transactional<T> _Transactional = null;

            public EnlistmentManager(string id, Transactional<T> transactional)
            {
                _Id = id;
                _Transactional = transactional;
            }

            public void Commit(Enlistment enlistment)
            {
                KeyValuePair<Guid, T> value = _Transactional.Values[_Id];

                _Transactional._CurrentValue = _Transactional.CreatePair(value.Value);
                _Transactional.Values.Remove(_Id);

                Monitor.Exit(_Transactional._SyncLock);
                enlistment.Done();
            }

            public void InDoubt(Enlistment enlistment)
            {
                enlistment.Done();
            }

            public void Prepare(PreparingEnlistment preparingEnlistment)
            {
                Monitor.Enter(_Transactional._SyncLock);

                KeyValuePair<Guid, T> value = _Transactional.Values[_Id];
                if (_Transactional._CurrentValue.Key != value.Key)
                {
                    Monitor.Exit(_Transactional._SyncLock);
                    throw new TransactionalConcurrencyException();
                }

                preparingEnlistment.Prepared();
            }

            public void Rollback(Enlistment enlistment)
            {
                _Transactional.Values.Remove(_Id);
                enlistment.Done();
            }
        }

        #endregion

        private object _SyncLock = new object();
        private KeyValuePair<Guid, T> _CurrentValue = default(KeyValuePair<Guid, T>);
        private Dictionary<string, KeyValuePair<Guid, T>> _Values = null;

        static Transactional()
        {
            if (Cloneable<T>.CanClone() != true)
                throw new InvalidOperationException("Underlying type cannot be cloned.");
        }

        public Transactional() : this(default(T))
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
                    T newValue = default(T);

                    if (_CurrentValue.Value != null)
                        newValue = Cloneable<T>.Clone(_CurrentValue.Value);

                    Values.Add(id, CreatePair(_CurrentValue.Key, newValue));

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
                        Values.Add(id, CreatePair(_CurrentValue.Key, value));

                    Enlist(id);
                }
                else
                    Values[id] = CreatePair(Values[id].Key, value);
            }

            if (Transaction.Current == null)
            {
                _CurrentValue = CreatePair(value);
            }
        }

        private void Enlist(string id)
        {
            Transaction.Current.EnlistVolatile(new EnlistmentManager(id, this), EnlistmentOptions.None);
        }

        private KeyValuePair<Guid, T> CreatePair(T value)
        {
            return CreatePair(Guid.NewGuid(), value);
        }

        private KeyValuePair<Guid, T> CreatePair(Guid id, T value)
        {
            return new KeyValuePair<Guid, T>(id, value);
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
