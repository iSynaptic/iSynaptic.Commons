using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

namespace iSynaptic.Commons.Diagnostics
{
    [TestFixture]
    public class LoggerExtensionsTests
    {
        [Test]
        public void NullLoggerIsNoOpWithNoException()
        {
            ILogger logger = null;

            logger.Debug("Message");
            logger.Debug<string>("Message", "Context");

            logger.Info("Message");
            logger.Info<string>("Message", "Context");

            logger.Warn("Message");
            logger.Warn<string>("Message", "Context");

            logger.Error("Message");
            logger.Error<string>("Message", "Context");

            logger.Fatal("Message");
            logger.Fatal<string>("Message", "Context");
        }

        [Test]
        public void LogsInfoCorrectly()
        {
            var logger = MockRepository.GenerateStub<ILogger>();

            logger.Info("Message");
            logger.Info("Message", "Context");

            logger.AssertWasCalled(x => x.Log<object>(LogLevel.Info, "Message", null));
            logger.AssertWasCalled(x => x.Log<string>(LogLevel.Info, "Message", "Context"));
        }
    }
}
