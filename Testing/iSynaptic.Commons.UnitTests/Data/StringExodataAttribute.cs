using System;
using System.Reflection.Emit;

namespace iSynaptic.Commons.Data
{
    public class StringExodataAttribute : Attribute, IExodataAttribute<int, int>, IExodataAttribute<string, string>, IExodataAttribute<StringExodataDefinition, CommonExodataDefinition>
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
                request.Declaration == StringExodata.All ||
                request.Declaration == StringExodata.MinLength ||
                request.Declaration == StringExodata.MaxLength ||
                request.Declaration == CommonExodata.All ||
                request.Declaration == CommonExodata.Description;
        }

        public StringExodataDefinition Resolve<TContext, TSubject>(IExodataRequest<CommonExodataDefinition, TContext, TSubject> request)
        {
            return new StringExodataDefinition(_MinLength, _MaxLength, _Description);
        }

        public string Resolve<TContext, TSubject>(IExodataRequest<string, TContext, TSubject> request)
        {
            return _Description;
        }

        public int Resolve<TContext, TSubject>(IExodataRequest<int, TContext, TSubject> request)
        {
            return request.Declaration == StringExodata.MinLength ? _MinLength : _MaxLength;
        }
    }
}