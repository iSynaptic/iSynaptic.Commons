using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IExodataResolutionRoot<TExodata>
    {
        IExodataResolutionSubject<TExodata> Given<TContext>();
        IExodataResolutionSubject<TExodata> Given<TContext>(TContext context);
    }

    public interface IExodataResolutionSubject<TExodata>
    {
        TExodata Get();
        TExodata For<TSubject>();
        TExodata For<TSubject>(TSubject subject);
        TExodata For<TSubject>(Expression<Func<TSubject, object>> member);
        TExodata For<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member);
    }
}
