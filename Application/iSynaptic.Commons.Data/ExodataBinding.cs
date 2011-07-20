﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using iSynaptic.Commons.Collections.Generic;

namespace iSynaptic.Commons.Data
{
    public static class ExodataBinding
    {
        public static IExodataBinding Create<TExodata, TContext, TSubject>(string name, IExodataBindingSource source, Maybe<ISymbol> symbol, Maybe<TContext> context, Maybe<TSubject> subject, MemberInfo[] members, Func<IExodataRequest<TExodata, TContext, TSubject>, bool> predicate, Func<IExodataRequest<TExodata, TContext, TSubject>, Maybe<TExodata>> valueFactory)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(valueFactory, "valueFactory");

            return new TypedBinding<TExodata, TContext, TSubject>()
            {
                Name = name,
                Source = source,
                Symbol = symbol,
                Context = context,
                Subject = subject,
                Members = members,
                Predicate = predicate,
                ValueFactory = valueFactory,
            };
        }

        private class TypedBinding<TExodata, TContext, TSubject> : IExodataBinding, IExodataBindingDetails
        {
            public Maybe<TRequestExodata> TryResolve<TRequestExodata, TRequestContext, TRequestSubject>(IExodataRequest<TRequestExodata, TRequestContext, TRequestSubject> request)
            {
                return ExodataRequest<TExodata, TContext, TSubject>
                    .TryToAdapt(request)
                    .Where(Matches)
                    .SelectMaybe(ValueFactory)
                    .Cast<TRequestExodata>();
            }

            private bool Matches(IExodataRequest<TExodata, TContext, TSubject> request)
            {
                Guard.NotNull(request, "request");

                if (Symbol.HasValue && Symbol.Value != request.Symbol)
                    return false;

                if (request.Member == null && Members != null && Members.Length > 0)
                    return false;

                if (request.Member != null && (Members == null || Members.Contains(request.Member) != true))
                    return false;

                if (Context.HasValue && !request.Context.HasValue)
                    return false;

                if (Context.HasValue && !EqualityComparer<TContext>.Default.Equals(request.Context.Value, Context.Value))
                    return false;

                if (Subject.HasValue && !request.Subject.HasValue)
                    return false;

                if (Subject.HasValue && !EqualityComparer<TSubject>.Default.Equals(request.Subject.Value, Subject.Value))
                    return false;

                if (Predicate != null)
                    return Predicate(request);

                return true;
            }
            public string Name { get; set; }
            public IExodataBindingSource Source { get; set; }

            public Maybe<ISymbol> Symbol { get; set; }
            public Maybe<TContext> Context { get; set; }
            public Maybe<TSubject> Subject { get; set; }
            public MemberInfo[] Members { get; set; }

            public Func<IExodataRequest<TExodata, TContext, TSubject>, bool> Predicate { get; set; }
            public Func<IExodataRequest<TExodata, TContext, TSubject>, Maybe<TExodata>> ValueFactory { get; set; }

            public bool BoundToSymbolInstance { get { return Symbol.HasValue; } }
            public bool BoundToContextInstance { get { return Context.HasValue; } }
            public bool BoundToSubjectInstance { get { return Subject.HasValue; } }

            public Type ExodataType { get { return typeof(TExodata); } }
            public Type ContextType { get { return typeof(TContext); } }
            public Type SubjectType { get { return typeof(TSubject); } }
        }
    }
}
