using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public static class Specification
    {
        #region Nested Members

        private sealed class NotSpecification<T> : Specification<T>
        {
            private ISpecification<T> _InnerSpecification = null;
            public NotSpecification(ISpecification<T> specification)
            {
                _InnerSpecification = specification;
            }

            public override bool IsSatisfiedBy(T candidate)
            {
                return !(_InnerSpecification.IsSatisfiedBy(candidate));
            }

            internal ISpecification<T> InnerSpecification
            {
                get { return _InnerSpecification; }
            }
        }

        private sealed class LogicalSpecification<T> : Specification<T>
        {
            private ISpecification<T> _Left = null;
            private ISpecification<T> _Right = null;
            private Func<bool, bool, bool> _Operation = null;

            public LogicalSpecification(ISpecification<T> left, ISpecification<T> right, Func<bool, bool, bool> operation)
            {
                _Left = left;
                _Right = right;
                _Operation = operation;
            }

            public override bool IsSatisfiedBy(T candidate)
            {
                bool leftResult = _Left.IsSatisfiedBy(candidate);
                bool rightResult = _Right.IsSatisfiedBy(candidate);

                return _Operation(leftResult, rightResult);
            }
        }

        private sealed class PredicateSpecification<T> : Specification<T>
        {
            private Predicate<T> _Predicate = null;

            public PredicateSpecification(Predicate<T> predicate)
            {
                _Predicate = predicate;
            }

            public override bool IsSatisfiedBy(T candidate)
            {
                return Predicate(candidate);
            }

            internal Predicate<T> Predicate { get { return _Predicate; } }
        }

        private sealed class FuncSpecification<T> : Specification<T>
        {
            private Func<T, bool> _Func = null;

            public FuncSpecification(Func<T, bool> func)
            {
                _Func = func;
            }

            public override bool IsSatisfiedBy(T candidate)
            {
                return Func(candidate);
            }

            internal Func<T, bool> Func { get { return _Func; } }
        }

        #endregion

        public static bool IsSatisfiedBy<T>(this ISpecification<T> specification, params T[] candidates)
        {
            return candidates.All(specification.IsSatisfiedBy);
        }

        public static bool IsSatisfiedBy<T>(this ISpecification<T> specification, IEnumerable<T> candidates)
        {
            return candidates.All(specification.IsSatisfiedBy);
        }

        #region Operator Implementation

        public static ISpecification<T> Not<T>(this ISpecification<T> specification)
        {
            if (specification == null)
                throw new ArgumentNullException("specification");

            if (specification.GetType() == typeof(NotSpecification<T>))
                return ((NotSpecification<T>)specification).InnerSpecification;

            return new NotSpecification<T>(specification);
        }

        public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            if (left == null)
                throw new ArgumentNullException("left");

            if (right == null)
                throw new ArgumentNullException("right");

            return new LogicalSpecification<T>(left,
                right,
                (l, r) => l && r);
        }

        public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            if (left == null)
                throw new ArgumentNullException("left");

            if (right == null)
                throw new ArgumentNullException("right");

            return new LogicalSpecification<T>(left,
                right,
                (l, r) => l || r);
        }

        public static ISpecification<T> XOr<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            if (left == null)
                throw new ArgumentNullException("left");

            if (right == null)
                throw new ArgumentNullException("right");

            return new LogicalSpecification<T>(left,
                right,
                (l, r) => l ^ r);
        }

        #endregion

        #region Conversions

        public static Predicate<T> ToPredicate<T>(this ISpecification<T> specification)
        {
            if (specification == null)
                return null;

            if (specification is PredicateSpecification<T>)
            {
                PredicateSpecification<T> predicateSpecification = specification as PredicateSpecification<T>;
                return predicateSpecification.Predicate;
            }
            else
                return specification.IsSatisfiedBy;
        }

        public static Func<T, bool> ToFunc<T>(this ISpecification<T> specification)
        {
            if (specification == null)
                return null;

            if (specification is FuncSpecification<T>)
            {
                FuncSpecification<T> funcSpecification = specification as FuncSpecification<T>;
                return funcSpecification.Func;
            }
            else
                return specification.IsSatisfiedBy;
        }

        public static ISpecification<T> ToSpecification<T>(this Predicate<T> predicate)
        {
            if (predicate == null)
                return null;

            if (predicate.Target != null && predicate.Target is ISpecification<T>)
                return predicate.Target as ISpecification<T>;
            else
                return new PredicateSpecification<T>(predicate);
        }

        public static ISpecification<T> ToSpecification<T>(this Func<T, bool> func)
        {
            if (func == null)
                return null;

            if (func.Target != null && func.Target is ISpecification<T>)
                return func.Target as ISpecification<T>;
            else
                return new FuncSpecification<T>(func);
        }

        #endregion
    }

    public abstract class Specification<T> : ISpecification<T>
    {
        public abstract bool IsSatisfiedBy(T candidate);

        #region Implicit Conversions

        public static implicit operator Predicate<T>(Specification<T> specification)
        {
            return specification.ToPredicate();
        }

        public static implicit operator Func<T, bool>(Specification<T> specification)
        {
            return specification.ToFunc();
        }

        #endregion

        #region Operator Overloads

        public static ISpecification<T> operator &(Specification<T> left, Specification<T> right)
        {
            return left.And(right);
        }

        public static ISpecification<T> operator |(Specification<T> left, Specification<T> right)
        {
            return left.Or(right);
        }

        public static ISpecification<T> operator ^(Specification<T> left, Specification<T> right)
        {
            return left.XOr(right);
        }

        public static ISpecification<T> operator !(Specification<T> specification)
        {
            return specification.Not();
        }

        public static bool operator true(Specification<T> specification)
        {
            return false;
        }

        public static bool operator false(Specification<T> specification)
        {
            return false;
        }

        #endregion
    }
}
