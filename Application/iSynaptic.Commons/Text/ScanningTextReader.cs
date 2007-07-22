using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace iSynaptic.Commons.Text
{
    public class ScanningTextReader : TextReader
    {
        private TextReader _InnerReader = null;
        private List<char> _LookAheadList = null;

        public ScanningTextReader(TextReader innerReader)
        {
            if (innerReader == null)
                throw new ArgumentNullException("innerReader");

            _InnerReader = innerReader;
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
            if (LookAheadList.Count <= 0)
                return _InnerReader.Read();

            int returnValue = LookAheadList[0];
            LookAheadList.RemoveAt(0);

            return returnValue;
        }
    }
}
