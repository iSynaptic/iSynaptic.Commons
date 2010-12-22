using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using NUnit.Framework;
using System.Collections;

namespace iSynaptic.Commons.Collections.Generic
{
    [TestFixture]
    public class ReadOnlyDictionaryTests
    {
        [Test]
        public void NullInnerDictionary()
        {
            Assert.Throws<ArgumentNullException>(() => new ReadOnlyDictionary<string, string>(null));
        }

        [Test]
        public void Add()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict = dict.ToReadOnlyDictionary();

            Assert.Throws<NotSupportedException>(() => dict.Add("", ""));
        }

        [Test]
        public void ContainsKey()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Key", "Value");
            
            dict = dict.ToReadOnlyDictionary();

            Assert.IsTrue(dict.ContainsKey("Key"));
            Assert.IsFalse(dict.ContainsKey("OtherKey"));
        }

        [Test]
        public void Keys()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Key", "Value");
            dict.Add("OtherKey", "OtherValue");
            
            dict = dict.ToReadOnlyDictionary();

            Assert.IsTrue(dict.Keys.SequenceEqual(new string[] { "Key", "OtherKey" }));
        }

        [Test]
        public void Remove()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Key", "Value");

            dict = dict.ToReadOnlyDictionary();

            Assert.Throws<NotSupportedException>(() => dict.Remove("Key"));
        }

        [Test]
        public void TryGetValue()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Key", "Value");

            dict = dict.ToReadOnlyDictionary();

            string value = null;
            
            Assert.IsTrue(dict.TryGetValue("Key", out value));
            Assert.AreEqual("Value", value);

            Assert.IsFalse(dict.TryGetValue("OtherKey", out value));
            Assert.IsNull(value);
        }

        [Test]
        public void Values()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Key", "Value");
            dict.Add("OtherKey", "OtherValue");

            dict = dict.ToReadOnlyDictionary();

            Assert.IsTrue(dict.Values.SequenceEqual(new string[] { "Value", "OtherValue" }));
        }

        [Test]
        public void GetViaKeyIndexer()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Key", "Value");
            dict.Add("OtherKey", "OtherValue");

            dict = dict.ToReadOnlyDictionary();

            Assert.AreEqual("Value", dict["Key"]);
            Assert.AreEqual("OtherValue", dict["OtherKey"]);

            Assert.Throws<KeyNotFoundException>(() => { string val = dict["NonExistentValue"]; });
        }

        [Test]
        public void SetViaKeyIndexer()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict = dict.ToReadOnlyDictionary();

            Assert.Throws<NotSupportedException>(() => dict["Key"] = "Value");
        }

        [Test]
        public void AddViaCollectionInterface()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict = dict.ToReadOnlyDictionary();

            Assert.Throws<NotSupportedException>(() => dict.Add(new KeyValuePair<string,string>("Key", "Value")));
        }

        [Test]
        public void Clear()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict = dict.ToReadOnlyDictionary();

            Assert.Throws<NotSupportedException>(() => dict.Clear());
        }

        [Test]
        public void Contains()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Key", "Value");
            dict.Add("OtherKey", "OtherValue");

            dict = dict.ToReadOnlyDictionary();

            Assert.IsTrue(dict.Contains(new KeyValuePair<string,string>("Key", "Value")));
            Assert.IsTrue(dict.Contains(new KeyValuePair<string,string>("OtherKey", "OtherValue")));

            Assert.IsFalse(dict.Contains(new KeyValuePair<string, string>("NonExistent", "NonExistentValue")));
        }

        [Test]
        public void CopyTo()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Key", "Value");
            dict.Add("OtherKey", "OtherValue");

            dict = dict.ToReadOnlyDictionary();

            var values = new KeyValuePair<string, string>[3];

            var expectedValues = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("Key", "Value"),
                new KeyValuePair<string, string>("OtherKey", "OtherValue"),
                default(KeyValuePair<string, string>)
            };

            dict.CopyTo(values, 0);
            Assert.IsTrue(values.SequenceEqual(expectedValues));

            dict.CopyTo(values, 1);

            expectedValues[2] = expectedValues[1];
            expectedValues[1] = expectedValues[0];

            Assert.IsTrue(values.SequenceEqual(expectedValues));
        }

        [Test]
        public void Count()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Key", "Value");
            dict.Add("OtherKey", "OtherValue");

            dict = dict.ToReadOnlyDictionary();

            Assert.AreEqual(2, dict.Count);
        }

        [Test]
        public void IsReadOnly()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict = dict.ToReadOnlyDictionary();

            Assert.IsTrue(dict.IsReadOnly);
        }

        [Test]
        public void RemoveViaCollectionInterface()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Key", "Value");

            dict = dict.ToReadOnlyDictionary();

            Assert.Throws<NotSupportedException>(() => dict.Remove(new KeyValuePair<string, string>("Key", "Value")));
        }

        [Test]
        public void GenericGetEnumerator()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Key", "Value");
            dict.Add("OtherKey", "OtherValue");

            dict = dict.ToReadOnlyDictionary();

            var values = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("Key", "Value"),
                new KeyValuePair<string, string>("OtherKey", "OtherValue")
            };

            Assert.IsTrue(dict.SequenceEqual(values));
        }

        [Test]
        public void GetEnumerator()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Key", "Value");
            dict.Add("OtherKey", "OtherValue");

            dict = dict.ToReadOnlyDictionary();

            var values = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("Key", "Value"),
                new KeyValuePair<string, string>("OtherKey", "OtherValue")
            };

            IEnumerable enumerable = dict;
            Assert.IsTrue(enumerable.OfType<KeyValuePair<string, string>>().SequenceEqual(values));
        }
    }
}
