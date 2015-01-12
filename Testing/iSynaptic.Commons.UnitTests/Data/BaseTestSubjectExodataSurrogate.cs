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

namespace iSynaptic.Commons.Data
{
    public abstract class BaseTestSubjectExodataSurrogate : ExodataSurrogate<TestSubject>
    {
        public static readonly TestSubject Subject = new TestSubject();
        public static bool ShouldYieldInstanceExodata { get; set; }
    }

    public class TestSubjectExodataSurrogateOne : BaseTestSubjectExodataSurrogate
    {
        public TestSubjectExodataSurrogateOne()
        {
            Bind(StringExodata.MaxLength)
                .For(x => x.MiddleName)
                .To(74088);

            Bind(CommonExodata.Description)
                .When(r => ShouldYieldInstanceExodata)
                .To("Surrogate Description");

            Bind(CommonExodata.Description)
                .For(Subject)
                .When(r => ShouldYieldInstanceExodata)
                .To(r => "Special Instance Description");

        }
    }

    public class TestSubjectExodataSurrogateTwo : BaseTestSubjectExodataSurrogate
    {
        public TestSubjectExodataSurrogateTwo()
        {
            Bind(CommonExodata.Description)
                .For(Subject, x => x.FirstName)
                .When(r => ShouldYieldInstanceExodata)
                .To("Special Member Description");

            Bind(CommonExodata.Description)
                .Given<string>()
                .For(x => x.FirstName)
                .When(r => ShouldYieldInstanceExodata)
                .To("Contextual Member Description");

            Bind(CommonExodata.Description)
                .Given("Context")
                .For(x => x.FirstName)
                .When(r => ShouldYieldInstanceExodata)
                .To("Specific Contextual Member Description");
        }
    }
}