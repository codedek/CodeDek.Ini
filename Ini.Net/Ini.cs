using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeDek.Ini
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
            Regex.Matches(text.Trim(), _sectionPattern).ForEach<Match>(m => ini.Add(CodeDek.Ini.Section.Parse(m.Value)));
            return ini;
        }

        public bool Remove(Section section) => _sections.Remove(section);

        public void Clear() => _sections.Clear();

        public Section Section(string name, bool ignoreCase = true) => _sections.FirstOrDefault(s => s.Name.IgnoreCaseEquals(name, ignoreCase));

        public IEnumerable<Section> Sections() => _sections;

        public IEnumerable<Section> Sections(Filter filterName, string search)
        {
            switch (filterName)
            {
                case Filter.Is:
                    return _sections.Where(s => s.Name.IgnoreCaseEquals(search, false));
                case Filter.StartsWith:
                    return _sections.Where(s => s.Name.IgnoreCaseStartsWith(search, false));
                case Filter.EndsWith:
                    return _sections.Where(s => s.Name.IgnoreCaseEndsWith(search, false));
                case Filter.Contains:
                    return _sections.Where(s => s.Name.IgnoreCaseContains(search, false));
                case Filter.IgnoreCaseIs:
                    return _sections.Where(s => s.Name.IgnoreCaseEquals(search));
                case Filter.IgnoreCaseStartsWith:
                    return _sections.Where(s => s.Name.IgnoreCaseStartsWith(search));
                case Filter.IgnoreCaseEndsWith:
                    return _sections.Where(s => s.Name.IgnoreCaseEndsWith(search));
                case Filter.IgnoreCaseContains:
                    return _sections.Where(s => s.Name.IgnoreCaseContains(search));
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterName), filterName, null);
            }
        }

        public override string ToString() =>
            $"{string.Join($"{Environment.NewLine}{Environment.NewLine}", _sections.Select(s => s))}".Trim();
    }
}