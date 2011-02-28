using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IExodataAttribute<out TExodata>
    {
        bool ProvidesExodataFor<TSubject>(IExodataRequest<TSubject> request);
        TExodata Resolve<TSubject>(IExodataRequest<TSubject> request);
    }
}
