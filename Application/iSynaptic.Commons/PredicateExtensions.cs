﻿using System;

namespace iSynaptic.Commons
{
    public static class PredicateExtensions
    {
        public static Predicate<T> And<T>(this Predicate<T> self, Predicate<T> right)
        {
            return input => self(input) && right(input);
        }

        public static Predicate<T> Or<T>(this Predicate<T> self, Predicate<T> right)
        {
            return input => self(input) || right(input);
        }

        public static Predicate<T> XOr<T>(this Predicate<T> self, Predicate<T> right)
        {
            return input => self(input) ^ right(input);
        }
    }
}