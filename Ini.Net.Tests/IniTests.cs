using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ini.Net.Tests
{
    [TestClass]
    public class IniTests
    {
        public TestContext TestContext { get; set; }
        // ctor
        // add
        // parse
        // sections
        // section
        // remove

        [TestMethod]
        public void Ini_WhenPassInAnotherIni_MakesAProperCopy()
        {
            var i = new Ini();
            var i1 = new Ini(i);
            Assert.AreNotEqual(i, i1);
        }

        [TestMethod]
        public void Ini_OnlyAddsUniqueSections()
        {
            var s1 = new Section("Sec1");
            var p1 = new Property("Key", "Value");
            var p2 = new Property("Key1", "Value1");
            var p3 = new Property("Key2", "value2");
            s1.Add(p1);
            s1.Add(p2);
            s1.Add(p3);

            var s2 = new Section("Sec2");
            var p4 = new Property("-", "delete1");
            var p5 = new Property("-", "delete2");
            s2.Add(p4);
            s2.Add(p5);

            var s3 = new Section("SEC2");
            var p6 = new Property("-", "DELETE1");
            var p7 = new Property("-", "DELETE2");
            s3.Add(p6);
            s3.Add(p7);

            var ini = new Ini();
            ini.Add(s1);
            ini.Add(s1);
            ini.Add(s2);
            ini.Add(s2);
            ini.Add(s3);
            ini.Add(s3);

            Assert.AreEqual(2, ini.Sections().Count());

            var expected = $@"
[Sec1]
Key=Value
Key1=Value1
Key2=Value2

[Sec2]
-=delete1
-=delete2
";
            Assert.IsTrue(expected.Trim().Equals(ini.ToString(),StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void Ini_WhenParseAString_ReturnsCorrectSectionAndPropertyCount()
        {
            const string ini = @"# this is a section
[SectionName]
# this is a property
propertyKey=propertyValue
# some other properties
prop1Key=prop1Val
prop2Key=prop2Val

[section2Name]
p2key=p2val
p3key=p3val";

            //var i0 = @"D:\PortableApps\#FirefoxPortable\App\AppInfo\Launcher\FirefoxPortable.ini";
            //var f = File.ReadAllText(i0);

            var i = Ini.Parse(ini);
            i.Section("sectionname").Remove("propertykey", "propertyvalue");
            Assert.AreEqual(2, i.Sections().Count());
            Assert.AreEqual(2, i.Section("sectionname").Properties().Count());
            Assert.AreEqual(2, i.Section("section2name").Properties().Count());

            //var sectionComment =
            //    from commentId in Parse.Char('#').Once()
            //    from whitespace in Parse.WhiteSpace.Many()
            //    from comment in Parse.LetterOrDigit.Many()
            //    from eol in Parse.LineEnd
            //    select comment;
            //var secComment = sectionComment.Parse(ini).ToString().Trim();
            //Assert.AreEqual("this", secComment);

            //var i1 = IniDocument.Parse(f);
            //Assert.AreEqual(10, i1.Sections().Count());
        }
    }
}
