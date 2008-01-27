using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace iSynaptic.Commons
{
    [Serializable]
    public class ContainerException : Exception
    {
        private List<Exception> _Exceptions = null;

        public ContainerException() { }
        public ContainerException(string message) : base(message) { }
        
        public ContainerException(IEnumerable<Exception> exceptions) : this(null, exceptions)
        {
        }

        public ContainerException(string message, IEnumerable<Exception> exceptions) : base(message)
        {
            if(exceptions != null)
                Exceptions.AddRange(exceptions);
        }

        protected ContainerException(SerializationInfo info,StreamingContext context) : base(info, context) { }

        public List<Exception> Exceptions
        {
            get { return _Exceptions ?? (_Exceptions = new List<Exception>()); }
        }
    }
}
