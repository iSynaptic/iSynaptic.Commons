using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using iSynaptic.Commons.AOP;

namespace iSynaptic.Commons.Xml
{
    public abstract class DeclarativeXmlParser
    {
        #region Nested Types

        protected interface IUponBuilder
        {
            Matcher<T> Attribute<T>(string name, Action<T> action);
            Matcher<T> ContentElement<T>(string name, Action<T> action);
            Matcher<string> Element(string name, Action<string> action);

            void IgnoreUnrecognizedAttributes();
            void IgnoreUnrecognizedElements();
            void IgnoreUnrecognizedText();
        }

        protected class UponBuilder : IUponBuilder
        {
            private List<IMatcher> _Matchers = null;
            private readonly XmlToken _Parent = default(XmlToken);
            private bool _IgnoreUnrecognizedAttributes = false;
            private bool _IgnoreUnrecognizedElements = false;
            private bool _IgnoreUnrecognizedText = false;

            public UponBuilder(XmlToken parent)
            {
                _Parent = parent;
            }

            public void IgnoreUnrecognizedAttributes() { _IgnoreUnrecognizedAttributes = true; }
            public void IgnoreUnrecognizedElements() { _IgnoreUnrecognizedElements = true; }
            public void IgnoreUnrecognizedText() { _IgnoreUnrecognizedText = true; }

            public Matcher<T> Attribute<T>(string name, Action<T> action)
            {
                var matcher = new Matcher<T>(_Parent, name, XmlNodeType.Attribute, pc => Convert<T>(pc.Token.Value), action);
                Matchers.Add(matcher);

                return matcher;
            }

            public Matcher<T> ContentElement<T>(string name, Action<T> action)
            {
                Func<ParseContext, Maybe<T>> converter = pc =>
                {
                    var token = pc.Token;

                    pc.MoveNext();

                    if (pc.Token.NodeType != XmlNodeType.Text)
                    {
                        pc.Panic();

                        string message = string.Format("Content element '{0}' must only contain text.", name);
                        pc.Errors.Add(new ParseError(message, token));

                        return Maybe<T>.NoValue;
                    }

                    var data = Convert<T>(pc.Token.Value);

                    pc.MoveNext();

                    if (pc.Token.NodeType != XmlNodeType.EndElement)
                    {
                        pc.Panic();

                        string message = string.Format("Content element '{0}' must only contain text.", name);
                        pc.Errors.Add(new ParseError(message, token));

                        return Maybe<T>.NoValue;
                    }

                    return data;
                };

                var matcher = new Matcher<T>(_Parent, name, XmlNodeType.Element, converter, action);
                Matchers.Add(matcher);

                return matcher;
            }

            public Matcher<string> Element(string name, Action<string> action)
            {
                var matcher = new Matcher<string>(_Parent, name, XmlNodeType.Element, pc => pc.Token.Name, action);
                Matchers.Add(matcher);

                return matcher;
            }

            protected Maybe<T> Convert<T>(string value)
            {
                if (typeof(T) == typeof(int))
                {
                    int result = default(int);

                    if (!int.TryParse(value, out result))
                        return Maybe<T>.NoValue;

                    return new Maybe<T>(Cast<int, T>.With(result));
                }

                if (typeof(T) == typeof(DateTime))
                {
                    DateTime result = default(DateTime);

                    if (!DateTime.TryParse(value, out result))
                        return Maybe<T>.NoValue;

                    return new Maybe<T>(Cast<DateTime, T>.With(result));
                }

                if (typeof(T) == typeof(string))
                    return Cast<string, T>.With(value);

                return Maybe<T>.NoValue;
            }

            public void ExecuteMatch(ParseContext pc)
            {
                var matcher = Matchers.FirstOrDefault(x => x.CanExecute(pc));
                if (matcher == null)
                {
                    if (pc.Token.NodeType == XmlNodeType.Attribute)
                    {
                        if (!_IgnoreUnrecognizedAttributes)
                            pc.Errors.Add(new ParseError(string.Format("Unexpected attribute: '{0}'.", pc.Token.Name), pc.Token));
                    }
                    else if (pc.Token.NodeType == XmlNodeType.Text)
                    {
                        if (!_IgnoreUnrecognizedText)
                            pc.Errors.Add(new ParseError(string.Format("Unexpected text: '{0}'.", pc.Token.Value), pc.Token));
                    }
                    else if (pc.Token.NodeType == XmlNodeType.Element)
                    {
                        if (!_IgnoreUnrecognizedElements)
                            pc.Errors.Add(new ParseError(string.Format("Unexpected element: '{0}'.", pc.Token.Name), pc.Token));

                        pc.Panic();
                    }
                }
                else
                    matcher.Execute(pc);

                pc.MoveNext();
            }

            public void ValidateMatchers(ParseContext pc)
            {
                foreach (var matcher in Matchers)
                    matcher.ValidateMatcher(pc);
            }

            private List<IMatcher> Matchers
            {
                get { return _Matchers ?? (_Matchers = new List<IMatcher>()); }
            }
        }

        protected enum Multiplicity
        {
            One,
            ZeroOrOne,
            ZeroOrMore,
            OneOrMore
        }

        protected interface IMatcher
        {
            bool CanExecute(ParseContext context);
            void Execute(ParseContext context);

            void ValidateMatcher(ParseContext context);
        }

        protected class Matcher<T> : IMatcher
        {
            private readonly XmlToken _Parent;
            private readonly string _Name;
            private readonly XmlNodeType _NodeType;
            private readonly Func<ParseContext, Maybe<T>> _Selector;
            private readonly Action<T> _MatchAction;

            private Multiplicity _Multiplicity = Multiplicity.One;

            private int _ExecutionCount = 0;

            public Matcher(XmlToken parent, string name, XmlNodeType nodeType, Func<ParseContext, Maybe<T>> selector, Action<T> matchAction)
            {
                _Parent = parent;
                _Name = name;
                _NodeType = nodeType;
                _Selector = selector;
                _MatchAction = matchAction;
            }

            public void ZeroOrOne() { _Multiplicity = Multiplicity.ZeroOrOne; }
            public void ZeroOrMore() { _Multiplicity = Multiplicity.ZeroOrMore; }
            public void OneOrMore() { _Multiplicity = Multiplicity.OneOrMore; }

            bool IMatcher.CanExecute(ParseContext context)
            {
                return context.Token.Name == _Name &&
                       context.Token.NodeType == _NodeType;
            }

            void IMatcher.Execute(ParseContext context)
            {
                _ExecutionCount++;
                bool shouldExecuteAtMostOnce = _Multiplicity == Multiplicity.One ||
                                             _Multiplicity == Multiplicity.ZeroOrOne;

                if (shouldExecuteAtMostOnce && _ExecutionCount > 1)
                {
                    var token = context.Token;

                    _Selector(context);

                    string nodeType = _NodeType == XmlNodeType.Text
                                          ? "text node"
                                          : _NodeType.ToString().ToLower();

                    string message = string.Format("More than one '{0}' {1} is not allowed.", _Name, nodeType);
                    context.Errors.Add(new ParseError(message, token));
                }
                else
                {
                    var data = _Selector(context);
                    if (data.HasValue)
                        _MatchAction(data.Value);
                }
            }

            void IMatcher.ValidateMatcher(ParseContext context)
            {
                bool required = _Multiplicity == Multiplicity.One ||
                                _Multiplicity == Multiplicity.OneOrMore;

                if (required && _ExecutionCount <= 0)
                {
                    string nodeType = _NodeType == XmlNodeType.Text
                                          ? "text node"
                                          : _NodeType.ToString().ToLower();

                    string message = string.Format("At least one '{0}' {1} is required.", _Name, nodeType);
                    context.Errors.Add(new ParseError(message, _Parent));
                }
            }
        }

        protected class ParseContext : Scope<ParseContext>
        {
            public ParseContext(XmlReader reader)
                : base(ScopeBounds.Thread, ScopeNesting.Prohibited)
            {
                Guard.NotNull(reader, "reader");
                Tokens = ParseElement(reader).GetEnumerator();
                Token = XmlToken.None;

                Errors = new List<ParseError>();
            }

            private static IEnumerable<XmlToken> ParseElement(XmlReader reader)
            {
                if (reader.NodeType == XmlNodeType.None)
                    reader.Read();

                yield return new XmlToken(reader);
                bool isEmptyElement = reader.IsEmptyElement;

                if (reader.HasAttributes)
                {
                    while (reader.MoveToNextAttribute())
                        yield return new XmlToken(reader);
                }

                if (isEmptyElement != true)
                {
                    while (ReadPastWhiteSpace(reader))
                    {
                        if (reader.NodeType == XmlNodeType.EndElement)
                        {
                            yield return new XmlToken(reader);
                            break;
                        }

                        if (reader.NodeType == XmlNodeType.Text)
                            yield return new XmlToken(reader);

                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            foreach (var token in ParseElement(reader))
                                yield return token;
                        }
                    }
                }
                else
                    yield return XmlToken.EndElement;
            }

            private static bool ReadPastWhiteSpace(XmlReader reader)
            {
                bool results = false;

                do
                {
                    results = reader.Read();
                } while (reader.NodeType == XmlNodeType.Whitespace || reader.NodeType == XmlNodeType.SignificantWhitespace);

                return results;
            }

            public bool MoveNext()
            {
                bool results = Tokens.MoveNext();
                Token = Tokens.Current;

                return results;
            }

            public void Panic()
            {
                MoveNext();

                int expectedEndElements = 0;
                while (Token.NodeType != XmlNodeType.EndElement || expectedEndElements > 0)
                {
                    if (Token.NodeType == XmlNodeType.Element)
                        expectedEndElements++;

                    if (Token.NodeType == XmlNodeType.EndElement)
                        expectedEndElements--;

                    MoveNext();
                }
            }

            public XmlToken Token { get; private set; }
            public List<ParseError> Errors { get; private set; }

            private IEnumerator<XmlToken> Tokens { get; set; }
            public static ParseContext Current
            {
                get { return GetCurrentScope(); }
            }
        }

        public struct XmlToken
        {
            public static readonly XmlToken None = new XmlToken(XmlNodeType.None);
            public static readonly XmlToken EndElement = new XmlToken(XmlNodeType.EndElement);

            private readonly string _Name;
            private readonly XmlNodeType _NodeType;
            private readonly string _Value;

            private readonly int? _LineNumber;
            private readonly int? _LinePosition;

            private XmlToken(XmlNodeType nodeType)
            {
                _Name = null;
                _NodeType = nodeType;
                _Value = null;
                _LineNumber = null;
                _LinePosition = null;
            }

            public XmlToken(XmlReader reader)
            {
                Guard.NotNull(reader, "reader");

                _Name = reader.Name;
                _NodeType = reader.NodeType;
                _Value = reader.Value;

                var lineInfo = reader as IXmlLineInfo;
                if (lineInfo != null && lineInfo.HasLineInfo())
                {
                    _LineNumber = lineInfo.LineNumber;
                    _LinePosition = lineInfo.LinePosition;
                }
                else
                {
                    _LineNumber = null;
                    _LinePosition = null;
                }
            }

            public string Name { get { return _Name; } }
            public XmlNodeType NodeType { get { return _NodeType; } }
            public string Value { get { return _Value; } }

            public int? LineNumber { get { return _LineNumber; } }
            public int? LinePosition { get { return _LinePosition; } }
        }

        public class ParseError
        {
            public ParseError(string message, XmlToken token)
            {
                Message = message;
                Token = token;
            }

            public string Message { get; private set; }
            public XmlToken Token { get; private set; }
        }

        #endregion

        protected static IEnumerable<ParseError> Upon(XmlReader reader, Action<IUponBuilder> builderActions)
        {
            if (ParseContext.Current == null)
            {
                Guard.NotNull(reader, "reader");
                using (var pc = new ParseContext(reader))
                {
                    Upon(builderActions);
                    return pc.Errors;
                }
            }

            if (reader != null)
                throw new ArgumentException("You can only use one xml reader at a time.");

            Upon(builderActions);
            return ParseContext.Current.Errors;
        }

        protected static IEnumerable<ParseError> Upon(Action<IUponBuilder> builderActions)
        {
            if (ParseContext.Current == null)
                throw new InvalidOperationException("No xml reader is available.");

            var pc = ParseContext.Current;

            if (pc.Token.NodeType == XmlNodeType.None)
                pc.MoveNext();

            if (pc.Token.NodeType != XmlNodeType.Element)
                throw new InvalidOperationException("Upon can only be called while reader's current node is the start of an element");


            var builder = new UponBuilder(pc.Token);
            builderActions(builder);

            pc.MoveNext();

            while (pc.Token.NodeType != XmlNodeType.EndElement)
                builder.ExecuteMatch(pc);

            builder.ValidateMatchers(pc);

            return ParseContext.Current.Errors;
        }
    }

}
