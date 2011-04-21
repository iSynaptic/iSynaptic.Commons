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

        int IExodataAttribute<int>.Resolve<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request)
        {
            return request.Declaration == StringExodata.MinLength ? _MinLength : _MaxLength;
        }

        string IExodataAttribute<string>.Resolve<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request)
        {
            return _Description;
        }

        StringExodataDefinition IExodataAttribute<StringExodataDefinition>.Resolve<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request)
        {
            return new StringExodataDefinition(_MinLength, _MaxLength, _Description);
        }

        public bool ProvidesExodataFor<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request)
        {
            return
                request.Declaration == StringExodata.All ||
                request.Declaration == StringExodata.MinLength ||
                request.Declaration == StringExodata.MaxLength ||
                request.Declaration == CommonExodata.All ||
                request.Declaration == CommonExodata.Description;
        }
    }
}