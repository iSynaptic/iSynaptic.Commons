using System;
using System.Collections.Generic;
using System.Linq;

namespace iSynaptic.Commons
{
    public static class Specification
    {
        #region Nested Members

        private sealed class NotSpecification<T> : Specification<T>
        {
            private readonly Specification<T> _InnerSpecification = null;
            public NotSpecification(Specification<T> specification)
            {
                Guard.NotNull(specification, "specification");
                _InnerSpecification = specification;
            }

            public override bool IsSatisfiedBy(T candidate)
            {
                return !(_InnerSpecification.IsSatisfiedBy(candidate));
            }

            internal Specification<T> InnerSpecification
            {
                get { return _InnerSpecification; }
            }
        }

        private sealed class LogicalSpecification<T> : Specification<T>
        {
            private readonly Specification<T> _Left = null;
            private readonly Specification<T> _Right = null;
            private readonly Func<bool, bool, bool> _Operation = null;

            public LogicalSpecification(Specification<T> left, Specification<T> right, Func<bool, bool, bool> operation)
            {
                Guard.NotNull(left, "left");
                Guard.NotNull(right, "right");
                Guard.NotNull(operation, "operation");

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
            private readonly Predicate<T> _Predicate = null;

            public PredicateSpecification(Predicate<T> predicate)
            {
                Guard.NotNull(predicate, "predicate");
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
            private readonly Func<T, bool> _Func = null;

            public FuncSpecification(Func<T, bool> func)
            {
                Guard.NotNull(func, "func");
                _Func = func;
            }

            public override bool IsSatisfiedBy(T candidate)
            {
                return Func(candidate);
            }

            internal Func<T, bool> Func { get { return _Func; } }
        }

        #endregion

        public static bool IsSatisfiedBy<T>(this Specification<T> specification, params T[] candidates)
        {
            Guard.NotNull(specification, "specification");
            Guard.NotNullOrEmpty(candidates, "candidates");

            return candidates.All(specification.IsSatisfiedBy);
        }

        public static bool IsSatisfiedBy<T>(this Specification<T> specification, IEnumerable<T> candidates)
        {
            Guard.NotNull(specification, "specification");
            Guard.NotNullOrEmpty(candidates, "candidates");

            return candidates.All(specification.IsSatisfiedBy);
        }

        #region Operator Implementation

        public static Specification<T> Not<T>(this Specification<T> specification)
        {
            Guard.NotNull(specification, "specification");

            if (specification.GetType() == typeof(NotSpecification<T>))
                return ((NotSpecification<T>)specification).InnerSpecification;

            return new NotSpecification<T>(specification);
        }

        public static Specification<T> And<T>(this Specification<T> left, Specification<T> right)
        {
            Guard.NotNull(left, "left");
            Guard.NotNull(right, "right");

            return new LogicalSpecification<T>(left,
                right,
                (l, r) => l && r);
        }

        public static Specification<T> Or<T>(this Specification<T> left, Specification<T> right)
        {
            Guard.NotNull(left, "left");
            Guard.NotNull(right, "right");

            return new LogicalSpecification<T>(left,
                right,
                (l, r) => l || r);
        }

        public static Specification<T> XOr<T>(this Specification<T> left, Specification<T> right)
        {
            Guard.NotNull(left, "left");
            Guard.NotNull(right, "right");

            return new LogicalSpecification<T>(left,
                right,
                (l, r) => l ^ r);
        }

        #endregion

        #region Conversions

        public static Predicate<T> ToPredicate<T>(this Specification<T> specification)
        {
            if (specification == null)
                return null;

            var predicateSpecification = specification as PredicateSpecification<T>;

            if (predicateSpecification != null)
                return predicateSpecification.Predicate;

            return specification.IsSatisfiedBy;
        }

        public static Func<T, bool> ToFunc<T>(this Specification<T> specification)
        {
            if (specification == null)
                return null;

            var funcSpecification = specification as FuncSpecification<T>;

            if (funcSpecification != null)
                return funcSpecification.Func;

            return specification.IsSatisfiedBy;
        }

        public static Specification<T> ToSpecification<T>(this Predicate<T> predicate)
        {
            if (predicate == null)
                return null;

            if (predicate.Target != null && predicate.Target is Specification<T>)
                return predicate.Target as Specification<T>;

            return new PredicateSpecification<T>(predicate);
        }

        public static Specification<T> ToSpecification<T>(this Func<T, bool> func)
        {
            if (func == null)
                return null;

            if (func.Target != null && func.Target is Specification<T>)
                return func.Target as Specification<T>;

            return new FuncSpecification<T>(func);
        }

        #endregion
    }

    public abstract class Specification<T>
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

        public static Specification<T> operator &(Specification<T> left, Specification<T> right)
        {
            return left.And(right);
        }

        public static Specification<T> operator |(Specification<T> left, Specification<T> right)
        {
            return left.Or(right);
        }

        public static Specification<T> operator ^(Specification<T> left, Specification<T> right)
        {
            return left.XOr(right);
        }

        public static Specification<T> operator !(Specification<T> specification)
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
