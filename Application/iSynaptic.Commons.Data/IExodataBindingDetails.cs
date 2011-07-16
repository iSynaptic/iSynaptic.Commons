using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IExodataBindingDetails
    {
        string Name { get; }

        IExodataBindingSource Source { get; }

        bool BoundToContextInstance { get; }
        bool BoundToSubjectInstance { get; }

        Type ContextType { get; }
        Type SubjectType { get; }
    }
}
