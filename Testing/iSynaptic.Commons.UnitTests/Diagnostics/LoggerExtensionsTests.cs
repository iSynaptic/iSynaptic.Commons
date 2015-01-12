// The MIT License
// 
// Copyright (c) 2012-2015 Jordan E. Terrell
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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
