using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace CodeDek.Ini
{
    internal static class StringExtensions
    {
        public static IEnumerable<string> SplitToLines(this string input)
        {
            if (input == null)
            {
                yield break;
            }

            using (var reader = new StringReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line))
                        yield return line;
                }
            }
        }

        public static bool IgnoreCaseEquals(this string source, string value, bool ignoreCase = true)
            => ignoreCase ? source.Equals(value, StringComparison.OrdinalIgnoreCase) : source.Equals(value);

        public static bool IgnoreCaseStartsWith(this string source, string value, bool ignoreCase = true)
            => ignoreCase ? source.StartsWith(value, StringComparison.OrdinalIgnoreCase) : source.StartsWith(value);

        public static bool IgnoreCaseEndsWith(this string source, string value, bool ignoreCase = true)
            => ignoreCase ? source.EndsWith(value, StringComparison.OrdinalIgnoreCase) : source.EndsWith(value);

        public static bool IgnoreCaseContains(this string source, string value, bool ignoreCase = true)
            => ignoreCase ? source.ToUpper().Contains(value.ToUpper()) : source.Contains(value);
    }
}