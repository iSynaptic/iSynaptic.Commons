using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public static class Check
    {
        public static Outcome<CheckFailure> NotNull<T>(T value, string name, string message = null)
        {
            return value == null
                ? Outcome.Failure(new CheckFailure(CheckType.NotNull, name, message ?? string.Format("The argument {0} must not be null.", name)))
                : Outcome<CheckFailure>.Success;
        }

        public static Outcome<CheckFailure> NotEmpty(string value, string name, string message = null)
        {
            return string.Empty.Equals(value)
                ? Outcome.Failure(new CheckFailure(CheckType.NotEmpty, name, message ?? string.Format("The argument {0} must not be empty.", name)))
                : Outcome<CheckFailure>.Success;
        }

        public static Outcome<CheckFailure> NotEmpty(Guid value, string name, string message = null)
        {
            return value.Equals(Guid.Empty)
                ? Outcome.Failure(new CheckFailure(CheckType.NotEmpty, name, message ?? string.Format("The argument {0} must not be empty.", name)))
                : Outcome<CheckFailure>.Success;
        }

        public static Outcome<CheckFailure> NotEmpty<T>(IEnumerable<T> value, string name, string message = null)
        {
            return null != value && value.Any() != true
                ? Outcome.Failure(new CheckFailure(CheckType.NotEmpty, name, message ?? string.Format("The argument {0} must not be empty.", name)))
                : Outcome<CheckFailure>.Success;
        }

        public static Outcome<CheckFailure> NotNullOrEmpty(string value, string name, string message = null)
        {
            return string.IsNullOrEmpty(value)
                ? Outcome.Failure(new CheckFailure(CheckType.NotNullOrEmpty, name, message ?? string.Format("The argument {0} must not be null or empty.", name)))
                : Outcome<CheckFailure>.Success;
        }

        public static Outcome<CheckFailure> NotNullOrEmpty<T>(IEnumerable<T> value, string name, string message = null)
        {
            return null == value || value.Any() != true
                ? Outcome.Failure(new CheckFailure(CheckType.NotNullOrEmpty, name, message ?? string.Format("The argument {0} must not be null or empty.", name)))
                : Outcome<CheckFailure>.Success;
        }

        public static Outcome<CheckFailure> NotNullOrWhiteSpace(string value, string name, string message = null)
        {
            return string.IsNullOrWhiteSpace(value)
                ? Outcome.Failure(new CheckFailure(CheckType.NotNullOrWhiteSpace, name, message ?? string.Format("The argument {0} must not be null or white space.", name)))
                : Outcome<CheckFailure>.Success;
        }

        public static Outcome<CheckFailure> MustBeDefined<T>(T value, string name, string message = null)
        {
            return Enum.IsDefined(typeof(T), value) != true
                ? Outcome.Failure(new CheckFailure(CheckType.MustBeDefined, name, message ?? string.Format("The argument {0} must be a defined enum value.", name)))
                : Outcome<CheckFailure>.Success;
        }

        public static Outcome<CheckFailure> IsOfType<T>(object value, string name, string message = null)
        {
            return !typeof(T).IsAssignableFrom(value.GetType())
                ? Outcome.Failure(new CheckFailure(CheckType.IsOfType, name, message ?? string.Format("The argument {0} must be an instance of the type {1}.", name, typeof(T).FullName)))
                : Outcome<CheckFailure>.Success;
        }

        public static Outcome<CheckFailure> That(bool expectation, string name, string message)
        {
            return !expectation
                ? Outcome.Failure(new CheckFailure(CheckType.That, name, message))
                : Outcome<CheckFailure>.Success;
        }
    }

    public class CheckFailure
    {
        public CheckFailure(CheckType type, string name, string message)
        {
            Type = type;
            Name = name;
            Message = message;
        }

        public CheckType Type { get; private set; }
        public string Name { get; private set; }
        public string Message { get; private set; }
    }

    public enum CheckType
    {
        NotNull,
        NotEmpty,
        NotNullOrEmpty,
        NotNullOrWhiteSpace,
        MustBeDefined,
        IsOfType,
        That
    }
}
