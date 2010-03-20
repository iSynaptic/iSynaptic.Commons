using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using NUnit.Core;
using NUnit.Core.Extensibility;

using iSynaptic.Commons.Extensions;

namespace iSynaptic.Commons.Testing.NUnit.Addins
{
    public class BehavioralAttributeTestDecorator : ITestDecorator
    {
        private class UnderlyingTestMethodDecorator : TestDecorator
        {
            public UnderlyingTestMethodDecorator(TestMethod test) : base(test)
            {
            }

            public override void doRun(TestResult testResult)
            {
                Type declaringType = Method.DeclaringType;

                var behaviors = declaringType.GetAttributesOfType<ITestBehavior>()
                    .Union(Method.GetAttributesOfType<ITestBehavior>())
                    .ToArray();

                foreach (var behavior in behaviors)
                    behavior.BeforeTest(Fixture);

                base.doRun(testResult);

                foreach (var behavior in behaviors)
                    behavior.AfterTest(Fixture);
            }
        }

        public Test Decorate(Test test, MemberInfo member)
        {
            if(test is TestMethod)
                return new UnderlyingTestMethodDecorator(test as TestMethod);

            return test;
        }
    }
}
