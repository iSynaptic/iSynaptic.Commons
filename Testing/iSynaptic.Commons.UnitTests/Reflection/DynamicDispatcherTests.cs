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
using NUnit.Framework;

namespace iSynaptic.Commons.Reflection
{
    [TestFixture]
    public class DynamicDispatcherTests
    {
        private class TestTarget
        {
        }

        [Test]
        public void Build_WithTDelegateAsNonDelegateType_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => DynamicDispatcher.Build<String>(typeof (String)));
        }

        [Test]
        public void Build_WithDelegateThatHasNoParameters_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => DynamicDispatcher.Build<Action>(typeof (String)));
        }

        [Test]    
        public void Build_WithTargetTypeThatCantBePassedToFirstDelegateParameter_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => DynamicDispatcher.Build<Action<String>>(typeof (Int32)));
        }

        [Test]
        public void Build_WithNoCandidateMethods_ThrowsExceptionByDefault()
        {
            Assert.Throws<InvalidOperationException>(() => DynamicDispatcher.Build<Action<TestTarget>>(typeof(TestTarget), x => false));
        }

        [Test]
        public void Build_WithNoCandidateMethods_ReturnsNoOpDelegate_WhenConfigured()
        {
            var options = new DynamicDispatcherOptions(DynamicDispatcher.MissingMethodBehavior.ReturnNoOpDelegate);
            var delgate = DynamicDispatcher.Build<Action<String>>(typeof (String), m => false, options);

            delgate("Yo");
        }
    }
}
