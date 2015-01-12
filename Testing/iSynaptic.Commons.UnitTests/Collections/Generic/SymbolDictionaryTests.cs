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
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons.Collections.Generic
{
    [TestFixture]
    public class SymbolDictionaryTests
    {
        [Test]
        public void ReadOnlyDictionaries_NotAllowed()
        {
            Assert.Throws<ArgumentException>(
                () => new SymbolDictionary(new Dictionary<ISymbol, object>().ToReadOnlyDictionary()));
        }

        [Test]
        public void GetSet_RoundTrip_WithTypedSymbol()
        {
            var symbol = new Symbol<int>();
            var dictionary = new SymbolDictionary();

            dictionary.Set(symbol, 42);

            Assert.AreEqual(42, dictionary.Get(symbol));
        }

        [Test]
        public void GetSet_RoundTrip_WithUntypedSymbol()
        {
            var symbol = new Symbol();
            var dictionary = new SymbolDictionary();

            dictionary.Set(symbol, 42);

            Assert.AreEqual(42, dictionary.Get<int>(symbol));
        }

        [Test]
        public void Set_WithNoValue_RemovesFromDictionary()
        {
            var symbol = new Symbol<int>();
            var innerDictionary = new Dictionary<ISymbol, object> { {symbol, 42}};
            var dictionary = new SymbolDictionary(innerDictionary);

            dictionary.Set(symbol, Maybe<int>.NoValue);

            Assert.AreEqual(0, innerDictionary.Count);
        }

        [Test]
        public void Set_ValueTwice_ReturnsSecondValue()
        {
            var symbol = new Symbol<int>();
            var innerDictionary = new Dictionary<ISymbol, object> { { symbol, 42 } };
            var dictionary = new SymbolDictionary(innerDictionary);

            dictionary.Set(symbol, 7);
            dictionary.Set(symbol, 42);

            Assert.AreEqual(1, innerDictionary.Count);
            Assert.AreEqual(42, (int)innerDictionary[symbol]);
        }

        [Test]
        public void TryGet_WithEmptyDictionaryAndTypedSymbol_ReturnsNoValue()
        {
            var symbol = new Symbol<int>();
            var dictionary = new SymbolDictionary();

            Assert.IsTrue(dictionary.TryGet(symbol) == Maybe<int>.NoValue);
        }

        [Test]
        public void TryGet_WithEmptyDictionaryAndUntypedSymbol_ReturnsNoValue()
        {
            var symbol = new Symbol();
            var dictionary = new SymbolDictionary();

            Assert.IsTrue(dictionary.TryGet<int>(symbol) == Maybe<int>.NoValue);
        }
    }
}
