using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Ini.Net
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
                return (T) formatter.Deserialize(ms);
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

        public static bool ComparisonEquals(this string source, string value,
            StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
            => source.Equals(value, comparisonType);
    }
}