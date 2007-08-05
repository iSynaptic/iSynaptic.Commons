using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

using iSynaptic.Commons.Text.Parsing;

namespace iSynaptic.Commons.Text
{
    public static class ProcessingInstructionParser
    {
        public static NameValueCollection Parse(string processingInstruction)
        {
            if (string.IsNullOrEmpty(processingInstruction))
                throw new ArgumentOutOfRangeException("processingInstruction");

            NameValueCollection results = new NameValueCollection();

            Token<TokenKind> attributeIdentifier = null;
            bool inAssignement = false;
            foreach (Token<TokenKind> token in SimpleScanner.ScanText(processingInstruction))
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

                    results.Add(attributeIdentifier.Value, token.Value);
                    attributeIdentifier = null;
                    inAssignement = false;
                }
                else
                {
                    if (inAssignement)
                        throw new ApplicationException("Expected string.");
                    else
                        throw new ApplicationException("Expected '='.");
                }
            }

            return results;
        }
    }
}
