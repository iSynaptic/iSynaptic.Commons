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
using iSynaptic.Commons.Collections.Generic;
using iSynaptic.Commons.Reflection;

namespace iSynaptic.Commons.Data
{
    public class AttributeExodataBindingSource : IExodataBindingSource
    {
        public IEnumerable<IExodataBinding> GetBindingsFor<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request)
        {
            Guard.NotNull(request, "request");

            ICustomAttributeProvider provider = request.Member ?? 
                request.Subject
                .Select(x => x.GetType())
                .ValueOrDefault(typeof(TSubject));

            return GetBindings(provider)
                .Select(x => ExodataBinding.Create<TExodata, object, TSubject>(null, this, Maybe<ISymbol>.NoValue, Maybe<object>.NoValue, request.Subject, request.Member != null ? new[]{request.Member} : null, null, r => x.TryResolve(r).AsMaybe()));
        }

        private static IEnumerable<IExodataBinding> GetBindings(ICustomAttributeProvider provider)
        {
            Guard.NotNull(provider, "provider");

            return provider.GetCustomAttributes(true)
                .Select(x => new
                {
                    Attribute = x,
                    Interfaces = x.GetType().GetInterfaces().Where(y => y.DoesImplementType(typeof(IExodataAttribute<>))).ToArray()
                })
                .Where(x => x.Interfaces.Length > 0)
                .SelectMany(x => x.Interfaces.Select(y => WrapAttribute(x.Attribute, y.GetGenericArguments()[0])));
        }

        private static IExodataBinding WrapAttribute(object attribute, Type exodataType)
        {
            return (IExodataBinding)Activator.CreateInstance(typeof(AttributeBinding<>).MakeGenericType(exodataType), attribute);
        }

        private class AttributeBinding<TExodata> : IExodataBinding
        {
            private readonly IExodataAttribute<TExodata> _Attribute;

            public AttributeBinding(IExodataAttribute<TExodata> attribute)
            {
                _Attribute = attribute;
            }

            public Maybe<TRequestExodata> TryResolve<TRequestExodata, TContext, TSubject>(IExodataRequest<TRequestExodata, TContext, TSubject> request)
            {
                return ExodataRequest<TExodata, TContext, TSubject>
                    .TryToAdapt(request)
                    .SelectMaybe(r => _Attribute.TryResolve(r))
                    .Cast<TRequestExodata>();
            }
        }
    }
}
