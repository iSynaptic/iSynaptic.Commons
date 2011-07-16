using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DescriptionAttribute : Attribute, IExodataAttribute<string>
    {
        private readonly string _Description;

        public DescriptionAttribute(string description)
        {
            _Description = description;
        }

        public IMaybe<string> TryResolve<TContext, TSubject>(IExodataRequest<string, TContext, TSubject> request)
        {
            return Maybe.If(request.Symbol == CommonExodata.Description, _Description.ToMaybe());
        }
    }
}
