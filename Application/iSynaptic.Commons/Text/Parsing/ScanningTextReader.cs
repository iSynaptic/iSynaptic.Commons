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
using System.IO;

namespace iSynaptic.Commons.Text.Parsing
{
    public class ScanningTextReader : TextReader
    {
        private TextReader _InnerReader = null;
        private List<char> _LookAheadList = null;
        private int _LastCharacterRead = -1;

        private int _Position = -1;
        private int _Column = 0;
        private int _Line = 1;

        public ScanningTextReader(TextReader innerReader)
        {
            _InnerReader = Guard.NotNull(innerReader, "innerReader");
        }

        public int LastCharacterRead
        {
            get { return _LastCharacterRead; }
        }

        public int Position
        {
            get { return _Position; }
        }

        public int Column
        {
            get { return _Column; }
        }

        public int Line
        {
            get { return _Line; }
        }

        protected List<char> LookAheadList
        {
            get { return _LookAheadList ?? (_LookAheadList = new List<char>()); }
        }

        public int LookAhead(int index)
        {
            if (LookAheadList.Count >= (index + 1))
                return LookAheadList[index];

            int charactersToRead = (index + 1) - LookAheadList.Count;

            char[] buffer = new char[charactersToRead];
            int charactersRead = _InnerReader.ReadBlock(buffer, 0, charactersToRead);

            if (charactersRead > 0)
            {
                LookAheadList.AddRange(buffer);
                return LookAheadList[index];
            }
            
            return -1;
        }

        public override int Peek()
        {
            return LookAheadList.Count > 0 ? LookAheadList[0] : _InnerReader.Peek();
        }

        public override int Read()
        {
            bool lastCharacterWasNewline = (_LastCharacterRead == '\n');

            if (LookAheadList.Count <= 0)
                _LastCharacterRead = _InnerReader.Read();
            else
            {
                _LastCharacterRead = LookAheadList[0];
                LookAheadList.RemoveAt(0);
            }

            if (_LastCharacterRead == -1)
            {
                _Column = -1;
                _Line = -1;
                _Position = -1;
                return -1;
            }

            if (lastCharacterWasNewline)
            {
                _Column = 1;
                _Line++;
            }
            else
                _Column++;

            _Position++;
            return _LastCharacterRead;
        }
    }
}
