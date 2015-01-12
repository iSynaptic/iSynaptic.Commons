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
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace iSynaptic.Commons.Threading.Tasks
{
    internal class TaskWrapper<TResult> : ITask<TResult>
    {
        private class TaskAwaiterWrapper : ITaskAwaiter<TResult>
        {
            private readonly TaskAwaiter<TResult> _underlying;

            public TaskAwaiterWrapper(TaskAwaiter<TResult> underlying)
            {
                _underlying = underlying;
            }

            public void OnCompleted(Action continuation) {  _underlying.OnCompleted(continuation); }
            public void UnsafeOnCompleted(Action continuation) { _underlying.UnsafeOnCompleted(continuation); }
            public TResult GetResult() { return _underlying.GetResult(); }
            public bool IsCompleted { get { return _underlying.IsCompleted; } }
        }

        private readonly Task<TResult> _underlying;

        public TaskWrapper(Task<TResult> underlying)
        {
            _underlying = underlying;
        }

        public TResult Result { get { return _underlying.Result; } }
        public ITaskAwaiter<TResult> GetAwaiter()
        {
            return new TaskAwaiterWrapper(_underlying.GetAwaiter());
        }
    }
}