using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using iSynaptic.Commons.Xml;
using System.Xml;
using System.IO;

using iSynaptic.Commons.Extensions;

namespace iSynaptic.Commons.UnitTests.Xml
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
        [ExpectedException(typeof(ApplicationException), "Expected identifier.")]
        public void ExpectedIdentifier()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<?test-instruction 999=""headlines.css"" ?>
<test />";
            ProcessingInstructionParser.ParseAll(BuildReader(xml)).ForceEnumeration();
        }

        [Test]
        [ExpectedException(typeof(ApplicationException), "Expected '='.")]
        public void ExpectedAssignmentOperator()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<?test-instruction href-""headlines.css"" ?>
<test />";
            ProcessingInstructionParser.ParseAll(BuildReader(xml)).ForceEnumeration();
        }

        [Test]
        [ExpectedException(typeof(ApplicationException), "Expected '='.")]
        public void ExpectedAssignmentOperatorTwo()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<?test-instruction href""headlines.css"" ?>
<test />";
            ProcessingInstructionParser.ParseAll(BuildReader(xml)).ForceEnumeration();
        }

        [Test]
        [ExpectedException(typeof(ApplicationException), "Expected string.")]
        public void ExpectedString()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<?test-instruction href=bob ?>
<test />";
            ProcessingInstructionParser.ParseAll(BuildReader(xml)).ForceEnumeration();
        }
    }
}
