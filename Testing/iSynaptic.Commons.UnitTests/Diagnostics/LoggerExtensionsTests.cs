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
            logger.Debug("Message", "Context");

            logger.Info("Message");
            logger.Info("Message", "Context");

            logger.Warn("Message");
            logger.Warn("Message", "Context");

            logger.Error("Message");
            logger.Error("Message", "Context");

            logger.Fatal("Message");
            logger.Fatal("Message", "Context");
        }

        [Test]
        public void LogsInfoCorrectly()
        {
            var logger = MockRepository.GenerateStub<ILogger>();

            logger.Info("Message");
            logger.Info("Message", "Context");

            logger.AssertWasCalled(x => x.Log(LogLevel.Info, "Message", null));
            logger.AssertWasCalled(x => x.Log(LogLevel.Info, "Message", "Context"));
        }
    }
}
