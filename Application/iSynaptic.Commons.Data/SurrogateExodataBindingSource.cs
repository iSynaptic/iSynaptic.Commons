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
using System.Text;

using iSynaptic.Commons.Collections.Generic;
using iSynaptic.Commons.Linq;

namespace iSynaptic.Commons.Data
{
    public class SurrogateExodataBindingSource : IExodataBindingSource
    {
        private readonly object _syncLock = new object();
        private volatile KeyValuePair<Type, IExodataBindingSource>[] _Surrogates;

        private static IEnumerable<KeyValuePair<Type, IExodataBindingSource>> ScanForSurrogates()
        {
            Type bindingSourceType = typeof(IExodataBindingSource);

            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(x => x.IsDynamic != true)
                .SelectMany(x => x.GetExportedTypes())
                .Where(x => !x.IsAbstract && !x.IsGenericTypeDefinition && bindingSourceType.IsAssignableFrom(x))
                .Select(x => new { Type = x, BaseType = GetExodataSurrgateBaseClass(x) })
                .Where(x => x.BaseType.HasValue)
                .Select(x => KeyValuePair.Create(x.BaseType.Value.GetGenericArguments()[0], InstantiateSurrogate(x.Type)));
        }

        private static Maybe<Type> GetExodataSurrgateBaseClass(Type type)
        {
            return type.Recurse(x => x.BaseType)
                .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof (ExodataSurrogate<>))
                .TryFirst();
        }

        private static IExodataBindingSource InstantiateSurrogate(Type type)
        {
            object surrogate = Ioc.Resolve(type);
            
            if(surrogate == null && type.GetConstructors().Any(x => x.GetParameters().Length == 0))
                surrogate = Activator.CreateInstance(type);

            if (surrogate == null)
                throw new InvalidOperationException(string.Format("Unable to instantiate Exodata surrogate '{0}'. The surrogate is not registered with Ioc and has no public parameterless constructor.", type.FullName));

            return (IExodataBindingSource)surrogate;
        }

        public IEnumerable<IExodataBinding> GetBindingsFor<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request)
        {
            Type subjectType = typeof(TSubject);

            if(_Surrogates == null)
            {
                lock(_syncLock)
                {
                    if (_Surrogates == null)
                        _Surrogates = ScanForSurrogates().ToArray();
                }
            }

            return _Surrogates
                .Where(x => x.Key.IsAssignableFrom(subjectType))
                .Select(x => x.Value)
                .SelectMany(x => x.GetBindingsFor(request));
        }
    }
}
