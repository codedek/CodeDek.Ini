using System;
using System.Text.RegularExpressions;

namespace CodeDek.Ini
{
    public class Property
    {
        const string _propertyPattern = @"^(?'key'[^\[#; ]+?)=(?'value'.*)$";

        public string Key { get; }
        public string Value { get; set; }

        public Property(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Please enter a valid key", nameof(key));

            Key = key.Trim();
            Value = value?.Trim();
        }

        public Property(Property other)
        {
            if (other == null) throw new ArgumentException("Cannot pass a null property", nameof(other));
            Key = other?.Key;
            Value = other?.Value;
        }

        public static Property Parse(string text)
        {
            var m = Regex.Match(text.Trim(), _propertyPattern);
            return !m.Success ? default : new Property(m.Groups["key"].Value, m.Groups["value"].Value);
        }

        public override string ToString() => $"{Key}={Value}".Trim();
    }
}