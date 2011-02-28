using System;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface IFluentExodataBindingTo<TExodata, TSubject> : IFluentInterface
    {
        void To(TExodata value);
        void To(Func<IExodataRequest<TSubject>, TExodata> valueFactory);
    }
}