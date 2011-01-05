using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public static class Guard
    {
        public static void MustBeDefined(Enum value, string valueName)
        {
            if(value.IsDefined() != true)
                throw new ArgumentOutOfRangeException(valueName);
        }

        public static void NotNull<T>(T value, string valueName)
        {
            if(value == null)
                throw new ArgumentNullException(valueName);
        }

        public static void NotEmpty(string value, string valueName)
        {
            if (value == string.Empty)
                throw new ArgumentException(string.Format("{0} must not be empty.", valueName), valueName);
        }

        public static void NotWhiteSpace(string value, string valueName)
        {
            if (value == null)
                return;

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(string.Format("{0} must not be whitespace only.", valueName), valueName);
        }

        public static void NotEmpty(Guid value, string valueName)
        {
            if (value == Guid.Empty)
                throw new ArgumentException(string.Format("{0} must not be equal to Guid.Empty.", valueName), valueName);
        }

        public static void NotEmpty<T>(IEnumerable<T> value, string valueName)
        {
            if (value == null)
                return;

            if (value.Any() != true)
                throw new ArgumentException(string.Format("{0} must not be empty.", valueName), valueName);
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
    }
}
