using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeDek.Ini
{
    public class Section
    {
        const string _sectionPattern = @"^(?:\[(?<sectionName>.+)?\])(?:[\w\W](?!^\[.+\]))+$";
        readonly List<Property> _properties = new List<Property>();

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

        public bool Add(Property property, PropertyAddOption addOption = PropertyAddOption.PropertyIsUnique)
        {
            if (property == null) return false;

            switch (addOption)
            {
                case PropertyAddOption.KeyIsUnique:
                    if (Property(property.Key) == null) _properties.Add(property);

                    return true;
                case PropertyAddOption.PropertyIsUnique:
                    if (Property(property.Key, property.Value) == null) _properties.Add(property);

                    return true;
                case PropertyAddOption.Overwrite:
                    var index = _properties
                        .FindIndex(p => p.Key.Equals(property.Key, StringComparison.OrdinalIgnoreCase));

                    if (index <= -1)
                    {
                        _properties.Add(property);
                        return true;
                    } else
                    {
                        _properties.RemoveAt(index);
                        _properties.Insert(index, property);
                        return true;
                    }
            }

            return false;
        }

        public static Section Parse(string text)
        {
            if (string.IsNullOrEmpty(text)) return default;

            var s = Regex.Match(text.Trim(), _sectionPattern);
            if (!s.Success) return default;

            var sec = new Section(s.Groups["sectionName"].Value);
            foreach (var l in s.Value.SplitToLines())
                sec.Add(CodeDek.Ini.Property.Parse(l));
            return sec;
        }

        public void Clear() => _properties.Clear();

        public bool Remove(Property property) => _properties.Remove(property);

        public void Remove(string key, string value) => _properties.Remove(Property(key, value));

        public Property Property(string key, bool ignoreCase = true) =>
            _properties.Find(p => ignoreCase
                                 ? p.Key.Equals(key, StringComparison.OrdinalIgnoreCase)
                                 : p.Key.Equals(key));

        public Property Property(string key, string value) =>
            _properties.Find(p => p.Key.IgnoreCaseEquals(key) && p.Value.IgnoreCaseEquals(value));

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
            $"[{Name}]{Environment.NewLine}{string.Join(Environment.NewLine, _properties.FindAll(p => p.ToString() != ""))}"
                .Trim();
    }
}