using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Ini.Net
{
    public class Property
    {
        private const string _propertyPattern = @"^(?'key'[^\[#; ]+?)=(?'value'.*)$";

        public string Key { get; }
        public string Value { get; set; }

        public Property(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Please enter a valid key", nameof(key));

            Key = key.Trim();
            Value = value.Trim();
        }

        public Property(Property other)
        {
            Key = other?.Key;
            Value = other?.Value;
        }

        public override string ToString()
        {
            return $"{Key}={Value}".Trim();
        }

        public static Property Parse(string text)
        {
            var m = Regex.Match(text, _propertyPattern);
            return !m.Success ? default : new Property(m.Groups["key"].Value, m.Groups["value"].Value);
        }
    }
}