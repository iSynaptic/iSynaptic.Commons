using System;
using System.Reflection.Emit;

namespace iSynaptic.Commons.Data
{
    public class StringExodataAttribute : Attribute, IExodataAttribute<int>, IExodataAttribute<string>, IExodataAttribute<StringExodataDefinition>
    {
        private readonly int _MinLength;
        private readonly int _MaxLength;
        private readonly string _Description;

        public StringExodataAttribute(int minLength, int maxLength, string description)
        {
            _MinLength = minLength;
            _MaxLength = maxLength;
            _Description = description;
        }

        public bool ProvidesExodataFor<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request)
        {
            return
                request.Symbol == StringExodata.All ||
                request.Symbol == StringExodata.MinLength ||
                request.Symbol == StringExodata.MaxLength ||
                request.Symbol == CommonExodata.All ||
                request.Symbol == CommonExodata.Description;
        }

        public StringExodataDefinition Resolve<TContext, TSubject>(IExodataRequest<StringExodataDefinition, TContext, TSubject> request)
        {
            return new StringExodataDefinition(_MinLength, _MaxLength, _Description);
        }

        public string Resolve<TContext, TSubject>(IExodataRequest<string, TContext, TSubject> request)
        {
            return _Description;
        }

        public int Resolve<TContext, TSubject>(IExodataRequest<int, TContext, TSubject> request)
        {
            return request.Symbol == StringExodata.MinLength ? _MinLength : _MaxLength;
        }
    }
}