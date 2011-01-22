using System.Collections.Generic;

namespace iSynaptic.Commons.Runtime.Serialization
{
    internal class CloneContext
    {
        private IDictionary<object, object> _CloneMap = null;

        public CloneContext(bool isShallowClone, bool shouldUseExistingObjects)
        {
            IsShallowClone = isShallowClone;
            ShouldUseExistingObjects = shouldUseExistingObjects;
        }

        public bool IsShallowClone { get; private set; }
        public bool ShouldUseExistingObjects { get; private set; }

        public IDictionary<object, object> CloneMap
        {
            get { return _CloneMap ?? (_CloneMap = new Dictionary<object, object>()); }
        }
    }
}