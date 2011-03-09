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

            if (typeof(T).IsAssignableFrom(value.GetType()) != true)
            {
                throw string.IsNullOrWhiteSpace(message)
                    ? new ArgumentException(string.Format("{0} must be of type '{1}'", name, typeof(T).FullName), name)
                    : new ArgumentException(message, name);
            }

            return (T) value;
        }

        public static string Matches(string value, Regex validation, string name, string message = null)
        {
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
            if (ReferenceEquals(value, null))
                return value;

            if (value.Any() != true)
            {
                throw string.IsNullOrWhiteSpace(message)
                    ? new ArgumentException(string.Format("{0} must not be empty.", name), name)
                    : new ArgumentException(message, name);
            }

            return value;
        }

        public static void NotNullOrEmpty(string value, string valueName)
        {
            NotNull(value, valueName);
            NotEmpty(value, valueName);
        }

        public static void NotNullOrEmpty<T>(IEnumerable<T> value, string valueName)
        {
            NotNull(value, valueName);
            NotEmpty(value, valueName);
        }

        public static void NotNullOrWhiteSpace(string value, string valueName)
        {
            NotNull(value, valueName);
            NotWhiteSpace(value, valueName);
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
