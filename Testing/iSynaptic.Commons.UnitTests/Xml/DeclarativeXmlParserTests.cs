using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
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
            private TestParser() {}

            public static Tuple<Library, IEnumerable<ParseError>> Library(XmlReader reader)
            {
                var library = new Library();

                var errors = Upon(reader, b =>
                {
                    b.Attribute<string>("owner", x => library.Owner = x);
                    b.Attribute<string>("location", x => library.Location = x);
                    
                    b.Attribute<DateTime>("lastUpdated", x => library.LastUpdated = x)
                        .ZeroOrOne();

                    b.Element("category", x => library.Categories.Add(BookCategory()))
                        .OneOrMore();
                });

                return Tuple.Create(library, errors);
            }

            private static BookCategory BookCategory()
            {
                var category = new BookCategory();

                Upon(b =>
                {
                    b.ContentElement<string>("name", x => category.Name = x);
                    b.Element("book", x => category.Books.Add(Book()))
                        .OneOrMore();
                });

                return category;
            }

            private static Book Book()
            {
                var book = new Book();

                Upon(b =>
                {
                    b.Attribute<string>("title", x => book.Title = x);
                    b.Attribute<string>("author", x => book.Author = x);
                    b.Attribute<DateTime>("publishDate", x => book.PublishDate = x);

                    b.Attribute<string>("isbn", x => book.ISBN = x)
                        .ZeroOrOne();

                    b.Attribute<bool>("requiredReading", x => book.RequiredReading = x)
                        .ZeroOrOne();
                });

                return book;
            }
        }

        [Test]
        public void TopLevel_RequiredAttributes_ParseCorrectly()
        {
            var xml = BuildValidXml();

            var r = XmlReader.Create(new StringReader(xml.ToString()));

            var results = TestParser.Library(r);
            var library = results.Item1;

            Assert.AreEqual("iSynaptic", library.Owner);
            Assert.AreEqual("Office", library.Location);
            Assert.AreEqual(0, results.Item2.Count());
        }

        [Test]
        public void FirstLevel_RequiredElement_ParsesCorrectly()
        {
            var xml = BuildValidXml();

            var r = XmlReader.Create(new StringReader(xml.ToString()));

            var results = TestParser.Library(r);
            var library = results.Item1;

            Assert.IsTrue(library.Categories.Count >= 1);
            Assert.IsTrue(library.Categories.Any(x => x.Name == "Testing"));

            Assert.AreEqual(0, results.Item2.Count());
        }

        [Test]
        public void FirstLevel_OptionalAttribute_ParsesCorrectly()
        {
            var lastUpdated = DateTime.Parse("01/28/2011");

            var xml = BuildValidXml();
            xml.Add(new XAttribute("lastUpdated", lastUpdated.ToString()));

            var r = XmlReader.Create(new StringReader(xml.ToString()));

            var results = TestParser.Library(r);
            var library = results.Item1;

            Assert.AreEqual(lastUpdated, library.LastUpdated.Value);

            Assert.AreEqual(0, results.Item2.Count());
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
                        new XAttribute("publishDate", "06/03/2009"))),
                new XElement("category",
                    new XElement("name", "Collective Intelligence"),
                    new XElement("book",
                        new XAttribute("title", "Collective Intelligence in Action"),
                        new XAttribute("author", "Satnam Alag"),
                        new XAttribute("publishDate", "10/17/2008"),
                        new XAttribute("isbn", "1933988312"))));
        }
    }
}
