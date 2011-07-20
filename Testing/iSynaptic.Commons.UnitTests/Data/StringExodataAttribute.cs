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

        public Maybe<StringExodataDefinition> TryResolve<TContext, TSubject>(IExodataRequest<StringExodataDefinition, TContext, TSubject> request)
        {
            return Maybe.If(request.Symbol == StringExodata.All || request.Symbol == CommonExodata.All, 
                 new StringExodataDefinition(_MinLength, _MaxLength, _Description).ToMaybe());
        }

        public Maybe<string> TryResolve<TContext, TSubject>(IExodataRequest<string, TContext, TSubject> request)
        {
            return Maybe.If(request.Symbol == CommonExodata.Description, _Description.ToMaybe());
        }

        public Maybe<int> TryResolve<TContext, TSubject>(IExodataRequest<int, TContext, TSubject> request)
        {
            return Maybe.If(request.Symbol == StringExodata.MinLength, _MinLength.ToMaybe())
                .Or(Maybe.If(request.Symbol == StringExodata.MaxLength, _MaxLength.ToMaybe()));
        }
    }
}