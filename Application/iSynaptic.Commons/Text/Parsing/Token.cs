using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.Text.Parsing
{
    public class Token<T>
    {
        private T _Kind = default(T);
        private int _Position = 0;
        private int _Column = 0;
        private int _Line = 0;
        private int _Length = 0;
        private string _Value = null;

        public Token()
        {
        }

        public Token(T kind, int position, int column, int line, int length, string value)
        {
            _Kind = kind;
            _Position = position;
            _Column = column;
            _Line = line;
            _Length = length;
            _Value = value;
        }

        public T Kind
        {
            get { return _Kind; }
            set { _Kind = value; }
        }

        public int Position
        {
            get { return _Position; }
            set { _Position = value; }
        }

        public int Column
        {
            get { return _Column; }
            set { _Column = value; }
        }

        public int Line
        {
            get { return _Line; }
            set { _Line = value; }
        }

        public int Length
        {
            get { return _Length; }
            set { _Length = value; }
        }

        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
    }
}
