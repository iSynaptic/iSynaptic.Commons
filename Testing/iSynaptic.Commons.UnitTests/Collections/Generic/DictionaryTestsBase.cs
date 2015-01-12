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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace iSynaptic.Commons.Collections.Generic
{
    public abstract class DictionaryTestsBase<TKey, TValue>
    {
        protected abstract TKey CreateKey(bool keepReference = false);
        protected abstract TValue CreateValue(bool keepReference = false);

        protected abstract IDictionary<TKey, TValue> CreateDictionary();
        protected abstract IDictionary<TKey, TValue> CreateDictionary(IEqualityComparer<TKey> comparer);

        [Test]
        public void Keys_ReturnsCorrectly()
        {
            var dictionary = CreateDictionary();
            var keys = dictionary.Keys;

            var key = CreateKey(true);
            var value = CreateValue(true);

            dictionary.Add(key, value);

            Assert.IsTrue(keys.SequenceEqual(new []{ key }));
        }

        [Test]
        public void Values_ReturnsCorrectly()
        {
            var dictionary = CreateDictionary();
            var values = dictionary.Values;

            var key = CreateKey(true);
            var value = CreateValue(true);

            dictionary.Add(key, value);

            Assert.IsTrue(values.SequenceEqual(new[] { value }));
        }

        [Test]
        public void Count_ReturnsCorrectly()
        {
            var dictionary = CreateDictionary();
            dictionary.Add(CreateKey(true), CreateValue(true));
            dictionary.Add(CreateKey(true), CreateValue(true));
            dictionary.Add(CreateKey(true), CreateValue(true));

            Assert.AreEqual(3, dictionary.Count);
        }

        [Test]
        public void Indexer_ReturnsCorrectly()
        {
            var dictionary = CreateDictionary();

            var key = CreateKey(true);
            var value = CreateValue(true);

            dictionary.Add(key, value);
            Assert.AreEqual(value, dictionary[key]);
        }


        [Test]
        public void Indexer_AddsValueToDictionary()
        {
            var dictionary = CreateDictionary();

            var key = CreateKey(true);
            var value = CreateValue(true);

            dictionary[key] = value;

            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual(value, dictionary[key]);
        }

        [Test]
        public void Indexer_WithInvalidKey_ThrowsException()
        {
            var dictionary = CreateDictionary();
            var key = CreateKey(true);

            Assert.Throws<KeyNotFoundException>(() => { var result = dictionary[key]; });
        }

        [Test]
        public void ContainsKey_ReturnsCorrectly()
        {
            var dictionary = CreateDictionary();

            var key = CreateKey(true);
            var value = CreateValue(true);

            dictionary.Add(key, value);

            Assert.IsTrue(dictionary.ContainsKey(key));
            Assert.IsFalse(dictionary.ContainsKey(CreateKey()));
        }

        [Test]
        public void Contains_ReturnsCorrectly()
        {
            var dictionary = CreateDictionary();

            var key = CreateKey(true);
            var value = CreateValue(true);

            dictionary.Add(key, value);

            Assert.IsTrue(dictionary.Contains(KeyValuePair.Create(key, value)));
            Assert.IsFalse(dictionary.Contains(KeyValuePair.Create(CreateKey(), CreateValue())));
        }

        [Test]
        public void Remove_ByPair_FunctionsCorrectly()
        {
            var dictionary = CreateDictionary();

            var key = CreateKey(true);
            var value = CreateValue(true);

            dictionary.Add(key, value);
            dictionary.Remove(KeyValuePair.Create(key, value));

            Assert.IsFalse(dictionary.ContainsKey(key));
            Assert.AreEqual(0, dictionary.Count);
        }

        [Test]
        public void Remove_ByKey_FunctionsCorrectly()
        {
            var dictionary = CreateDictionary();

            var key = CreateKey(true);
            var value = CreateValue(true);

            dictionary.Add(key, value);
            dictionary.Remove(key);

            Assert.IsFalse(dictionary.ContainsKey(key));
            Assert.AreEqual(0, dictionary.Count);
        }

        [Test]
        public void IsReadOnly_ReturnsFalse()
        {
            var dictionary = CreateDictionary();
            Assert.IsFalse(dictionary.IsReadOnly);
        }

        [Test]
        public void SetValue_ViaIndexer()
        {
            var dictionary = CreateDictionary();

            var key = CreateKey(true);
            var value = CreateValue(true);

            dictionary[key] = value;

            Assert.IsTrue(ReferenceEquals(dictionary[key], value));
        }

        [Test]
        public void Clear_BehavesCorrectly()
        {
            var dictionary = CreateDictionary();

            var key = CreateKey(true);
            var value = CreateValue(true);

            dictionary.Add(key, value);

            Assert.AreEqual(1, dictionary.Count);

            dictionary.Clear();
            Assert.AreEqual(0, dictionary.Count);
        }

        [Test]
        public void Add_ViaPair()
        {
            var dictionary = CreateDictionary();

            var key = CreateKey(true);
            var value = CreateValue(true);

            dictionary.Add(KeyValuePair.Create(key, value));

            Assert.AreEqual(value, dictionary[key]);
        }

        [Test]
        public void CopyTo_PairArray()
        {
            var dictionary = CreateDictionary();

            var key = CreateKey(true);
            var value = CreateValue(true);

            dictionary.Add(key, value);

            var array = new KeyValuePair<TKey, TValue>[1];
            dictionary.CopyTo(array, 0);

            Assert.AreEqual(key, array[0].Key);
            Assert.AreEqual(value, array[0].Value);
        }

        [Test]
        public void Remove_ViaPair()
        {
            var dictionary = CreateDictionary();

            var key = CreateKey(true);
            var value = CreateValue(true);

            dictionary.Add(key, value);
            Assert.AreEqual(1, dictionary.Count);

            dictionary.Remove(KeyValuePair.Create(key, value));
            Assert.AreEqual(0, dictionary.Count);
        }

        [Test]
        public void NonGenericEnumerator()
        {
            var dictionary = CreateDictionary();

            var key = CreateKey(true);
            var value = CreateValue(true);

            dictionary.Add(key, value);

            var array = dictionary
                .OfType<KeyValuePair<TKey, TValue>>()
                .ToArray();

            Assert.AreEqual(key, array[0].Key);
            Assert.AreEqual(value, array[0].Value);
        }

        [Test]
        public void DictionaryUsesProvidedEqualityComparerToGetHashCodes()
        {
            var key = CreateKey(true);
            var value = CreateValue(true);

            var comparer = MockRepository.GenerateMock<IEqualityComparer<TKey>>();
            comparer.Expect(x => x.GetHashCode(key))
                .Return(42)
                .Repeat.AtLeastOnce();

            var dictionary = CreateDictionary(comparer);

            dictionary.Add(key, value);

            comparer.VerifyAllExpectations();
        }

        [Test]
        public void Enumerator_IsInitialyEmpty()
        {
            var dictionary = CreateDictionary();
            var array = dictionary
                .ToArray();

            Assert.AreEqual(0, array.Length);
        }
    }
}
