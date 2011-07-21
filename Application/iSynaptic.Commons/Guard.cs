// The MIT License
// 
// Copyright (c) 2011 Jordan E. Terrell
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
using System.Text.RegularExpressions;

namespace iSynaptic.Commons
{
    public static class Guard
    {
        public static T MustBeDefined<T>(Enum value, string name, string message = null)
        {
            GuardClassRequires(() => typeof(T).IsEnum, "T", "Type argument 'T' must be an enum type.");
            GuardClassNotNullOrWhiteSpace(name, "name");

            IsOfType<T>(value, "value");

            if (value.IsDefined() != true)
            {
                throw string.IsNullOrWhiteSpace(message)
                    ? new ArgumentOutOfRangeException(name)
                    : new ArgumentOutOfRangeException(name, message);
            }

            return (T) (object) value;
        }

        public static T IsOfType<T>(object value, string name, string message = null)
        {
            GuardClassNotNull(value, "value");
            GuardClassNotNullOrEmpty(name, "name");

            return (T) MustSatisfy(value, x => typeof(T).IsAssignableFrom(x.GetType()), name,
                string.IsNullOrWhiteSpace(message)
                    ? string.Format("{0} must be of type '{1}'", name, typeof(T).FullName)
                    : message);
        }

        public static string Matches(string value, string pattern, string name, string message = null)
        {
            GuardClassNotNull(value, "value");
            GuardClassNotNullOrEmpty(pattern, "pattern");

            return Matches(value, new Regex(pattern), name, message);
        }

        public static string Matches(string value, Regex validation, string name, string message = null)
        {
            GuardClassNotNull(value, "value");
            GuardClassNotNull(validation, "validation");

            return MustSatisfy(value, validation.IsMatch, name,
                               string.IsNullOrWhiteSpace(message)
                                   ? string.Format("{0} does not match the required pattern.", name)
                                   : message);
        }

        public static T MustSatisfy<T>(T value, Func<T, bool> predicate, string name, string message)
        {
            GuardClassNotNull(predicate, "predicate");
            GuardClassNotNullOrWhiteSpace(name, "name");

            if(!predicate(value))
                throw new ArgumentException(name, message);

            return value;
        }

        public static T NotNull<T>(T value, string name, string message = null)
        {
            GuardClassNotNullOrEmpty(name, "name");

            if (ReferenceEquals(value, null))
            {
                throw string.IsNullOrWhiteSpace(message)
                    ? new ArgumentNullException(name)
                    : new ArgumentNullException(name, message);
            }

            return value;
        }

        public static string NotEmpty(string value, string name, string message = null)
        {
            if (value.Equals(string.Empty))
            {
                throw string.IsNullOrWhiteSpace(message)
                    ? new ArgumentException(string.Format("{0} must not be empty.", name), name)
                    : new ArgumentException(message, name);
            }

            return value;
        }

        public static string NotWhiteSpace(string value, string name, string message = null)
        {
            if (ReferenceEquals(value, null))
                return value;

            if (string.IsNullOrWhiteSpace(value))
            {
                throw string.IsNullOrWhiteSpace(message)
                    ? new ArgumentException(string.Format("{0} must not be whitespace only.", name), name)
                    : new ArgumentException(message, name);
            }

            return value;
        }

        public static Guid NotEmpty(Guid value, string name, string message = null)
        {
            if (value.Equals(Guid.Empty))
            {
                 throw string.IsNullOrWhiteSpace(message)
                    ? new ArgumentException(string.Format("{0} must not be equal to Guid.Empty.", name), name)
                    : new ArgumentException(message, name);
            }

            return value;
        }

        public static IEnumerable<T> NotEmpty<T>(IEnumerable<T> value, string name, string message = null)
        {
            if (!ReferenceEquals(value, null) && value.Any() != true)
            {
                throw string.IsNullOrWhiteSpace(message)
                    ? new ArgumentException(string.Format("{0} must not be empty.", name), name)
                    : new ArgumentException(message, name);
            }

            return value;
        }

        public static string NotNullOrEmpty(string value, string name)
        {
            NotNull(value, name);
            NotEmpty(value, name);

            return value;
        }

        public static IEnumerable<T> NotNullOrEmpty<T>(IEnumerable<T> value, string name)
        {
            NotNull(value, name);
            return NotEmpty(value, name);
        }

        public static T MustBeGreaterThan<T>(T value, T compareTo, string name, string message = null)
            where T : IComparable<T>
        {
            return Compare(value, compareTo, x => x > 0, name, string.IsNullOrWhiteSpace(message)
                ? string.Format("{0} must be greater than {1}.", name, compareTo)
                : message);
        }

        public static T MustBeGreaterThanOrEqual<T>(T value, T compareTo, string name, string message = null)
            where T : IComparable<T>
        {
            return Compare(value, compareTo, x => x >= 0, name, string.IsNullOrWhiteSpace(message)
                ? string.Format("{0} must be greater than or equal to {1}.", name, compareTo)
                : message);
        }

        public static T MustBeLessThan<T>(T value, T compareTo, string name, string message = null)
            where T : IComparable<T>
        {
            return Compare(value, compareTo, x => x < 0, name, string.IsNullOrWhiteSpace(message)
                ? string.Format("{0} must be less than {1}.", name, compareTo)
                : message);
        }

        public static T MustBeLessThanOrEqual<T>(T value, T compareTo, string name, string message = null)
            where T : IComparable<T>
        {
            return Compare(value, compareTo, x => x <= 0, name, string.IsNullOrWhiteSpace(message)
                ? string.Format("{0} must be less than or equal to {1}.", name, compareTo)
                : message);
        }

        private static T Compare<T>(T value, T compareTo, Func<int, bool> rule, string name, string message)
            where T : IComparable<T>
        {
            GuardClassNotNull(value, "value");
            GuardClassNotNull(compareTo, "compareTo");
            GuardClassNotNull(rule, "rule");
            GuardClassNotNullOrWhiteSpace(name, "name");
            GuardClassNotNullOrWhiteSpace(message, "message");

            if (rule(value.CompareTo(compareTo)) != true)
                throw new ArgumentOutOfRangeException(name, value, message);

            return value;
        }

        public static string NotNullOrWhiteSpace(string value, string name)
        {
            NotNull(value, name);
            return NotWhiteSpace(value, name);
        }

        private static void GuardClassRequires(Func<bool> predicate, string name, string message)
        {
            if(!predicate())
                throw new ArgumentException(string.Format("Guard class usage: {0}", string.Format(message, name)), name);
        }

        private static void GuardClassNotNullOrEmpty(string value, string name)
        {
            GuardClassNotNull(value, name);
            GuardClassNotEmpty(value, name);
        }

        private static void GuardClassNotNullOrWhiteSpace(string value, string name)
        {
            GuardClassNotNull(value, name);
            GuardClassNotWhiteSpace(value, name);
        }

        private static void GuardClassNotNull<T>(T value, string name)
        {
            GuardClassRequires(
                () => !ReferenceEquals(value, null),
                name,
                "'{0}' must be non-null.");
        }

        private static void GuardClassNotEmpty(string value, string name)
        {
            GuardClassRequires(
                () => ReferenceEquals(value, null) || !string.IsNullOrEmpty(value),
                name,
                "'{0}' must be non-empty.");
        }

        private static void GuardClassNotWhiteSpace(string value, string name)
        {
            GuardClassRequires(
                () => ReferenceEquals(value, null) || !string.IsNullOrWhiteSpace(value),
                name,
                "'{0}' must be non-whitespace.");
        }
    }
}
