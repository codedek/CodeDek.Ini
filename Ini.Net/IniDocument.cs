using System.IO;
using System.Linq;

namespace CodeDek.Ini
{
    public static class IniDocument
    {
        public static Ini Parse(string text)
        {
            var ini = new Ini();
            Section sec = null;
            foreach (var line in text.SplitToLines()
                                     .Where(l
                                                => !string.IsNullOrEmpty(l) &&
                                                   !l.StartsWith(";") &&
                                                   !l.StartsWith("#")))
            {
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    if (sec != null) ini.Add(new Section(sec));
                    sec = new Section(line);
                }

                if (line.Contains("="))
                {
                    sec?.Add(new Property(line.Split('=')[0], line.Substring(line.IndexOf('=') + 1)));
                }
            }

            ini.Add(sec);
            return ini;
        }

        public static Ini Load(string path)
        {
            if (!File.Exists(path)) return default;
            var ini = new Ini();
            Section sec = null;
            foreach (var line in File.ReadLines(path)
                                     .Where(l
                                                => !string.IsNullOrEmpty(l) &&
                                                   !l.StartsWith(";") &&
                                                   !l.StartsWith("#")))
            {
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    if (sec != null) ini.Add(new Section(sec));
                    sec = new Section(line);
                }

                if (line.Contains("="))
                {
                    sec?.Add(new Property(line.Split('=')[0], line.Substring(line.IndexOf('=') + 1)));
                }
            }

            ini.Add(sec);
            return ini;
        }

        public static bool Write(string file, string section, string key, string value,
            WriteOption option = WriteOption.UpdateExistingPropertyValue)
        {
            var ini = Load(file) ?? new Ini();
            if (ini.Section(section) == null) ini.Add(new Section(section));
            switch (option)
            {
                case WriteOption.UpdateExistingPropertyValue:
                    if (ini.Section(section).Property(key) == null)
                        ini.Section(section).Add(new Property(key, value));
                    else ini.Section(section).Property(key).Value = value;
                    File.WriteAllText(file, ini.ToString());
                    return true;
                case WriteOption.IfPropertyKeyAndValueIsUnique:
                    if (ini.Section(section).Property(key, value) == null)
                        ini.Section(section).Add(new Property(key, value));
                    File.WriteAllText(file, ini.ToString());
                    return true;
            }

            return false;
        }

        //public static string Read(string file, string section, string key) =>
        //    Load(file)?.Section(section)?.Property(key)?.Value;

        public static string Read(string file, string section, string key, bool ignoreCase = true)
        {
            if (!File.Exists(file)) return default;
            var isMatch = false;
            foreach (var l in File.ReadLines(file)
                                     .Where(l => !string.IsNullOrEmpty(l) &&
                                                 !l.StartsWith(";") &&
                                                 !l.StartsWith("#")))
            {
                var line = l.Trim();
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    isMatch = section.IgnoreCaseEquals(line.TrimStart('[').TrimEnd(']').Trim());
                }

                if (isMatch && line.Contains("="))
                {
                    if (key.IgnoreCaseEquals(line.Split('=')[0], ignoreCase))
                    {
                        return line.Substring(line.IndexOf('=') + 1);
                    }
                }
            }

            return default;
        }

        public static string SerializeObject(object obj)
        {
            return default;
        }

        public static T DeserializeObject<T>(string obj)
        {
            return default;
        }
    }
}