using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class ExpressionExtensionsTests
    {
        [Test]
        public void ToCovariant_WhenCompiled_ExecutesCorrectly()
        {
            Expression<Func<string, string>> expr = x => x.ToUpper();
            Expression<Func<string, object>> covariantExpr = expr.ToCovariant<string, string, object>();

            Func<string, object> covariantFunc = covariantExpr.Compile();
            Assert.AreEqual("HELLO", covariantFunc("hello"));
        }
    }
}
