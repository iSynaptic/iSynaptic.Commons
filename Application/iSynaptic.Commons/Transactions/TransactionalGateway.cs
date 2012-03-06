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
using iSynaptic.Commons.Runtime.Serialization;

namespace iSynaptic.Commons.Transactions
{
    public class TransactionalGateway<T> : TransactionalBase<T> where T : class
    {
        public TransactionalGateway(T value) : base(value)
        {
        }

        protected override void SetCurrentValue(T value)
        {
            Guard.NotNull(value, "value");

            var currentValue = GetCurrentValue();
            if(currentValue.Value != null)
            {
                var newValue = value.CloneTo(currentValue.Value);
                base.SetCurrentValue(newValue);
            }
            else
                base.SetCurrentValue(value);
        }

        protected override void SetTransactionValue(T value)
        {
            Guard.NotNull(value, "value");

            var currentValue = GetTransactionValue();
            if (currentValue.HasValue)
            {
                var newValue = value.CloneTo(currentValue.Value.Value);
                base.SetTransactionValue(newValue);
            }
            else
                base.SetTransactionValue(value);
        }
    }
}
