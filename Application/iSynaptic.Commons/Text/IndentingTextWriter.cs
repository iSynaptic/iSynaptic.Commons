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

namespace iSynaptic.Commons.Text
{
    public class IndentingTextWriter : TextWriterDecorator
    {
        private Int32 _matchIndex = Environment.NewLine.Length;

        public IndentingTextWriter(TextWriter underlying, String indentationToken) : base(underlying)
        {
            IndentationToken = Guard.NotNullOrEmpty(indentationToken, "indentationToken");
        }

        public override void Write(Char value)
        {
            if (_matchIndex == Environment.NewLine.Length)
            {
                WriteIndentation();
                _matchIndex = 0;
            }

            _matchIndex = Environment.NewLine[_matchIndex] == value
                ? _matchIndex + 1
                : 0;

            base.Write(value);
        }

        public void IncreaseIndentation()
        {
            IncreaseIndentation(1);
        }

        public void IncreaseIndentation(Int32 increaseBy)
        {
            if (increaseBy <= 0) throw new ArgumentOutOfRangeException("increaseBy", "Provided value must be greater than 0.");

            Int32 newIndentation = Indentation + increaseBy;
            if (newIndentation < 0)
                newIndentation = Int32.MaxValue;

            Indentation = newIndentation;
        }

        public void DecreaseIndentation()
        {
            DecreaseIndentation(1);
        }

        public void DecreaseIndentation(Int32 decreaseBy)
        {
            if (decreaseBy <= 0) throw new ArgumentOutOfRangeException("decreaseBy", "Provided value must be greater than 0.");
            
            Int32 newIndentation = Indentation - decreaseBy;
            if (newIndentation < 0)
                newIndentation = 0;

            Indentation = newIndentation;
        }

        private void WriteIndentation()
        {
            for (int i = 0; i < Indentation; i++)
                WriteDirect(IndentationToken);
        }

        public Int32 Indentation { get; private set; }
        public String IndentationToken { get; private set; }
    }
}
