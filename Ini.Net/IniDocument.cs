using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ini.Net
{
    public static class IniDocument
    {
        public static Ini Parse(string text)
        {
            var ini = new Ini();
            Section sec = null;
            foreach (var line in text.SplitToLines().Where(l
                => !string.IsNullOrEmpty(l)
                   && !l.StartsWith(";")
                   && !l.StartsWith("#")))
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
            foreach (var line in File.ReadLines(path).Where(l
                => !string.IsNullOrEmpty(l)
                   && !l.StartsWith(";")
                   && !l.StartsWith("#")))
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

        public static string Read(string file, string section, string key)
        {
            return Load(file)?.Section(section)?.Property(key)?.Value;
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

        //public static string Write(string file, string section, string key, string value,
        //    bool writeUniqueProperty = false, bool overwrite = true)
        //{
        //    try
        //    {
        //        var ini = Load(file) ?? new Ini();
        //        if (ini.Section(section) == null) ini.Add(new Section(section));
        //        if (writeUniqueProperty)
        //        {
        //            if (ini.Section(section).Property(key) == null) ini.Section(section).Add(new Property(key, value));
        //            else if (overwrite)
        //                ini.Section(section).Property(key).Value = value;
        //        }

        //        if (ini.Section(section).Property(key, value) == null)
        //            ini.Section(section).Add(new Property(key, value));

        //        File.WriteAllText(file, ini.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }

        //    return "";
        //}

        public static string SerializeObject(object obj)
        {
            return default;
        }

        public static T DeserializeObject<T>(string obj)
        {
            return default;
        }
    }

    public enum WriteOption
    {
        UpdateExistingPropertyValue,
        IfPropertyKeyAndValueIsUnique
    }
}