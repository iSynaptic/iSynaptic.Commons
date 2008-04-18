using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace iSynaptic.Commons.Text.Parsing
{
    public static class SimpleScanner
    {
        public static IEnumerable<Token<TokenKind>> ScanText(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentOutOfRangeException("input");

            return ScanTextCore(new StringReader(input));
        }

        public static IEnumerable<Token<TokenKind>> ScanText(TextReader inputReader)
        {
            if (inputReader == null)
                throw new ArgumentNullException("inputReader");

            return ScanTextCore(inputReader);
        }

        private static IEnumerable<Token<TokenKind>> ScanTextCore(TextReader inputReader)
        {
            ScanningTextReader reader = inputReader as ScanningTextReader ?? new ScanningTextReader(inputReader);

            int ch = reader.Read();
            while (ch != -1)
            {
                if (IsWhitespaceCharacter(ch)) { reader.Read(); }
                else if (IsAlphaNumeric(ch)) { yield return ParseIdentifierOrNumber(reader); }
                else if (ch == '"' || ch == '\'') { yield return ParseString(reader); }
                else if (ch == '=' && reader.LookAhead(0) != '=')
                {
                    yield return CreateToken(TokenKind.AssignmentEquals, 1, reader);
                    reader.Read();
                }
                else
                    throw new Exception("Unrecognized character.");

                ch = reader.LastCharacterRead;
            }
        }

        private static Token<TokenKind> ParseString(ScanningTextReader reader)
        {
            Token<TokenKind> token = CreateToken(TokenKind.String, reader);

            StringBuilder builder = new StringBuilder();

            int ch = reader.Read();
            while (ch != -1 && ch != '\"')
            {
                if (ch == '\\')
                {
                    ch = reader.Read();

                    if (IsEscapable(ch))
                        builder.Append(GetEscapedCharacter(ch));
                    else
                        throw new ApplicationException("Unrecognized escape character.");
                }
                else
                {
                    builder.Append((char)ch);
                }

                ch = reader.Read();
            }

            reader.Read(); // consume ending quote

            token.Value = builder.ToString();
            token.Length = token.Value.Length + 2; // length of string plus beginnging and ending quote
            return token;
        }

        private static char GetEscapedCharacter(int ch)
        {
            switch (ch)
            {
                case '\'': return '\'';
                case '\"': return '\"';
                case '\\': return '\\';
                case '0': return '\0';
                case 'a': return '\a';
                case 'b': return '\b';
                case 'f': return '\f';
                case 'n': return '\n';
                case 'r': return '\r';
                case 't': return '\t';
                case 'v': return '\v';
                default:
                    throw new ApplicationException("Unrecognized escape character.");
            }
        }

        private static Token<TokenKind> ParseIdentifierOrNumber(ScanningTextReader reader)
        {
            Token<TokenKind> token = CreateToken(TokenKind.Number, reader);

            int ch = reader.LastCharacterRead;

            if (!(ch >= '0' && ch <= '9'))
                token.Kind = TokenKind.Identifier;
            else
                token.Kind = TokenKind.Number;

            StringBuilder builder = new StringBuilder();
            while (IsAlphaNumeric(ch) || ch == '-' || ch == '_')
            {
                if (token.Kind == TokenKind.Number && !(ch >= '0' && ch <= '9'))
                    throw new ApplicationException("Expected digit.");

                builder.Append((char)ch);

                ch = reader.Read();
            }

            token.Value = builder.ToString();
            token.Length = token.Value.Length;
            return token;
        }

        private static Token<TokenKind> CreateToken(TokenKind kind, ScanningTextReader reader)
        {
            return CreateToken(kind, 0, reader);
        }

        private static Token<TokenKind> CreateToken(TokenKind kind, int length, ScanningTextReader reader)
        {
            Token<TokenKind> token = new Token<TokenKind>();
            token.Kind = kind;

            token.Position = reader.Position;
            token.Column = reader.Column;
            token.Line = reader.Line;

            return token;
        }

        private static bool IsEscapable(int ch)
        {
            return (ch == '\'' ||
                    ch == '\"' ||
                    ch == '\\' ||
                    ch == '0' ||
                    ch == 'a' ||
                    ch == 'b' ||
                    ch == 'f' ||
                    ch == 'n' ||
                    ch == 'r' ||
                    ch == 't' ||
                    ch == 'v');
        }

        private static bool IsAlphaNumeric(int ch)
        {
            return (ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z');
        }

        private static bool IsWhitespaceCharacter(int ch)
        {
            return ch == ' ' || ch == 9 || ch == 10 || ch == 13;
        }
    }
}
