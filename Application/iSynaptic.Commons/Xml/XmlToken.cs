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
