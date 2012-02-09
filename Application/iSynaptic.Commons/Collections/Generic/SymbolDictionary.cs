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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public class SymbolDictionary : ISymbolDictionary
    {
        private readonly IKeyedReaderWriter<ISymbol, Maybe<object>> _KeyedReaderWriter = null;

        public SymbolDictionary()
            : this(new Dictionary<ISymbol, object>())
        {
            
        }

        public SymbolDictionary(IDictionary<ISymbol, object> dictionary)
            : this(GetDictionaryBackedKeyedReaderWriter(dictionary))
        {
        }
        
        public SymbolDictionary(IKeyedReaderWriter<ISymbol, Maybe<object>> keyedReaderWriter)
        {
            _KeyedReaderWriter = Guard.NotNull(keyedReaderWriter, "keyedReaderWriter");
        }

        private static IKeyedReaderWriter<ISymbol, Maybe<object>> GetDictionaryBackedKeyedReaderWriter(IDictionary<ISymbol, object> dictionary)
        {
            Guard.NotNull(dictionary, "dictionary");
            
            if(dictionary.IsReadOnly)
                throw new ArgumentException("Dictionary provided must not be read-only.", "dictionary");

            return new KeyedReaderWriter<ISymbol, Maybe<object>>(dictionary.TryGetValue, (s, v) =>
            {
                if (v.HasValue != true)
                    return dictionary.Remove(s);

                dictionary[s] = v.Value;
                return true;
            }, () => dictionary.Keys);
        }

        public Maybe<T> TryGet<T>(ISymbol symbol)
        {
            Guard.NotNull(symbol, "symbol");

            return _KeyedReaderWriter.Get(symbol)
                .Cast<T>();
        }
        
        public void Set<T>(ISymbol symbol, Maybe<T> value)
        {
            Guard.NotNull(symbol, "symbol");
            _KeyedReaderWriter.Set(symbol, value.Cast<object>());
        }

        public IEnumerator<ISymbol> GetEnumerator()
        {
            return _KeyedReaderWriter.GetKeys()
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
