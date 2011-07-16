using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class AmbiguousExodataBindingsException : Exception
    {
        private readonly IExodataBinding[] _Bindings = null;
        public AmbiguousExodataBindingsException(string message, IEnumerable<IExodataBinding> bindings) : base(message)
        {
            _Bindings = bindings.ToArray();
        }

        public IEnumerable<IExodataBinding> Bindings
        {
            get { return _Bindings; }
        }
    }
}
