using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class MetadataDeclaration<T> : IMetadataDeclaration<T>
    {
        private Maybe<T> _Default = Maybe<T>.NoValue;

        public MetadataDeclaration()
        {
            MetadataType = typeof(T);
        }

        public MetadataDeclaration(T @default) : this()
        {
            _Default = new Maybe<T>(@default);
        }

        protected virtual T GetDefault()
        {
            if (_Default.HasValue)
                return _Default.Value;

            return default(T);
        }

        public void CheckValue(T value)
        {
            OnCheckValue(value, "value");
        }

        protected virtual void OnCheckValue(T value, string valueName)
        {
        }

        public T Default
        {
            get
            {
                T defaultValue = GetDefault();

                try
                {
                    OnCheckValue(defaultValue, "default");
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The default value defined was not valid. See the inner exception for details.", ex);
                }

                return defaultValue;
            }
        }

        public Type MetadataType { get; private set; }
    }
}
