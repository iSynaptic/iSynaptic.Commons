using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons.UnitTests
{
    public static class UnitTestingExtensions
    {
        public static TestDelegate AsTestDelegate<T>(this Func<T> func)
        {
            if (func == null)
                throw new ArgumentNullException("func");

            return () => func();
        }

        public static TestDelegate AsTestDelegate(this Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return () => action();
        }
    }
}
