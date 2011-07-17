using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using iSynaptic.Commons.Collections.Generic;

namespace iSynaptic.Commons.Data
{

    public static class ExodataBinding
    {
        public static IExodataBinding Create<TExodata, TContext, TSubject>(IExodataBindingSource source, Func<IExodataRequest<TExodata, TContext, TSubject>, bool> predicate, Func<IExodataRequest<TExodata, TContext, TSubject>, Maybe<TExodata>> valueFactory, string name, bool boundToContextInstance = false, bool boundToSubjectInstance = false)
        {
            Guard.NotNull(predicate, "predicate");
            Guard.NotNull(valueFactory, "valueFactory");
            Guard.NotNull(source, "source");

            return new TypedBinding<TExodata, TContext, TSubject>(predicate, valueFactory, source, name)
            {
                ContextType = typeof(TContext),
                SubjectType = typeof(TSubject),
                BoundToContextInstance = boundToContextInstance,
                BoundToSubjectInstance = boundToSubjectInstance
            };
        }

        private class TypedBinding<TExodata, TContext, TSubject> : IExodataBinding, IExodataBindingDetails
        {
            public TypedBinding(Func<IExodataRequest<TExodata, TContext, TSubject>, bool> predicate, Func<IExodataRequest<TExodata, TContext, TSubject>, Maybe<TExodata>> valueFactory, IExodataBindingSource source, string name)
            {
                Guard.NotNull(predicate, "predicate");
                Guard.NotNull(valueFactory, "valueFactory");
                Guard.NotNull(source, "source");

                Predicate = predicate;
                ValueFactory = valueFactory;
                Source = source;
                Name = name;
            }

            public Maybe<TRequestExodata> TryResolve<TRequestExodata, TRequestContext, TRequestSubject>(IExodataRequest<TRequestExodata, TRequestContext, TRequestSubject> request)
            {
                return TryGetRequest(request)
                    .Where(Predicate)
                    .SelectMaybe(ValueFactory)
                    .Cast<TRequestExodata>();
            }

            private static Maybe<IExodataRequest<TExodata, TContext, TSubject>> TryGetRequest<TRequestExodata, TRequestContext, TRequestSubject>(IExodataRequest<TRequestExodata, TRequestContext, TRequestSubject> request)
            {
                var typedRequest = request as IExodataRequest<TExodata, TContext, TSubject>;
                if(typedRequest != null)
                    return typedRequest.ToMaybe();

                if (typeof(TRequestExodata).IsAssignableFrom(typeof(TExodata)) &&
                    typeof(TContext).IsAssignableFrom(typeof(TRequestContext)) &&
                    typeof(TSubject).IsAssignableFrom(typeof(TRequestSubject)))
                {
                    return new RequestAdapter<TRequestExodata, TRequestContext, TRequestSubject>(request)
                        .ToMaybe<IExodataRequest<TExodata, TContext, TSubject>>();
                }

                return Maybe<IExodataRequest<TExodata, TContext, TSubject>>.NoValue;
            }

            public string Name { get; set; }

            public Type ContextType { get; set; }
            public Type SubjectType { get; set; }
            public IExodataBindingSource Source { get; set; }

            public bool BoundToContextInstance { get; set; }
            public bool BoundToSubjectInstance { get; set; }

            public Func<IExodataRequest<TExodata, TContext, TSubject>, bool> Predicate { get; set; }
            public Func<IExodataRequest<TExodata, TContext, TSubject>, Maybe<TExodata>> ValueFactory { get; set; }

            private class RequestAdapter<TSourceExodata, TSourceContext, TSourceSubject>
                : IExodataRequest<TExodata, TContext, TSubject>
            {
                private readonly IExodataRequest<TSourceExodata, TSourceContext, TSourceSubject> _Request;

                public RequestAdapter(IExodataRequest<TSourceExodata, TSourceContext, TSourceSubject> request)
                {
                    _Request = request;
                }

                public ISymbol<TExodata> Symbol
                {
                    get { return _Request.Symbol as ISymbol<TExodata>; }
                }

                public Maybe<TContext> Context
                {
                    get { return _Request.Context.Cast<TContext>(); }
                }

                public Maybe<TSubject> Subject
                {
                    get { return _Request.Subject.Cast<TSubject>(); }
                }

                public MemberInfo Member
                {
                    get { return _Request.Member; }
                }
            }

        }
    }
}
