// The MIT License
// 
// Copyright (c) 2011 Jordan E. Terrell
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
using NUnit.Framework;

namespace iSynaptic.Commons.Collections.Generic
{
    public abstract class WeakDictionaryTestsBase<TKey, TValue> : DictionaryTestsBase<TKey, TValue>
    {
        protected override IDictionary<TKey, TValue> CreateDictionary()
        {
            return CreateWeakDictionary();
        }

        protected override IDictionary<TKey, TValue> CreateDictionary(IEqualityComparer<TKey> comparer)
        {
            return CreateWeakDictionary(comparer);
        }

        protected abstract IWeakDictionary<TKey, TValue> CreateWeakDictionary(IEqualityComparer<TKey> comparer = null);
    
        [Test]
        public void PurgeGarbage_RemovesEntriesForGarbageCollectedKeys()
        {
            var dictionary = CreateWeakDictionary();

            var key = CreateKey(true);
            var value = CreateValue(true);

            dictionary.Add(key, value);
            dictionary.Add(CreateKey(), CreateValue());

            GC.Collect();
            dictionary.PurgeGarbage(null);

            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsKey(key));
            Assert.AreEqual(value, dictionary[key]);
        }
        
        [Test]
        public void PurgeGarbage_OnGarbagePurgeCalled()
        {
            bool onGarbagePurgeExecuted = false;
            Action<Maybe<TKey>, Maybe<TValue>> withPurgedPair = (k, v) => onGarbagePurgeExecuted = true;

            var dictionary = CreateWeakDictionary();
            dictionary.Add(CreateKey(), CreateValue());

            GC.Collect();
            dictionary.PurgeGarbage(withPurgedPair);

            Assert.IsTrue(onGarbagePurgeExecuted);
        }
    }
}