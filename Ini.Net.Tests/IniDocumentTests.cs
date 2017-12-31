using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using System.IO;

namespace Ini.Net.Tests
{
    [TestClass]
    public class IniDocumentTests
    {
        public TestContext TestContext { get; set; }

        // load
        // parse
        // read
        // write

        [TestMethod]
        public void IniDocument_WhenLoadProperIni_ReturnsCorrectToString()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "test.ini");
            var file = File.ReadAllText(path);
            var ini = IniDocument.Load(path);

            TestContext.WriteLine(ini?.ToString());
            Assert.AreEqual(file, ini?.ToString());
        }

        [TestMethod]
        public void IniDocument_WhenRead_ReturnsPropertyValue()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "test.ini");

            var result = IniDocument.Read(path, "Format", "type");
            TestContext.WriteLine(result);
            Assert.AreEqual("PortableApps.comFormat", result);
        }

        [TestMethod]
        public void IniDocument_WhenWrite_ReturnsPropertyValue()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "test.ini");
            TestContext.WriteLine(path);
            TestContext.WriteLine($"WriteResult: {IniDocument.Write(path, "sec3", "fizz", "buzz")}");

            var ini = IniDocument.Load(path);
            TestContext.WriteLine(ini.ToString());
        }

        [TestMethod]
        public void IniDocument_WhenParseAnIniStringWithTwoSectionsEachWithTwoProperties_ReturnsCorrectSectionAndPropertyCount()
        {
            const string ini = @"# this is a section
[SectionName]
# this is a property
prop1Key=prop1Val
prop2Key=prop2Val

[section2Name]
p2key=p2val
p3key=p3val";

            var i = IniDocument.Parse(ini);
            Assert.AreEqual(2, i.Sections().Count());
            Assert.AreEqual(2, i.Section("sectionname").Properties().Count());
            Assert.AreEqual(2, i.Section("section2name").Properties().Count());
        }

//        [TestMethod]
//        public void IniDocument_WhenSerializeObject_ReturnsIniString()
//        {
//            var ini = @"
//[Person]
//name=jon
//age=6";
//            var p = new Person();
//            p.Parent = new Parent { Name = "Jon", Age = 6 };
//            p.Child = new Child { Name = "Phil", Age = 2 };

//            var json = JsonConvert.SerializeObject(p);
//            TestContext.WriteLine(json);
//            var p1 = JsonConvert.DeserializeObject<Person>(json);
//            TestContext.WriteLine(p1.ToString());
//        }
    }

    //public class Person
    //{
    //    public Parent Parent { get; set; }
    //    public Child Child { get; set; }

    //    public override string ToString()
    //    {
    //        return $"{Parent.ToString()}{Environment.NewLine}{Environment.NewLine}{Child.ToString()}";
    //    }
    //}

    //public class Parent
    //{
    //    public string Name { get; set; }
    //    public int Age { get; set; }
    //    public override string ToString()
    //    {
    //        return $"[{nameof(Parent)}]{Environment.NewLine}{nameof(Name)}={Name}{Environment.NewLine}{nameof(Age)}={Age}";
    //    }
    //}
    //public class Child
    //{
    //    public string Name { get; set; }
    //    public int Age { get; set; }
    //    public override string ToString()
    //    {
    //        return $"[{nameof(Child)}]{Environment.NewLine}{nameof(Name)}={Name}{Environment.NewLine}{nameof(Age)}={Age}";
    //    }
    //}
}
