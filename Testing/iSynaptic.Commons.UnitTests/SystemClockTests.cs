// The MIT License
// 
// Copyright (c) 2012 Jordan E. Terrell
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
using System.Reflection;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class SystemClockTests
    {
        [Test]
        public void AbsoluteDefaultStrategy_ReturnsUtcNow()
        {
            var systemClockNow = SystemClock.UtcNow;
            Assert.AreEqual(DateTimeKind.Utc, systemClockNow.Kind);
        }

        [Test]
        public void ChangingDefaultStrategy_TakesAffect()
        {
            try
            {
                DateTime now = DateTime.UtcNow;

                SystemClock.DefaultDateTimeStrategy = () => now;
                var systemClockNow = SystemClock.UtcNow;

                Assert.AreEqual(systemClockNow.ToString(), now.ToString());
            }
            finally
            {
                SystemClock.DefaultDateTimeStrategy = null;
            }
        }

        [Test]
        public void SettingDefaultDateTimeStrategyToNull_ContinuesToUseAbsoluteDefault()
        {
            SystemClock.DefaultDateTimeStrategy = null;

            var systemClockNow = SystemClock.UtcNow;
            Assert.AreEqual(systemClockNow.ToUniversalTime().ToString(), systemClockNow.ToString());
        }

        [Test]
        public void WhileInFixedBlock_SystemTimeNowIsAlwaysTheSame()
        {
            var fixedDateTime = DateTime.UtcNow.Subtract(TimeSpan.FromDays(42));

            using(SystemClock.Fixed(fixedDateTime))
            {
                Assert.AreEqual(fixedDateTime, SystemClock.UtcNow);
            }

            Assert.AreNotEqual(fixedDateTime, SystemClock.UtcNow);
        }

        [Test]
        public void WhileInUsingBlock_SystemTimeNowUsesStrategy()
        {
            var startDateTime = DateTime.UtcNow.Subtract(TimeSpan.FromDays(42));
            var dateTime = startDateTime;

            using (SystemClock.Using(() => { dateTime = dateTime.AddMinutes(10); return dateTime; }))
            {
                for(int i = 1; i < 10; i++)
                    Assert.AreEqual(startDateTime.AddMinutes(10 * i), SystemClock.UtcNow);
            }

            Assert.IsTrue(SystemClock.UtcNow > startDateTime.AddDays(41));
        }

        [Test]
        public void CallingFixedOrUsing_AfterPreventClockAlterations_ThrowsException()
        {
            try
            {
                SystemClock.PreventClockAlterations();

                Assert.Throws<InvalidOperationException>(() => SystemClock.Fixed(DateTime.Now));
                Assert.Throws<InvalidOperationException>(() => SystemClock.Using(() =>DateTime.Now));
            }
            finally
            {
                RestoreClockAlterations();
            }
        }

        [Test]
        public void SettingDefaultDateTimeStrategy_AfterPreventClockAlterations_ThrowsException()
        {
            try
            {
                SystemClock.PreventClockAlterations();

                Assert.Throws<InvalidOperationException>(() => { SystemClock.DefaultDateTimeStrategy = () => DateTime.UtcNow; });
            }
            finally
            {
                RestoreClockAlterations();
            }
        }

        private static void RestoreClockAlterations()
        {
            typeof (SystemClock)
                .GetField("_PreventClockAlterations", BindingFlags.NonPublic | BindingFlags.Static)
                .SetValue(null, false);
        }
    }
}
