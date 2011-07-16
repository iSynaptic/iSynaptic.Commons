using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using iSynaptic.Commons.Collections.Generic;
using iSynaptic.Commons.Linq;
using NUnit.Framework;

namespace iSynaptic.Commons.Xml
{
    [TestFixture]
    public class DeclarativeXmlParserTests
    {
        private class Library
        {
            private List<BookCategory> _Categories = null;

            public string Owner { get; set; }
            public string Location { get; set; }
            public string Comment { get; set; }

            public DateTime? LastUpdated { get; set; }

            public List<BookCategory> Categories
            {
                get { return _Categories ?? (_Categories = new List<BookCategory>()); }
            }
        }

        private class BookCategory
        {
            private List<Book> _Books = null;

            public string Name { get; set; }

            public List<Book> Books
            {
                get { return _Books ?? (_Books = new List<Book>());}
            }
        }

        private class Book
        {
            public string Title { get; set; }
            public string Author { get; set; }
            public DateTime PublishDate { get; set; }

            public string ISBN { get; set; }
            public bool RequiredReading { get; set; }
        }

        private sealed class TestParser : DeclarativeXmlParser
        {
            private TestParser(XmlReader reader) : base(reader)
            {
            }

            public static Tuple<Library, IEnumerable<ParseError>> Parse(XmlReader reader)
            {
                var parser = new TestParser(reader);
                var library = parser.Library();

                return Tuple.Create(library, parser.Context.Errors.AsEnumerable());
            }

            private Library Library()
            {
                var library = new Library();

                Upon(b =>
                {
                    b.Attribute<string>("owner", x => library.Owner = x);
                    b.Attribute<string>("location", x => { if(x == "throw") throw new InvalidOperationException("Bad stuff here!"); library.Location = x; });

                    b.ContentElement<string>("comment", x => library.Comment = x)
                        .ZeroOrOne();
                    
                    b.Attribute<DateTime>("lastUpdated", x => library.LastUpdated = x)
                        .Optional();

                    b.Element("category", () => library.Categories.Add(BookCategory()))
                        .OneOrMore();
                });

                return library;
            }

            private BookCategory BookCategory()
            {
                var category = new BookCategory();

                Upon(b =>
                {
                    b.IgnoreUnrecognizedAttributes();

                    b.ContentElement<string>("name", x => category.Name = x);
                    b.Element("book", () => category.Books.Add(Book()))
                        .ZeroOrMore();
                });

                return category;
            }

            private Book Book()
            {
                var book = new Book();

                Upon(b =>
                {
                    b.IgnoreUnrecognizedElements();
                    b.IgnoreUnrecognizedText();

                    b.Attribute<string>("title", x => book.Title = x);
                    b.Attribute<string>("author", x => book.Author = x);
                    b.Attribute<DateTime>("publishDate", x => book.PublishDate = x);

                    b.Attribute<string>("isbn", x => book.ISBN = x)
                        .Optional();

                    b.Attribute<bool>("requiredReading", x => book.RequiredReading = x)
                        .Optional();
                });

                return book;
            }
        }

        [Test]
        public void TestXml_IsValid()
        {
            var xml = BuildValidXml();
            var r = XmlReader.Create(new StringReader(xml.ToString()));
            var results = TestParser.Parse(r);

            AssertErrorCount(0, results.Item2);
        }

        [Test]
        public void NotStartingOnElement_ThrowsException()
        {
            var xml = BuildValidXml();
            var r = XmlReader.Create(new StringReader(xml.ToString()));

            r.Read();
            r.MoveToNextAttribute();

            Assert.Throws<InvalidOperationException>(() => TestParser.Parse(r));
        }

        [Test]
        public void RequiredAttributes_ParseCorrectly()
        {
            var xml = BuildValidXml();

            var r = XmlReader.Create(new StringReader(xml.ToString()));

            var results = TestParser.Parse(r);
            var library = results.Item1;

            Assert.AreEqual("iSynaptic", library.Owner);
            Assert.AreEqual("Office", library.Location);
            AssertErrorCount(0, results.Item2);
        }

        [Test]
        public void RequiredElement_ParsesCorrectly()
        {
            var xml = BuildValidXml();

            var r = XmlReader.Create(new StringReader(xml.ToString()));

            var results = TestParser.Parse(r);
            var library = results.Item1;

            Assert.IsTrue(library.Categories.Count >= 1);
            Assert.IsTrue(library.Categories.Any(x => x.Name == "Testing"));

            AssertErrorCount(0, results.Item2);
        }

        [Test]
        public void OptionalAttribute_ParsesCorrectly()
        {
            var lastUpdated = DateTime.Parse("01/28/2011");

            var xml = BuildValidXml();
            xml.Add(new XAttribute("lastUpdated", lastUpdated.ToString()));

            var r = XmlReader.Create(new StringReader(xml.ToString()));

            var results = TestParser.Parse(r);
            var library = results.Item1;

            Assert.AreEqual(lastUpdated, library.LastUpdated.Value);

            AssertErrorCount(0, results.Item2);
        }

        [Test]
        public void OptionalElement_ParsesCorrectly()
        {
            string comment = "No book lending here!!!";

            var xml = BuildValidXml();
            xml.Add(new XElement("comment", comment));

            var r = XmlReader.Create(new StringReader(xml.ToString()));

            var results = TestParser.Parse(r);
            var library = results.Item1;

            Assert.AreEqual(comment, library.Comment);

            AssertErrorCount(0, results.Item2);
        }

        [Test]
        public void ExtraElement_GeneratesErrorWhilePreservingFirstComment()
        {
            string comment = "No book lending here!!!";

            var xml = BuildValidXml();
            xml.Add(new XElement("comment", comment));
            xml.Add(new XElement("comment", "This should cause an error!!!"));

            var r = XmlReader.Create(new StringReader(xml.ToString()));

            var results = TestParser.Parse(r);
            var library = results.Item1;

            Assert.AreEqual(comment, library.Comment);

            AssertErrorCount(1, results.Item2);
        }

        [Test]
        public void MissingAttribute_GeneratesError()
        {
            var xml = BuildValidXml();
            xml.Attribute("owner")
                .Remove();

            var r = XmlReader.Create(new StringReader(xml.ToString()));
            var results = TestParser.Parse(r);

            AssertErrorCount(1, results.Item2);
        }

        [Test]
        public void MissingElement_GeneratesError()
        {
            var xml = BuildValidXml();
            xml.Elements("category")
                .Remove();

            var r = XmlReader.Create(new StringReader(xml.ToString()));
            var results = TestParser.Parse(r);

            AssertErrorCount(1, results.Item2);
        }

        [Test]
        public void UnexpectedElement_GeneratesErrorAndParsingContinues()
        {
            var badElement = new XElement("bad",
                                new XAttribute("message", "I am not expected!"),
                                new XElement("additionalMessages",
                                            new XElement("message", "Mwwwahahahaha!")));

            var xml = BuildValidXml();
            xml.AddFirst(badElement);

            var r = XmlReader.Create(new StringReader(xml.ToString()));
            var results = TestParser.Parse(r);
            var library = results.Item1;

            Assert.AreEqual(xml.Elements("category").Count(), library.Categories.Count);

            AssertErrorCount(1, results.Item2);
        }

        [Test]
        public void UnexpectedAttribute_GeneratesErrorAndParsingContinues()
        {
            var xml = BuildValidXml();
            xml.SetAttributeValue("badAttribute", "This should cause an error!");

            var r = XmlReader.Create(new StringReader(xml.ToString()));
            var results = TestParser.Parse(r);
            var library = results.Item1;

            Assert.AreEqual(xml.Elements("category").Count(), library.Categories.Count);

            AssertErrorCount(1, results.Item2);
        }

        [Test]
        public void UnexpectedText_GeneratesErrorAndParsingContinues()
        {
            var xml = BuildValidXml();
            xml.AddFirst("Random Text");

            var r = XmlReader.Create(new StringReader(xml.ToString()));
            var results = TestParser.Parse(r);
            var library = results.Item1;

            Assert.AreEqual(xml.Elements("category").Count(), library.Categories.Count);

            AssertErrorCount(1, results.Item2);
        }

        [Test]
        public void UnexpectedElement_CanBeIgnoredAndParsingContinues()
        {
            var ignoredElement = new XElement("misc", "Here lies misc extra stuff!");

            var xml = BuildValidXml();
            var firstBook = xml.Element("category")
                                .Element("book");
            
            firstBook.AddFirst(ignoredElement);

            var r = XmlReader.Create(new StringReader(xml.ToString()));
            var results = TestParser.Parse(r);
            var library = results.Item1;

            Assert.AreEqual(firstBook.Attribute("title").Value, library.Categories[0].Books[0].Title);

            AssertErrorCount(0, results.Item2);
        }

        [Test]
        public void UnexpectedText_CanBeIgnoredAndParsingContinues()
        {
            var xml = BuildValidXml();
            var firstBook = xml.Element("category")
                                .Element("book");

            firstBook.AddFirst("Random Text");

            var r = XmlReader.Create(new StringReader(xml.ToString()));
            var results = TestParser.Parse(r);
            var library = results.Item1;

            Assert.AreEqual(firstBook.Attribute("title").Value, library.Categories[0].Books[0].Title);

            AssertErrorCount(0, results.Item2);
        }

        [Test]
        public void UnexpectedAttribute_CanBeIgnoredAndParsingContinues()
        {
            var xml = BuildValidXml();
            var firstCategory = xml.Element("category");

            firstCategory.SetAttributeValue("misc", "Some Misc Attribute");

            var r = XmlReader.Create(new StringReader(xml.ToString()));
            var results = TestParser.Parse(r);
            var library = results.Item1;

            Assert.AreEqual(firstCategory.Element("name").Value, library.Categories[0].Name);

            AssertErrorCount(0, results.Item2);
        }

        [Test]
        public void ExceptionLogsErrorAndContinuesParsing()
        {
            var xml = BuildValidXml();
            xml.Attribute("location").SetValue("throw");

            var r = XmlReader.Create(new StringReader(xml.ToString()));
            var results = TestParser.Parse(r);
            var library = results.Item1;

            Assert.IsTrue(library.Categories.Count >= 1);

            AssertErrorCount(1, results.Item2);
        }

        private void AssertErrorCount(int count, IEnumerable<DeclarativeXmlParser.ParseError> errors)
        {
            var list = errors.ToList();

            foreach(var error in list.WithIndex())
                Console.WriteLine("{0}) {1} ({2}, {3})", error.Index, error.Value.Message, error.Value.Token.LineNumber, error.Value.Token.LinePosition);

            if(list.Count != count)
                Assert.Fail("Number of expected errors was incorrect.");
        }

        private static XElement BuildValidXml()
        {
            return new XElement("library",
                new XAttribute("owner", "iSynaptic"),
                new XAttribute("location", "Office"),
                new XElement("category",
                    new XElement("name", "Testing"),
                    new XElement("book",
                        new XAttribute("title", "The Art of Unit Testing"),
                        new XAttribute("author", "Roy Osherove"),
                        new XAttribute("publishDate", "06/03/2009")
                    )
                ),
                new XElement("category",
                    new XElement("name", "Collective Intelligence"),
                    new XElement("book",
                        new XAttribute("title", "Collective Intelligence in Action"),
                        new XAttribute("author", "Satnam Alag"),
                        new XAttribute("publishDate", "10/17/2008"),
                        new XAttribute("isbn", "1933988312")
                    )
                ),
                new XElement("category",
                    new XElement("name", "Category Theory")
                )
            );
        }
    }
}
