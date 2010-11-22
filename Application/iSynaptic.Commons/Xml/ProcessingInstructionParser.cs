using System;
using System.Collections.Generic;
using System.Xml;

using iSynaptic.Commons.Text.Parsing;

namespace iSynaptic.Commons.Xml
{
    public static class ProcessingInstructionParser
    {
        public static IEnumerable<ProcessingInstruction> ParseAll(XmlReader reader)
        {
            while (CanContinueToParse(reader))
            {
                if (reader.NodeType == XmlNodeType.ProcessingInstruction)
                    yield return Parse(reader.LocalName, reader.Value);

                reader.Read();
            }
        }

        public static ProcessingInstruction Parse(string name, string attributes)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentOutOfRangeException("name");

            if (string.IsNullOrEmpty(attributes))
                throw new ArgumentOutOfRangeException("attributes");

           return new ProcessingInstruction(name, ParseAttributes(attributes));
        }

        private static IEnumerable<KeyValuePair<string, string>> ParseAttributes(string attributes)
        {
            Token<TokenKind> attributeIdentifier = null;
            bool inAssignement = false;
            foreach (Token<TokenKind> token in SimpleScanner.ScanText(attributes))
            {
                if (attributeIdentifier == null)
                {
                    if (token.Kind != TokenKind.Identifier)
                        throw new ApplicationException("Expected identifier.");

                    attributeIdentifier = token;
                }
                else if (token.Kind == TokenKind.AssignmentEquals) { inAssignement = true; }
                else if (token.Kind == TokenKind.String)
                {
                    if (inAssignement != true)
                        throw new ApplicationException("Expected '='.");

                    yield return new KeyValuePair<string, string>(attributeIdentifier.Value, token.Value);
                    attributeIdentifier = null;
                    inAssignement = false;
                }
                else
                {
                    throw new ApplicationException("Expected string.");
                }
            }
        }

        private static bool CanContinueToParse(XmlReader reader)
        {
            return
                reader.NodeType == XmlNodeType.None ||
                reader.NodeType == XmlNodeType.XmlDeclaration ||
                reader.NodeType == XmlNodeType.ProcessingInstruction ||
                reader.NodeType == XmlNodeType.Whitespace ||
                reader.NodeType == XmlNodeType.SignificantWhitespace;
        }

    }
}
