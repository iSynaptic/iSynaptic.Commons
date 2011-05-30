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
        private readonly ParseContext _Context = null;

        protected DeclarativeXmlParser(XmlReader reader)
        {
            Guard.NotNull(reader, "reader");
            _Context = new ParseContext(reader);
        }

        #region Nested Types

        protected interface IUponBuilder : IFluentInterface
        {
            IAttributeMultiplicity Attribute<T>(string name, Action<T> action);
            IElementMultiplicity ContentElement<T>(string name, Action<T> action);
            IElementMultiplicity Element(string name, Action action);

            void IgnoreUnrecognizedAttributes();
            void IgnoreUnrecognizedElements();
            void IgnoreUnrecognizedText();
        }

        protected interface IAttributeMultiplicity : IFluentInterface
        {
            void Optional();
        }

        protected interface IElementMultiplicity : IFluentInterface
        {
            void ZeroOrOne();
            void ZeroOrMore();
            void OneOrMore();
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

            public IAttributeMultiplicity Attribute<T>(string name, Action<T> action)
            {
                var matcher = new Matcher<T>(_Parent, name, XmlNodeType.Attribute, pc => Maybe.Return(pc.Token.Value).Select(Convert<string, T>.From), action);
                Matchers.Add(matcher);

                return matcher;
            }

            public IElementMultiplicity ContentElement<T>(string name, Action<T> action)
            {
                Func<ParseContext, Maybe<T>> converter = pc =>
                {
                    var token = pc.Token;

                    pc.MoveNext();

                    if (pc.Token.Kind != XmlNodeType.Text)
                    {
                        pc.Panic();

                        string message = string.Format("Content element '{0}' must only contain text.", name);
                        pc.Errors.Add(new ParseError(message, token));

                        return Maybe<T>.NoValue;
                    }

                    var data = Maybe
                        .Return(pc.Token.Value)
                        .Select(Convert<string, T>.From);

                    pc.MoveNext();

                    if (pc.Token.Kind != XmlNodeType.EndElement)
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

            public IElementMultiplicity Element(string name, Action action)
            {
                var matcher = new Matcher<object>(_Parent, name, XmlNodeType.Element, pc => null, x => action());
                Matchers.Add(matcher);

                return matcher;
            }

            public void ExecuteMatch(ParseContext pc)
            {
                var matcher = Matchers.FirstOrDefault(x => x.CanExecute(pc));
                if (matcher == null)
                {
                    if (pc.Token.Kind == XmlNodeType.Attribute)
                    {
                        if (!_IgnoreUnrecognizedAttributes)
                            pc.Errors.Add(new ParseError(string.Format("Unexpected attribute: '{0}'.", pc.Token.Name), pc.Token));
                    }
                    else if (pc.Token.Kind == XmlNodeType.Text)
                    {
                        if (!_IgnoreUnrecognizedText)
                            pc.Errors.Add(new ParseError(string.Format("Unexpected text: '{0}'.", pc.Token.Value), pc.Token));
                    }
                    else if (pc.Token.Kind == XmlNodeType.Element)
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

        protected class Matcher<T> : IMatcher, IElementMultiplicity, IAttributeMultiplicity
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

            void IAttributeMultiplicity.Optional() { _Multiplicity = Multiplicity.ZeroOrOne; }

            void IElementMultiplicity.ZeroOrOne() { _Multiplicity = Multiplicity.ZeroOrOne; }
            void IElementMultiplicity.ZeroOrMore() { _Multiplicity = Multiplicity.ZeroOrMore; }
            void IElementMultiplicity.OneOrMore() { _Multiplicity = Multiplicity.OneOrMore; }

            bool IMatcher.CanExecute(ParseContext context)
            {
                return context.Token.Name == _Name &&
                       context.Token.Kind == _NodeType;
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
                    _Selector(context)
                        .OnValue(x => _MatchAction(x))
                        .CatchExceptions()
                        .OnException(x => context.Errors.Add(new ParseError(string.Format("Unable to interpet data; exception occured: {0}", x.Message), context.Token)))
                        .Run();
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

        protected class ParseContext
        {
            public ParseContext(XmlReader reader)
            {
                Guard.NotNull(reader, "reader");
                Tokens = ParseElement(reader).GetEnumerator();
                Token = XmlToken.None;

                Errors = new List<ParseError>();
            }

            private static IEnumerable<XmlToken> ParseElement(XmlReader reader)
            {
                if (reader.NodeType == XmlNodeType.None && reader.EOF != true)
                    reader.Read();

                yield return XmlToken.FromReader(reader);
                bool isEmptyElement = reader.IsEmptyElement;

                if (reader.HasAttributes)
                {
                    while (reader.MoveToNextAttribute())
                        yield return XmlToken.FromReader(reader);
                }

                if (isEmptyElement != true)
                {
                    while (ReadPastWhiteSpace(reader))
                    {
                        if (reader.NodeType == XmlNodeType.EndElement)
                        {
                            yield return XmlToken.FromReader(reader);
                            break;
                        }

                        if (reader.NodeType == XmlNodeType.Text)
                            yield return XmlToken.FromReader(reader);

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
                while (Token.Kind != XmlNodeType.EndElement || expectedEndElements > 0)
                {
                    if (Token.Kind == XmlNodeType.Element)
                        expectedEndElements++;

                    if (Token.Kind == XmlNodeType.EndElement)
                        expectedEndElements--;

                    MoveNext();
                }
            }

            public XmlToken Token { get; private set; }
            public List<ParseError> Errors { get; private set; }

            private IEnumerator<XmlToken> Tokens { get; set; }
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

        protected ParseContext Context
        {
            get { return _Context; }
        }

        #endregion

        protected IEnumerable<ParseError> Upon(Action<IUponBuilder> builderActions)
        {
            if (Context.Token.Kind == XmlNodeType.None)
                Context.MoveNext();

            if (Context.Token.Kind != XmlNodeType.Element)
                throw new InvalidOperationException("Upon can only be called while reader's current node is the start of an element");

            var builder = new UponBuilder(Context.Token);
            builderActions(builder);

            Context.MoveNext();

            while (Context.Token.Kind != XmlNodeType.EndElement)
                builder.ExecuteMatch(Context);

            builder.ValidateMatchers(Context);

            return Context.Errors;
        }
    }
}
