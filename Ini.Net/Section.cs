using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Ini.Net
{
    public class Section
    {
        private const string _sectionPattern = @"^(?:\[(?<sectionName>.+)?\])(?:[\w\W](?!^\[.+\]))+$";
        private readonly IList<Property> _properties = new List<Property>();

        public string Name { get; }

        public Section(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new System.ArgumentException("Please enter a valid name", nameof(name));

            Name = name.Trim().TrimStart('[').TrimEnd(']').Trim();
        }

        public Section(Section other)
        {
            if (other == null)
                throw new ArgumentException("Cannot pass a null section", nameof(other));

            Name = other?.Name;
            _properties = other?.Properties().ToList();
        }

        public void Clear() => _properties.Clear();

        public bool Add(Property property, AddProperty option = AddProperty.IfKeyAndValueIsUnique)
        {
            if (property == null) return false;
            switch (option)
            {
                case AddProperty.IfKeyIsUnique:
                    if (Property(property.Key) == null) _properties.Add(property);
                    
                    return true;
                case AddProperty.IfKeyAndValueIsUnique:
                    if (Property(property.Key, property.Value) == null) _properties.Add(property);
                    
                    return true;
                case AddProperty.UpdateValue:
                    if (Property(property.Key, property.Value) != null)
                    {
                        Property(property.Key, property.Value).Value = property.Value;
                        return true;
                    }

                    if (Property(property.Key) != null)
                    {
                        Property(property.Key).Value = property.Value;
                        return true;
                    }

                    break;
            }

            return false;
        }

        public void Remove(Property property)
        {
            if (property == null) return;
            _properties.Remove(property);
        }

        public void Remove(string key, string value)
        {
            _properties.Remove(Property(key, value));
        }

        public Property Property(string key)
        {
            return _properties.FirstOrDefault(p => p.Key.ComparisonEquals(key));
        }

        public Property Property(string key, string value)
        {
            return _properties.FirstOrDefault(p => p.Key.ComparisonEquals(key) && p.Value.ComparisonEquals(value));
        }

        public IEnumerable<Property> Properties()
        {
            return _properties;
        }

        public override string ToString()
        {
            return $"[{Name}]{Environment.NewLine}{string.Join(Environment.NewLine, _properties.Select(p => p))}"
                .Trim();
        }

        public static Section Parse(string text)
        {
            var s = Regex.Match(text, _sectionPattern);
            if (!s.Success) return default;
            var sec = new Section(s.Groups["sectionName"].Value);

            foreach (var l in s.Value.SplitToLines())
                sec.Add(Net.Property.Parse(l));
            return sec;
        }
    }
}