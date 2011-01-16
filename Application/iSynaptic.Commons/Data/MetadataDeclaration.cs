using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class MetadataDeclaration<TMetadata> : IMetadataDeclaration<TMetadata>
    {
        public static readonly MetadataDeclaration<TMetadata> TypeDeclaration = new MetadataDeclaration<TMetadata>();

        private Maybe<TMetadata> _Default = Maybe<TMetadata>.NoValue;

        public MetadataDeclaration()
        {
            MetadataType = typeof(TMetadata);
        }

        public MetadataDeclaration(TMetadata @default) : this()
        {
            _Default = new Maybe<TMetadata>(@default);
        }

        protected virtual TMetadata GetDefault()
        {
            if (_Default.HasValue)
                return _Default.Value;

            return default(TMetadata);
        }

        public TMetadata Get()
        {
            return Metadata.Resolve(this, null, null);
        }

        public TMetadata For<T>()
        {
            return Metadata.Resolve(this, typeof(T), null);
        }

        public TMetadata For<T>(T subject)
        {
            return Metadata.Resolve(this, subject, null);
        }

        public TMetadata For<T>(Expression<Func<T, object>> member)
        {
            return Metadata.Resolve(this, typeof (T), member);
        }

        public TMetadata For<T>(T subject, Expression<Func<T, object>> member)
        {
            return Metadata.Resolve(this, subject, member);
        }

        public void ValidateValue(TMetadata value)
        {
            OnValidateValue(value, "value");
        }

        protected virtual void OnValidateValue(TMetadata value, string valueName)
        {
        }

        public static implicit operator TMetadata(MetadataDeclaration<TMetadata> declaration)
        {
            return declaration.Get();
        }

        public TMetadata Default
        {
            get
            {
                TMetadata defaultValue = GetDefault();

                OnValidateValue(defaultValue, "default");

                return defaultValue;
            }
        }

        public Type MetadataType { get; private set; }
    }
}
