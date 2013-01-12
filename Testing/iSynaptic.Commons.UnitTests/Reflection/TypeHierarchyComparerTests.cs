// The MIT License
// 
// Copyright (c) 2012 Jordan E. Terrell
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
using NUnit.Framework;

namespace iSynaptic.Commons.Reflection
{
    public class TypeHierarchyComparerTests
    {
        private interface IBase { }
        private interface IDerived : IBase { }
        private class Base : IBase { }
        private class Derived : Base, IDerived { }

        private readonly List<Type> _SortedTypes;

        public TypeHierarchyComparerTests()
        {
            _SortedTypes = new[] { typeof(IBase), typeof(Derived), typeof(Base), typeof(IDerived), typeof(string), typeof(int), typeof(IFormattable) }.ToList();
            _SortedTypes.Sort(new TypeHierarchyComparer());

        }

        [Test]
        public void CompareViaSort_InterfacesAreLesser()
        {
            Assert.IsTrue(_SortedTypes.Take(3).All(x => x.IsInterface));
        }

        [Test]
        public void CompareViaSort_BaseInterfacesAreLesserThatDerivedInterfaces()
        {
            Assert.IsTrue(_SortedTypes.IndexOf(typeof(IBase)) < _SortedTypes.IndexOf(typeof(IDerived)));
        }

        [Test]
        public void CompareViaSort_BaseClassesAreLesserThanDerivedClasses()
        {
            Assert.IsTrue(_SortedTypes.IndexOf(typeof(Base)) < _SortedTypes.IndexOf(typeof(Derived)));
        }
    }
}
