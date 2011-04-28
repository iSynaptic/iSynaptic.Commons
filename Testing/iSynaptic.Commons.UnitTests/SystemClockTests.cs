using System;
using System.Collections.Generic;
using System.Linq;
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
            var systemClockNow = SystemClock.Now;
            Assert.AreEqual(systemClockNow.ToUniversalTime().ToString(), systemClockNow.ToString());
        }

        [Test]
        public void ChangingDefaultStrategy_TakesAffect()
        {
            try
            {
                SystemClock.DefaultDateTimeStrategy = () => DateTime.Now;
                var systemClockNow = SystemClock.Now;

                Assert.AreNotEqual(systemClockNow.ToString(), systemClockNow.ToUniversalTime().ToString());
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

            var systemClockNow = SystemClock.Now;
            Assert.AreEqual(systemClockNow.ToUniversalTime().ToString(), systemClockNow.ToString());
        }

        [Test]
        public void WhileInFixedBlock_SystemTimeNowIsAlwaysTheSame()
        {
            var fixedDateTime = DateTime.Now.Subtract(TimeSpan.FromDays(42));

            using(SystemClock.Fixed(fixedDateTime))
            {
                Assert.AreEqual(fixedDateTime, SystemClock.Now);
            }

            Assert.AreNotEqual(fixedDateTime, SystemClock.Now);
        }

        [Test]
        public void WhileInUsingBlock_SystemTimeNowUsesStrategy()
        {
            var startDateTime = DateTime.Now.Subtract(TimeSpan.FromDays(42));
            var dateTime = startDateTime;

            using (SystemClock.Using(() => { dateTime = dateTime.AddMinutes(10); return dateTime; }))
            {
                for(int i = 1; i < 10; i++)
                    Assert.AreEqual(startDateTime.AddMinutes(10 * i), SystemClock.Now);
            }

            Assert.IsTrue(SystemClock.Now > startDateTime.AddDays(41));
        }
    }
}
