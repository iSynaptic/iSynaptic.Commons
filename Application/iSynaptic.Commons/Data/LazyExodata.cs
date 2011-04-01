using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class LazyExodata<TExodata> : Lazy<TExodata>
    {
        public LazyExodata()
            : base(ExodataDeclaration.Get<TExodata>)
        {
        }

        public LazyExodata(IExodataDeclaration<TExodata> declaration)
            : base(() => declaration.Resolve(Maybe<object>.NoValue, Maybe<object>.NoValue, null))
        {
            Guard.NotNull(declaration, "declaration");
        }

        public static implicit operator TExodata(LazyExodata<TExodata> value)
        {
            return value.Value;
        }
    }

    public class LazyExodata<TExodata, TSubject> : Lazy<TExodata>
    {
        public LazyExodata(IExodataDeclaration<TExodata> declaration)
            : base(() => declaration.Resolve(Maybe<object>.NoValue, Maybe<TSubject>.NoValue, null))
        {
        }

        public LazyExodata(IExodataDeclaration<TExodata> declaration, TSubject subject)
            : base(() => declaration.Resolve(Maybe<object>.NoValue, Maybe.Value(subject), null))
        {
            Guard.NotNull(declaration, "declaration");
        }

        public LazyExodata(IExodataDeclaration<TExodata> declaration, MemberInfo member)
            : base(() => declaration.Resolve(Maybe<object>.NoValue, Maybe<TSubject>.NoValue, member))
        {
            Guard.NotNull(declaration, "declaration");
        }

        public LazyExodata(IExodataDeclaration<TExodata> declaration, TSubject subject, MemberInfo member)
            : base(() => declaration.Resolve(Maybe<object>.NoValue, Maybe.Value(subject), member))
        {
            Guard.NotNull(declaration, "declaration");
        }

        public static implicit operator TExodata(LazyExodata<TExodata, TSubject> value)
        {
            return value.Value;
        }
    }
}
