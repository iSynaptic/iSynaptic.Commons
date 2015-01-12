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
using System.IO;
using NUnit.Framework;

namespace iSynaptic.Commons.Text
{
    [TestFixture]
    public class IndentingTestWriterTests
    {
        [Test]
        public void Write_WithNoIndentation()
        {
            var sw = new StringWriter();
            var iw = new IndentingTextWriter(sw, "  ");

            iw.Write("Hello\r\nWorld");

            String result = sw.GetStringBuilder().ToString();

            Assert.AreEqual("Hello\r\nWorld", result);
        }

        [Test]
        public void Write_WithOneLevelOfIndentation()
        {
            var sw = new StringWriter();
            var iw = new IndentingTextWriter(sw, "  ");
            iw.IncreaseIndentation();

            iw.Write("Hello\r\nWorld");

            String result = sw.GetStringBuilder().ToString();

            Assert.AreEqual("  Hello\r\n  World", result);
        }

        [Test]
        public void Write_WithTwoLevelsOfIndentation()
        {
            var sw = new StringWriter();
            var iw = new IndentingTextWriter(sw, "  ");
            iw.IncreaseIndentation(2);

            iw.Write("Hello\r\nWorld");

            String result = sw.GetStringBuilder().ToString();

            Assert.AreEqual("    Hello\r\n    World", result);
        }

        [Test]
        public void Write_WithIndentationTokenContainingNewLine_DoesNotRecurse()
        {
            var sw = new StringWriter();
            var iw = new IndentingTextWriter(sw, "\r\n  ");
            iw.IncreaseIndentation(1);

            iw.Write("Hello\r\nWorld");

            String result = sw.GetStringBuilder().ToString();

            Assert.AreEqual("\r\n  Hello\r\n\r\n  World", result);
        }

        [Test]
        public void InitialIndendation_IsZero()
        {
            var sw = new StringWriter();
            var iw = new IndentingTextWriter(sw, "  ");

            Assert.AreEqual(0, iw.Indentation);
        }

        [Test]
        public void IncreasingIndentation_NeverOverflows()
        {
            var sw = new StringWriter();
            var iw = new IndentingTextWriter(sw, "  ");

            iw.IncreaseIndentation(Int32.MaxValue);
            iw.IncreaseIndentation(42);

            Assert.AreEqual(Int32.MaxValue, iw.Indentation);
        }

        [Test]
        public void DecreasingIndentation_NeverDropsBelowZero()
        {
            var sw = new StringWriter();
            var iw = new IndentingTextWriter(sw, "  ");

            iw.DecreaseIndentation();
            iw.DecreaseIndentation();
            Assert.AreEqual(0, iw.Indentation);
        }

        [Test]
        public void IncreasingIndentation_ByMustBeGreaterThanZero()
        {
            var sw = new StringWriter();
            var iw = new IndentingTextWriter(sw, "  ");

            Assert.Throws<ArgumentOutOfRangeException>(() => iw.IncreaseIndentation(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => iw.IncreaseIndentation(-42));
        }

        [Test]
        public void DecreasingIndentation_ByMustBeGreaterThanZero()
        {
            var sw = new StringWriter();
            var iw = new IndentingTextWriter(sw, "  ");

            Assert.Throws<ArgumentOutOfRangeException>(() => iw.DecreaseIndentation(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => iw.DecreaseIndentation(-42));
        }
    }
}
