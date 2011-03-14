using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons.Collections.Generic
{
    [TestFixture]
    public class MultiMapTests
    {
        [Test]
        public void Add_AddsItemToCorrectCollection()
        {
            var map = new MultiMap<int, string>();
            map.Add(1, "1");
            map.Add(42, "42");

            Assert.IsTrue(map[1].SequenceEqual(new[] { "1"} ));
            Assert.IsTrue(map[42].SequenceEqual(new[] { "42" }));
        }

        [Test]
        public void AddRange_AddsItemsToCorrectCollection()
        {
            var map = new MultiMap<int, string>();
            map.AddRange(1, Enumerable.Range(1, 10).Select(x => x.ToString()));
            map.AddRange(42, Enumerable.Repeat(42, 42).Select(x => x.ToString()));

            Assert.IsTrue(map[1].SequenceEqual(Enumerable.Range(1, 10).Select(x => x.ToString())));
            Assert.IsTrue(map[42].SequenceEqual(Enumerable.Repeat(42, 42).Select(x => x.ToString())));
        }

        [Test]
        public void Remove_RemovesItemFromTheCorrectCollection()
        {
            var map = new MultiMap<int, string>();
            map.AddRange(1, new []{"1", "2", "3"});
            map.AddRange(42, new [] { "42", "42", "42"});

            map.Remove(1, "2");

            Assert.IsTrue(map[1].SequenceEqual(new []{"1", "3"}));
            Assert.IsTrue(map[42].SequenceEqual(new []{"42", "42", "42"}));
        }

        [Test]
        public void Indexer_ReturnsCorrectCollection()
        {
            var map = new MultiMap<int, string>();
            map.AddRange(1, new[] { "1", "2", "3" });
            map.AddRange(42, new[] { "42", "42", "42" });

            var colFor1 = map[1];
            var colFor42 = map[42];

            Assert.IsTrue(colFor1.SequenceEqual(new[] { "1", "2", "3" }));
            Assert.IsTrue(colFor42.SequenceEqual(new[] { "42", "42", "42" }));
        }
    }
}
