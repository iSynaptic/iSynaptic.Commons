using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

using iSynaptic.Commons.Collections.Generic;

namespace iSynaptic.Commons.Xml
{
    public class ProcessingInstruction
    {
        internal ProcessingInstruction(string name, IEnumerable<KeyValuePair<string, string>> attributes)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentOutOfRangeException("name");

            if (attributes == null)
                throw new ArgumentNullException("attributes");

            Name = name;
            Attributes = BuildAttributeDictionary(attributes);
        }

        private static ReadOnlyDictionary<string, string> BuildAttributeDictionary(IEnumerable<KeyValuePair<string, string>> attributes)
        {
            Dictionary<string, string> attributeDictionary = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> attribute in attributes)
                attributeDictionary.Add(attribute.Key, attribute.Value);

            return new ReadOnlyDictionary<string, string>(attributeDictionary);
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
