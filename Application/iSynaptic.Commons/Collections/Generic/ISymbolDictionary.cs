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

namespace iSynaptic.Commons.Collections.Generic
{
    public interface ISymbolDictionary
    {
        Maybe<T> TryGet<T>(ISymbol symbol);
        Maybe<T> TryGet<T>(ISymbol<T> symbol);
        T Get<T>(ISymbol symbol);
        T Get<T>(ISymbol<T> symbol);
        bool Set<T>(ISymbol symbol, Maybe<T> value);
        bool Set<T>(ISymbol<T> symbol, Maybe<T> value);
        bool Set<T>(ISymbol symbol, T value);
        bool Set<T>(ISymbol<T> symbol, T value);
    }
}