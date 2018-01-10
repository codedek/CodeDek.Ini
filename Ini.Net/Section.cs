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

                    _properties.Add(property);
                    return true;
            }

            return false;
        }

        public static Section Parse(string text)
        {
            var s = Regex.Match(text.Trim(), _sectionPattern);
            if (!s.Success) return default;
            var sec = new Section(s.Groups["sectionName"].Value);
            s.Value.SplitToLines().ForEach(l => sec.Add(Net.Property.Parse(l)));
            return sec;
        }

        public void Clear() => _properties.Clear();

        public bool Remove(Property property) => _properties.Remove(property);

        public void Remove(string key, string value) => _properties.Remove(Property(key, value));

        public Property Property(string key, bool ignoreCase = true) => _properties.FirstOrDefault(p => p.Key.IgnoreCaseEquals(key,ignoreCase));

        public Property Property(string key, string value) =>
            _properties.FirstOrDefault(p => p.Key.IgnoreCaseEquals(key) && p.Value.IgnoreCaseEquals(value));

        public IEnumerable<Property> Properties() => _properties;

        public IEnumerable<Property> Properties(Filter filterKey, string search)
        {
            switch (filterKey)
            {
                case Filter.Is:
                    return _properties.Where(p => p.Key.IgnoreCaseEquals(search, false));
                case Filter.StartsWith:
                    return _properties.Where(p => p.Key.IgnoreCaseStartsWith(search, false));
                case Filter.EndsWith:
                    return _properties.Where(p => p.Key.IgnoreCaseEndsWith(search, false));
                case Filter.Contains:
                    return _properties.Where(p => p.Key.IgnoreCaseContains(search, false));
                case Filter.IgnoreCaseIs:
                    return _properties.Where(p => p.Key.IgnoreCaseEquals(search));
                case Filter.IgnoreCaseStartsWith:
                    return _properties.Where(p => p.Key.IgnoreCaseStartsWith(search));
                case Filter.IgnoreCaseEndsWith:
                    return _properties.Where(p => p.Key.IgnoreCaseEndsWith(search));
                case Filter.IgnoreCaseContains:
                    return _properties.Where(p => p.Key.IgnoreCaseContains(search));
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterKey), filterKey, null);
            }
        }

        public override string ToString() =>
            $"[{Name}]{Environment.NewLine}{string.Join(Environment.NewLine, _properties.Select(p => p))}".Trim();
    }
}