using System;

namespace iSynaptic.Commons
{
    public static class PredicateExtensions
    {
        public static Predicate<T> And<T>(this Predicate<T> @this, Predicate<T> other)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(other, "other");
            return input => @this(input) && other(input);
        }

        public static Predicate<T> Or<T>(this Predicate<T> @this, Predicate<T> other)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(other, "other");
            return input => @this(input) || other(input);
        }

        public static Predicate<T> XOr<T>(this Predicate<T> @this, Predicate<T> other)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(other, "other");
            return input => @this(input) ^ other(input);
        }
    }
}
