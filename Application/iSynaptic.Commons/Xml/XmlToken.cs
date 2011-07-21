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
using System.Linq;
using System.Text;
using System.Xml;

namespace iSynaptic.Commons.Xml
{
    public struct XmlToken
    {
        public static readonly XmlToken None = new XmlToken(XmlNodeType.None);
        public static readonly XmlToken EndElement = new XmlToken(XmlNodeType.EndElement);

        private readonly string _Name;
        private readonly XmlNodeType _Kind;
        private readonly string _Value;

        private readonly int? _LineNumber;
        private readonly int? _LinePosition;

        public static XmlToken FromReader(XmlReader reader)
        {
            Guard.NotNull(reader, "reader");
            return new XmlToken(reader);
        }

        private XmlToken(XmlNodeType kind)
        {
            _Name = null;
            _Kind = kind;
            _Value = null;
            _LineNumber = null;
            _LinePosition = null;
        }

        private XmlToken(XmlReader reader)
        {
            _Name = reader.Name;
            _Kind = reader.NodeType;
            _Value = reader.Value;
            _LineNumber = null;
            _LinePosition = null;

            var lineInfo = reader as IXmlLineInfo;
            if (lineInfo != null && lineInfo.HasLineInfo())
            {
                _LineNumber = lineInfo.LineNumber;
                _LinePosition = lineInfo.LinePosition;
            }
        }

        public string Name { get { return _Name; } }
        public XmlNodeType Kind { get { return _Kind; } }
        public string Value { get { return _Value; } }

        public int? LineNumber { get { return _LineNumber; } }
        public int? LinePosition { get { return _LinePosition; } }
    }

}
