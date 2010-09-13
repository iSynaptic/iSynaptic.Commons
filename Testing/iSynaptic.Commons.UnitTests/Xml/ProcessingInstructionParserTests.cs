using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iSynaptic.Commons.Collections.Generic;
using NUnit.Framework;
using iSynaptic.Commons.Xml;
using System.Xml;
using System.IO;

namespace iSynaptic.Commons.Xml
{
    [TestFixture]
    public class ProcessingInstructionParserTests
    {
        private XmlReader BuildReader(string input)
        {
            StringReader innerReader = new StringReader(input);
            return XmlReader.Create(innerReader);
        }

        [Test]
        public void ParseAll()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<?test-instruction href=""headlines.css"" type=""text/css"" ?>
<?test-instruction2 href=""other.css"" type=""text/css"" ?>
<test />";

            var instructions = ProcessingInstructionParser.ParseAll(BuildReader(xml)).ToList();

            Assert.AreEqual(2, instructions.Count);

            Assert.AreEqual("test-instruction", instructions[0].Name);

            Assert.IsTrue(instructions[0].ContainsKey("href"));
            Assert.IsTrue(instructions[0].ContainsKey("type"));
            Assert.IsFalse(instructions[0].ContainsKey("name"));

            Assert.AreEqual("headlines.css", instructions[0].Attributes["href"]);
            Assert.AreEqual("text/css", instructions[0]["type"]);

            Assert.AreEqual("test-instruction2", instructions[1].Name);

            Assert.IsTrue(instructions[1].ContainsKey("href"));
            Assert.IsTrue(instructions[1].ContainsKey("type"));
            Assert.IsFalse(instructions[1].ContainsKey("name"));

            Assert.AreEqual("other.css", instructions[1].Attributes["href"]);
            Assert.AreEqual("text/css", instructions[1]["type"]);

        }

        [Test]
        public void ExpectedIdentifier()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<?test-instruction 999=""headlines.css"" ?>
<test />";

            Assert.Throws<ApplicationException>(() => ProcessingInstructionParser.ParseAll(BuildReader(xml)).ForceEnumeration(), "Expected identifier.");
        }

        [Test]
        public void ExpectedAssignmentOperator()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<?test-instruction href-""headlines.css"" ?>
<test />";

            Assert.Throws<ApplicationException>(() => ProcessingInstructionParser.ParseAll(BuildReader(xml)).ForceEnumeration(), "Expected '='.");
        }

        [Test]
        public void ExpectedAssignmentOperatorTwo()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<?test-instruction href""headlines.css"" ?>
<test />";
            Assert.Throws<ApplicationException>(() => ProcessingInstructionParser.ParseAll(BuildReader(xml)).ForceEnumeration(), "Expected '='.");
        }

        [Test]
        public void ExpectedString()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<?test-instruction href=bob ?>
<test />";
            Assert.Throws<ApplicationException>(() => ProcessingInstructionParser.ParseAll(BuildReader(xml)).ForceEnumeration(), "Expected string.");
        }

        [Test]
        public void ParseWithNullName()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ProcessingInstructionParser.Parse(null, @"version=""1.0"""));
        }

        [Test]
        public void ParseWithEmpryName()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ProcessingInstructionParser.Parse("", @"version=""1.0"""));
        }

        [Test]
        public void ParseWithNullAttributes()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ProcessingInstructionParser.Parse("xml", null));
        }

        [Test]
        public void ParseWithEmpryAttributes()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ProcessingInstructionParser.Parse("xml", ""));
        }
    }
}
