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
        private readonly IComparer<Type> _comparer = new TypeHierarchyComparer();

        private interface IBase { }
        private interface IDerived : IBase { }
        private class Base : IBase { }
        private class Derived : Base, IDerived { }

        private readonly List<Type> _SortedTypes;

        public TypeHierarchyComparerTests()
        {
            _SortedTypes = new[] { null, typeof(IBase), typeof(Derived), typeof(Base), typeof(IDerived), typeof(string), typeof(int), typeof(IFormattable) }
                .OrderBy(x => x, _comparer)
                .ToList();
        }

        [Test]
        public void CompareViaSort_InCorrectOrder()
        {
            Assert.IsTrue(_SortedTypes
                .SelectMany((r, i) => _SortedTypes.Take(i).Select(l => _comparer.Compare(l, r)))
                .All(x => x == -1 || x == 0));
        }

        [Test]
        public void Compare_NullSortsLowestOfAll()
        {
            Assert.AreEqual(-1, _comparer.Compare(null, typeof(Object)));
            Assert.AreEqual(1, _comparer.Compare(typeof(Object), null));

            Assert.AreEqual(-1, _comparer.Compare(null, typeof(IBase)));
            Assert.AreEqual(1, _comparer.Compare(typeof(IBase), null));

            Assert.AreEqual(-1, _comparer.Compare(null, typeof(Derived)));
            Assert.AreEqual(1, _comparer.Compare(typeof(Derived), null));
        }

        [Test]
        public void Compare_InterfacesAreLesser()
        {
            Assert.AreEqual(-1, _comparer.Compare(typeof(IBase), typeof(Object)));
            Assert.AreEqual(1, _comparer.Compare(typeof(Object), typeof(IBase)));

            Assert.AreEqual(-1, _comparer.Compare(typeof(IBase), typeof(Base)));
            Assert.AreEqual(1, _comparer.Compare(typeof(Base), typeof(IBase)));
            
            Assert.AreEqual(-1, _comparer.Compare(typeof(IBase), typeof(Derived)));
            Assert.AreEqual(1, _comparer.Compare(typeof(Derived), typeof(IBase)));
        }

        [Test]
        public void Compare_BaseInterfacesAreLesserThatDerivedInterfaces()
        {
            Assert.AreEqual(-1, _comparer.Compare(typeof(IBase), typeof(IDerived)));
            Assert.AreEqual(1, _comparer.Compare(typeof(IDerived), typeof(IBase)));
        }

        [Test]
        public void Compare_BaseClassesAreLesserThanDerivedClasses()
        {
            Assert.AreEqual(-1, _comparer.Compare(typeof(Base), typeof(Derived)));
            Assert.AreEqual(1, _comparer.Compare(typeof(Derived), typeof(Base)));
        }
    }
}
