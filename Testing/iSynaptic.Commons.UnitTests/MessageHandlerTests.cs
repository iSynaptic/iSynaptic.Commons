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
using System.Threading.Tasks;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class MessageHandlerTests
    {
        private class TestMessageHandler : MessageHandler
        {
            public string LastString;

            public Task Handle(object obj)
            {
                return OnHandle(obj);
            }

            protected override bool ShouldHandle(object message)
            {
                return message.GetType() != typeof (object);
            }

            private void On(String text)
            {
                LastString = text;
            }
        }

        [Test]
        public void Handler_DispatchesMessagesCorrectly()
        {
            var handler = new TestMessageHandler();
            handler.Handle("Hello").Wait();

            Assert.AreEqual("Hello", handler.LastString);
        }
        
        [Test]
        public void Handler_ShouldThrowExceptionUponUnexpectedMessage()
        {
            var handler = new TestMessageHandler();
            Assert.Throws<AggregateException>(() => handler.Handle(new Symbol()).Wait());
        }

        [Test]
        public void Handler_CanIgnoreMessages()
        {
            var handler = new TestMessageHandler();
            handler.Handle(new object()).Wait();
        }
    }
}
