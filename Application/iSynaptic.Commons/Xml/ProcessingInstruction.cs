using System.Collections.Generic;
using iSynaptic.Commons.Collections.Generic;

namespace iSynaptic.Commons.Xml
{
    public class ProcessingInstruction
    {
        internal ProcessingInstruction(string name, IEnumerable<KeyValuePair<string, string>> attributes)
        {
            Name = name;
            Attributes = attributes.ToReadOnlyDictionary();
        }

        public string Name { get; private set; }
        public ReadOnlyDictionary<string, string> Attributes { get; private set; }

        public string this[string key]
        {
            get { return Attributes[key]; }
        }

        public bool ContainsKey(string key)
        {
            return Attributes.ContainsKey(key);
        }
    }
}
