// The MIT License
// 
// Copyright (c) 2011 Jordan E. Terrell
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
using System.Threading;

namespace iSynaptic.Commons
{
    public static class ActionExtensions
    {
        public static IDisposable ToDisposable(this Action @this)
        {
            Guard.NotNull(@this, "@this");
            return ToDisposable(disposing => @this());
        }

        public static IDisposable ToDisposable(this Action<bool> @this)
        {
            return new ActionDisposer(Guard.NotNull(@this, "@this"));
        }

        private sealed class ActionDisposer : IDisposable
        {
            private readonly Action<bool> _Action = null;

            public ActionDisposer(Action<bool> action)
            {
                Guard.NotNull(action, "action");
                _Action = action;
            }

            ~ActionDisposer()
            {
                Dispose(false);
            }

            public void Dispose()
            {
                try
                {
                    Dispose(true);
                }
                finally
                {
                    GC.SuppressFinalize(this);
                }
            }

            private void Dispose(bool disposing)
            {
                _Action(disposing);
            }
        }
    }
}
