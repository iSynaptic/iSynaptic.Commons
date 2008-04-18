using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace iSynaptic.Commons
{
    public class CompoundException : Exception
    {
        private List<Exception> _Exceptions = null;

        public CompoundException(string message) : this(message, null)
        {
        }

        public CompoundException(string message, IEnumerable<Exception> exceptions) : base(message)
        {
            if(exceptions != null)
                Exceptions.AddRange(exceptions);
        }

        public List<Exception> Exceptions
        {
            get { return _Exceptions ?? (_Exceptions = new List<Exception>()); }
        }
    }
}
