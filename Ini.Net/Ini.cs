using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Ini.Net
{
    public class Ini
    {
        private const string _sectionPattern = @"(?:\[(?<sectionName>\w+)\])(?:[\w\W](?!\[\w+\]))+";
        private readonly IList<Section> _sections = new List<Section>();

        public Ini()
        {
        }

        public Ini(Ini other)
        {
            _sections = other?.Sections().ToList();
        }

        public string Add(Section section)
        {
            if (section == null) return "Invalid Section";
            if (_sections.Any(s =>
                s.Name.ComparisonEquals(section.Name) || s.ToString().ComparisonEquals(section.ToString())))
                return "Section exists";
            _sections.Add(section);
            return "";
        }

        public void Remove(Section section)
        {
            if (section == null) return;
            _sections.Remove(section);
        }

        public Section Section(string name)
        {
            return _sections.FirstOrDefault(s => s.Name.ComparisonEquals(name));
        }

        public IEnumerable<Section> Sections()
        {
            return _sections;
        }

        public override string ToString()
        {
            return $"{string.Join($"{Environment.NewLine}{Environment.NewLine}", _sections.Select(s => s))}".Trim();
        }

        public static Ini Parse(string text)
        {
            var ini = new Ini();
            foreach (Match m in Regex.Matches(text, _sectionPattern))
            {
                ini.Add(Net.Section.Parse(m.Value));
            }

            return ini;
        }
    }
}