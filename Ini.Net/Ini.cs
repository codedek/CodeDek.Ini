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

        public Ini(Ini other) => _sections = other?.Sections().ToList();

        public bool Add(Section section, AddSection option = AddSection.IfNameIsUnique)
        {
            if (section == null) return false;
            switch (option)
            {
                case AddSection.IfNameIsUnique:
                    if (Section(section.Name) == null) _sections.Add(section);

                    return true;
                case AddSection.MergeWithExisting:
                    if (Section(section.Name) == null) _sections.Add(section);
                    else
                        foreach (var property in section.Properties())
                            Section(section.Name).Add(property);

                    return true;
                case AddSection.MergeAndUpdateExisting:
                    if (Section(section.Name) == null) _sections.Add(section);
                    else
                        foreach (var property in section.Properties())
                            Section(section.Name).Add(property, AddProperty.UpdateValue);

                    return true;
                case AddSection.OverwriteExisting:
                    var s = Section(section.Name);
                    if (s == null) _sections.Add(section);
                    else _sections[_sections.IndexOf(s)] = section;

                    return true;
            }

            return false;
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

        public bool Remove(Section section) => _sections.Remove(section);

        public void Clear() => _sections.Clear();

        public Section Section(string name) => _sections.FirstOrDefault(s => s.Name.ComparisonEquals(name));

        public IEnumerable<Section> Sections() => _sections;

        public override string ToString() =>
            $"{string.Join($"{Environment.NewLine}{Environment.NewLine}", _sections.Select(s => s))}".Trim();
    }
}