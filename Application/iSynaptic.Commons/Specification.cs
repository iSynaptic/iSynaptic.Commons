using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons
{
    public abstract partial class Specification<T>
    {
        #region Nested Members

        private sealed partial class NotSpecification : Specification<T>
        {
            private Specification<T> _InnerSpecification = null;
            public NotSpecification(Specification<T> specification)
            {
                _InnerSpecification = specification;
            }

            public override bool IsSatisfiedBy(T subject)
            {
                return !(_InnerSpecification.IsSatisfiedBy(subject));
            }

            internal Specification<T> InnerSpecification
            {
                get { return _InnerSpecification; }
            }
        }

        private sealed partial class LogicalSpecification : Specification<T>
        {
            private Specification<T> _Left = null;
            private Specification<T> _Right = null;
            private Func<bool, bool, bool> _Operation = null;

            public LogicalSpecification(Specification<T> left, Specification<T> right, Func<bool, bool, bool> operation)
            {
                _Left = left;
                _Right = right;
                _Operation = operation;
            }

            public override bool IsSatisfiedBy(T subject)
            {
                bool leftResult = _Left.IsSatisfiedBy(subject);
                bool rightResult = _Right.IsSatisfiedBy(subject);

                return _Operation(leftResult, rightResult);
            }
        }

        private sealed partial class PredicateSpecification : Specification<T>
        {
            private Predicate<T> _Predicate = null;

            public PredicateSpecification(Predicate<T> predicate)
            {
                _Predicate = predicate;
            }

            public override bool IsSatisfiedBy(T subject)
            {
                return Predicate(subject);
            }

            internal Predicate<T> Predicate
            {
                get { return _Predicate; }
            }
        }

        #endregion

        public IEnumerable<T> MeetsSpecifcation(IEnumerable<T> subjects)
        {
            foreach (T subject in subjects)
            {
                if (IsSatisfiedBy(subject))
                    yield return subject;
            }
        }

        public IEnumerable<T> FailsSpecification(IEnumerable<T> subjects)
        {
            foreach (T subject in subjects)
            {
                if (IsSatisfiedBy(subject) != true)
                    yield return subject;
            }
        }

        #region IsSatisfiedBy Methods

        public abstract bool IsSatisfiedBy(T subject);

        public bool IsSatisfiedBy(params T[] subjects)
        {
            return Array.TrueForAll<T>(subjects, IsSatisfiedBy);
        }

        public bool IsSatisfiedBy(IEnumerable<T> subjects)
        {
            return IsSatisfiedBy(new List<T>(subjects).ToArray());
        }

        #endregion

        #region Operator Implementation

        public Specification<T> Not()
        {
            return Not(this);
        }

        public virtual Specification<T> And(Specification<T> specification)
        {
            if (specification == null)
                throw new ArgumentNullException("specification");

            return And(this, specification);
        }

        public virtual Specification<T> Or(Specification<T> specification)
        {
            if (specification == null)
                throw new ArgumentNullException("specification");

            return Or(this, specification);
        }

        public virtual Specification<T> XOr(Specification<T> specification)
        {
            if (specification == null)
                throw new ArgumentNullException("specification");

            return XOr(this, specification);
        }

        public static Specification<T> Not(Specification<T> specification)
        {
            if (specification == null)
                throw new ArgumentNullException("specification");

            if (specification.GetType() == typeof(NotSpecification))
                return ((NotSpecification)specification).InnerSpecification;

            return new NotSpecification(specification);
        }

        public static Specification<T> And(Specification<T> left, Specification<T> right)
        {
            if (left == null)
                throw new ArgumentNullException("left");

            if (right == null)
                throw new ArgumentNullException("right");

            return new LogicalSpecification(left,
                right,
                (l, r) => l && r);
        }

        public static Specification<T> Or(Specification<T> left, Specification<T> right)
        {
            if (left == null)
                throw new ArgumentNullException("left");

            if (right == null)
                throw new ArgumentNullException("right");

            return new LogicalSpecification(left,
                right,
                (l, r) => l || r);
        }

        public static Specification<T> XOr(Specification<T> left, Specification<T> right)
        {
            if (left == null)
                throw new ArgumentNullException("left");

            if (right == null)
                throw new ArgumentNullException("right");

            return new LogicalSpecification(left,
                right,
                (l, r) => l ^ r);
        }

        #endregion

        #region Implicit Conversions

        public static implicit operator Predicate<T>(Specification<T> specification)
        {
            if (specification == null)
                return null;

            if (specification is PredicateSpecification)
            {
                PredicateSpecification predicateSpecification = specification as PredicateSpecification;
                return predicateSpecification.Predicate;
            }
            else
                return specification.IsSatisfiedBy;
        }

        public static implicit operator Specification<T>(Predicate<T> predicate)
        {
            if (predicate == null)
                return null;

            if (predicate.Target != null && predicate.Target is Specification<T>)
                return predicate.Target as Specification<T>;
            else
                return new PredicateSpecification(predicate);
        }

        #endregion

        #region Operator Overloads

        public static Specification<T> operator &(Specification<T> left, Specification<T> right)
        {
            return And(left, right);
        }

        public static Specification<T> operator |(Specification<T> left, Specification<T> right)
        {
            return Or(left, right);
        }

        public static Specification<T> operator ^(Specification<T> left, Specification<T> right)
        {
            return XOr(left, right);
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
