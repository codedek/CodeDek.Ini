using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace CodeDek.Ini
{
    internal static class HelperFunction
    {
        public static IEnumerable<TResult> Map<TSorce, TResult>(this IEnumerable<TSorce> source,
            Func<TSorce, TResult> selector)
        {
            foreach (var item in source)
                yield return selector(item);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }

        public static void ForEach<T>(this MatchCollection source, Action<T> action)
        {
            foreach (T item in source)
                action(item);
        }

        public static T DeepCopy<T>(this T other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

        public static string SerializeObject<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

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