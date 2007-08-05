using System;
using System.Collections.Generic;
using System.Text;
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
            if (innerReader == null)
                throw new ArgumentNullException("innerReader");

            _InnerReader = innerReader;
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
            get
            {
                if (_LookAheadList == null)
                    _LookAheadList = new List<char>();

                return _LookAheadList;
            }
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
            else
                return -1;
        }

        public override int Peek()
        {
            if (LookAheadList.Count <= 0)
                return _InnerReader.Peek();

            return LookAheadList[0];
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
