using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using NUnit.Framework;

using iSynaptic.Commons.Text.Parsing;
using iSynaptic.Commons.Testing.NUnit;

namespace iSynaptic.Commons.Text.Parsing
{
    [TestFixture]
    public class SimpleScannerTests : NUnitBaseTestFixture
    {
        [Test]
        public void ScanNullInput()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => SimpleScanner.ScanText((string)null));
            Assert.Throws<ArgumentNullException>(() => SimpleScanner.ScanText((TextReader)null));
        }

        [Test]
        public void ScanIdentifier()
        {
            StringReader reader = new StringReader("counter");

            List<Token<TokenKind>> tokens = new List<Token<TokenKind>>(SimpleScanner.ScanText(reader));

            Assert.AreEqual(1, tokens.Count, "Token count was higher or lower than expected.");
            Assert.AreEqual(TokenKind.Identifier, tokens[0].Kind, "Token kind was not 'Identifier'.");
            Assert.AreEqual(7, tokens[0].Length, "Token length was not set correctly.");
            Assert.AreEqual(0, tokens[0].Position, "Token position was not set correctly.");
            Assert.AreEqual(1, tokens[0].Column, "Token column was not set correctly.");
            Assert.AreEqual(1, tokens[0].Line, "Token line was not set correctly.");
            Assert.AreEqual("counter", tokens[0].Value, "Token value was not 'counter'.");
        }

        [Test]
        public void ScanNumber()
        {
            StringReader reader = new StringReader("144000");

            List<Token<TokenKind>> tokens = new List<Token<TokenKind>>(SimpleScanner.ScanText(reader));

            Assert.AreEqual(1, tokens.Count, "Token count was higher or lower than expected.");
            Assert.AreEqual(TokenKind.Number, tokens[0].Kind, "Token kind was not 'Number'.");
            Assert.AreEqual(6, tokens[0].Length, "Token length was not set correctly.");
            Assert.AreEqual(0, tokens[0].Position, "Token position was not set correctly.");
            Assert.AreEqual(1, tokens[0].Column, "Token column was not set correctly.");
            Assert.AreEqual(1, tokens[0].Line, "Token line was not set correctly.");
            Assert.AreEqual("144000", tokens[0].Value, "Token value was not '144000'.");
        }

        [Test]
        public void ScanString()
        {
            StringReader reader = new StringReader("\"This is a string with a: \\\\ \\\" \\a \\b \\f \\t \\v \\r \\n \\' \\0\"");
            string expected = "This is a string with a: \\ \" \a \b \f \t \v \r \n \' \0";

            List<Token<TokenKind>> tokens = new List<Token<TokenKind>>(SimpleScanner.ScanText(reader));

            Assert.AreEqual(1, tokens.Count, "Token count was higher or lower than expected.");
            Assert.AreEqual(TokenKind.String, tokens[0].Kind, "Token kind was not 'String'.");
            //Assert.AreEqual(expected.Length + 2, tokens[0].Length, "Token length was not set correctly.");
            Assert.AreEqual(0, tokens[0].Position, "Token position was not set correctly.");
            Assert.AreEqual(1, tokens[0].Column, "Token column was not set correctly.");
            Assert.AreEqual(1, tokens[0].Line, "Token line was not set correctly.");
            Assert.AreEqual(expected, tokens[0].Value, "Scanner did not parse string correctly.");
        }

        [Test]
        public void ScanIdentifierAndNumber()
        {
            StringReader reader = new StringReader("counter 144000");

            List<Token<TokenKind>> tokens = new List<Token<TokenKind>>(SimpleScanner.ScanText(reader));

            Assert.AreEqual(2, tokens.Count, "Token count was higher or lower than expected.");

            Assert.AreEqual(TokenKind.Identifier, tokens[0].Kind, "First token kinds was not 'Identifier'.");
            Assert.AreEqual(TokenKind.Number, tokens[1].Kind, "Second token kinds was not 'Number'.");

            Assert.AreEqual(7, tokens[0].Length, "First token length was not set correctly.");
            Assert.AreEqual(0, tokens[0].Position, "First token position was not set correctly.");
            Assert.AreEqual(1, tokens[0].Column, "First token column was not set correctly.");
            Assert.AreEqual(1, tokens[0].Line, "First token line was not set correctly.");

            Assert.AreEqual(6, tokens[1].Length, "Second token length was not set correctly.");
            Assert.AreEqual(8, tokens[1].Position, "Second token position was not set correctly.");
            Assert.AreEqual(9, tokens[1].Column, "Second token column was not set correctly.");
            Assert.AreEqual(1, tokens[1].Line, "Second token line was not set correctly.");

            Assert.AreEqual("counter", tokens[0].Value, "First token value was not 'counter'.");
            Assert.AreEqual("144000", tokens[1].Value, "Second token value was not '144000'.");
        }
    }
}
