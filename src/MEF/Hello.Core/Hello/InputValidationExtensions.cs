using System;
using System.Linq;

namespace Hello
{
    public static class InputValidationExtensions
    {
        private static readonly string[] AllowedMethods = {"GET","PUT","POST","DELETE","HEAD"};

        public static void ValidateStartsWith(this string o, string value)
        {
            if (!o.StartsWith(value, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception($"StartsWith [{value}] failed on subject:[{o}]");
            }
        }
        public static void ValidateMethod(this string stringToCheck)
        {
            var valid = AllowedMethods.Any(stringToCheck.Contains);

            if (!valid)
            {
                throw new Exception($"Method [{stringToCheck}] is not allowed");
            }
        }
    }
}