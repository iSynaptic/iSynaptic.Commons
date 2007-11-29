using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.CompilerServices;

namespace iSynaptic.Commons.Extensions
{
    public static class EnumExtensions
    {
        public static bool IsDefined(this Enum input)
        {
            return Enum.IsDefined(input.GetType(), input);
        }
    }
}
