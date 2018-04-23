using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeDek.Ini
{
  public class Ini
  {
    const string _sectionPattern = @"(?:\[(?<sectionName>\w+)\])(?:[\w\W](?!\[\w+\]))+";
    readonly List<Section> _sections = new List<Section>();

    public Ini()
    {
    }

    public Ini(params Section[] sections)
    {
      foreach (var section in sections)
        if (!CodeDek.Ini.Section.IsNullOrEmpty(section))
          Add(section);
    }

    public Ini(Ini other) => _sections=other?.Sections().ToList();

    public bool Add(Section section, SectionAddOption addOption = SectionAddOption.NameIsUnique)
    {
      if (section==null)
        return false;

      switch (addOption)
      {
        case SectionAddOption.NameIsUnique:
          if (Section(section.Name)==null)
            _sections.Add(section);

          return true;
        case SectionAddOption.Merge:
          if (Section(section.Name)==null)
            _sections.Add(section);
          else
            foreach (var property in section.Properties())
              Section(section.Name).Add(property);

          return true;
        case SectionAddOption.MergeOverwrite:
          if (Section(section.Name)==null)
            _sections.Add(section);
          else
            foreach (var property in section.Properties())
              Section(section.Name).Add(property, PropertyAddOption.Overwrite);

          return true;
        case SectionAddOption.Overwrite:
          var s = Section(section.Name);
          if (s==null)
            _sections.Add(section);
          else
            _sections[_sections.IndexOf(s)]=section;

          return true;
      }

      return false;
    }

    public static Ini Parse(string text)
    {
      if (string.IsNullOrEmpty(text))
        return default;

      var ini = new Ini();
      foreach (Match m in Regex.Matches(text.Trim(), _sectionPattern))
        ini.Add(CodeDek.Ini.Section.Parse(m.Value));
      return ini;
    }

    public bool Remove(Section section) => _sections.Remove(section);

    public void Clear() => _sections.Clear();

    public Section Section(string name, bool ignoreCase = true) =>
      _sections.Find(s => ignoreCase
                       ? s.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                       : s.Name.Equals(name));

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

    public IEnumerable<Section> Sections(string regxPattern)
    {
      if (string.IsNullOrWhiteSpace(regxPattern))
        yield return default;

      foreach (var section in _sections)
      {
        var m = Regex.Match(section.Name, regxPattern);
        if (m.Success)
          yield return section;
      }
    }

    public override string ToString() =>
      $"{string.Join($"{Environment.NewLine}{Environment.NewLine}", _sections.Select(s => s))}".Trim();

    public static bool IsNullOrEmpty(Ini ini) => ini?.Sections().Any()==false;
  }
}