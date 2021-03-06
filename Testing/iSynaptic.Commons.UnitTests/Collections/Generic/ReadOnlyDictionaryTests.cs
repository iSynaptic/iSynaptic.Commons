// The MIT License
// 
// Copyright (c) 2012-2015 Jordan E. Terrell
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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

            Assert.Throws<NotSupportedException>(() => dict.Add(KeyValuePair.Create("Key", "Value")));
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

            Assert.IsTrue(dict.Contains(KeyValuePair.Create("Key", "Value")));
            Assert.IsTrue(dict.Contains(KeyValuePair.Create("OtherKey", "OtherValue")));

            Assert.IsFalse(dict.Contains(KeyValuePair.Create("NonExistent", "NonExistentValue")));
        }

        [Test]
        public void CopyTo()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Key", "Value");
            dict.Add("OtherKey", "OtherValue");

            dict = dict.ToReadOnlyDictionary();

            var values = new KeyValuePair<string, string>[3];

            var expectedValues = new[]
            {
                KeyValuePair.Create("Key", "Value"),
                KeyValuePair.Create("OtherKey", "OtherValue"),
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

            Assert.Throws<NotSupportedException>(() => dict.Remove(KeyValuePair.Create("Key", "Value")));
        }

        [Test]
        public void GenericGetEnumerator()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Key", "Value");
            dict.Add("OtherKey", "OtherValue");

            dict = dict.ToReadOnlyDictionary();

            var values = new[]
            {
                KeyValuePair.Create("Key", "Value"),
                KeyValuePair.Create("OtherKey", "OtherValue")
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

            var values = new[]
            {
                KeyValuePair.Create("Key", "Value"),
                KeyValuePair.Create("OtherKey", "OtherValue")
            };

            IEnumerable enumerable = dict;
            Assert.IsTrue(enumerable.OfType<KeyValuePair<string, string>>().SequenceEqual(values));
        }
    }
}
