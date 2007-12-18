using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

using iSynaptic.Commons.Collections;

namespace iSynaptic.Commons.Xml
{
    public class ProcessingInstruction
    {
        private string _Name = null;
        private ReadOnlyDictionary<string, string> _Attributes = null;

        internal ProcessingInstruction(string name, IEnumerable<KeyValuePair<string, string>> attributes)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentOutOfRangeException("name");

            if (attributes == null)
                throw new ArgumentNullException("attributes");

            _Name = name;
            _Attributes = BuildAttributeDictionary(attributes);
        }

        private static ReadOnlyDictionary<string, string> BuildAttributeDictionary(IEnumerable<KeyValuePair<string, string>> attributes)
        {
            Dictionary<string, string> attributeDictionary = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> attribute in attributes)
                attributeDictionary.Add(attribute.Key, attribute.Value);

            return new ReadOnlyDictionary<string, string>(attributeDictionary);
        }

        public string Name
        {
            get { return _Name; }
        }

        public string this[string key]
        {
            get { return Attributes[key]; }
        }

        public bool ContainsKey(string key)
        {
            return Attributes.ContainsKey(key);
        }

        public ReadOnlyDictionary<string, string> Attributes
        {
            get { return _Attributes; }
        }
    }
}