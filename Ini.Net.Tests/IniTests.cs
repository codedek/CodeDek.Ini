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
        Ini _i;
        string _ini;

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Setup()
        {
            _i = new Ini();
            var s = new Section("sec");
            s.Add(new Property("key1", "val1"));
            s.Add(new Property("key2", "val2"));
            _i.Add(s);

            var s2 = new Section("del");
            s2.Add(new Property("-", "foo"));
            s2.Add(new Property("-", "bar"));
            _i.Add(s2);
            _ini = _i.ToString();
        }

        [TestMethod]
        public void Ini_SectionWithArgsNameDelIgnoreCaseTrue_IsNotNullReturnsTrue()
        {
            Assert.IsNotNull(_i.Section("DEL",true));
        }

        [TestMethod]
        public void Ini_SectionWithArgsNameDelIgnoreCaseFalse_IsNullReturnsTrue()
        {
            Assert.IsNull(_i.Section("DEL", false));
        }

        [TestMethod]
        public void Ini_SectionsIgnoreCaseIsFilter_ReturnsThree()
        {
            var s = new Section("SEC");
            s.Add(new Property("key1", "val1"));
            s.Add(new Property("key2", "val2"));
            _i.Add(s);
            Assert.AreEqual(1, _i.Sections(Filter.IgnoreCaseIs, "sec").Count());
        }

        [TestMethod]
        public void Ini_SectionsIsFilter_ReturnsOne()
        {
            var s = new Section("SEC");
            s.Add(new Property("key1", "val1"));
            s.Add(new Property("key2", "val2"));
            _i.Add(s);
            Assert.AreEqual(1, _i.Sections(Filter.Is, "sec").Count());
        }

        [TestMethod]
        public void Ini_SectionsIgnoreCaseContainsFilter_ReturnsTwo()
        {
            var s = new Section("SEC2");
            s.Add(new Property("key1", "val1"));
            s.Add(new Property("key2", "val2"));
            _i.Add(s);
            Assert.AreEqual(2, _i.Sections(Filter.IgnoreCaseContains, "sec").Count());
        }

        [TestMethod]
        public void Ini_SectionsContainsFilter_ReturnsOne()
        {
            var s = new Section("SEC2");
            s.Add(new Property("key1", "val1"));
            s.Add(new Property("key2", "val2"));
            _i.Add(s);
            Assert.AreEqual(1, _i.Sections(Filter.Contains, "sec").Count());
        }

        [TestMethod]
        public void Ini_SectionsIgnoreCaseEndsWithFilter_ReturnsTwo()
        {
            var s = new Section("DeC");
            s.Add(new Property("key1", "val1"));
            s.Add(new Property("key2", "val2"));
            _i.Add(s);
            Assert.AreEqual(2, _i.Sections(Filter.IgnoreCaseEndsWith, "c").Count());
        }

        [TestMethod]
        public void Ini_SectionsEndsWithFilter_ReturnsOne()
        {
            var s = new Section("DeC");
            s.Add(new Property("key1", "val1"));
            s.Add(new Property("key2", "val2"));
            _i.Add(s);
            Assert.AreEqual(1, _i.Sections(Filter.EndsWith, "c").Count());
        }

        [TestMethod]
        public void Ini_SectionsIgnoreCaseStartsWithFilter_ReturnsTwo()
        {
            var s = new Section("SEC2");
            s.Add(new Property("key1", "val1"));
            s.Add(new Property("key2", "val2"));
            _i.Add(s);
            Assert.AreEqual(2, _i.Sections(Filter.IgnoreCaseStartsWith, "sec").Count());
        }

        [TestMethod]
        public void Ini_SectionsStartsWithFilter_ReturnsOne()
        {
            var s = new Section("SEC2");
            s.Add(new Property("key1", "val1"));
            s.Add(new Property("key2", "val2"));
            _i.Add(s);
            Assert.AreEqual(1, _i.Sections(Filter.StartsWith, "sec").Count());
        }

        [TestMethod]
        public void Ini_WhenInstantiatedWithAnotherIni_CreatesACopyOfThatIni()
        {
            var i = new Ini(_i);
            Assert.AreNotEqual(i, _i);
        }

        [TestMethod]
        public void
            Ini_WhenParseAnIniStringWithTwoSectionsEachWithTwoProperties_ReturnsSectionsCountOfTwoAndEachPropertiesCountOfTwo()
        {
            var i = Ini.Parse(_ini);
            Assert.AreEqual(2, i.Sections().Count());
            Assert.AreEqual(2, i.Section("sec").Properties().Count());
            Assert.AreEqual(2, i.Section("del").Properties().Count());
        }

        [TestMethod]
        public void Ini_WhenParseAnIniStringWithTwoSectionsEachWithTwoProperties_ReturnsCorrectToString()
        {
            TestContext.WriteLine(_ini);
            Assert.AreEqual(_ini, _i.ToString());
        }

        [TestMethod]
        public void Ini_WhenAddSectionOptionIfNameIsUniqueSetOnANewSectionWithUniqueName_SectionsCountIsThree()
        {
            _i.Add(new Section("unique"));
            TestContext.WriteLine(_i.ToString());
            Assert.AreEqual(3, _i.Sections().Count());
        }

        [TestMethod]
        public void Ini_WhenAddSectionOptionIfNameIsUniqueSetOnANewSectionWithExistingName_SectionsCountIsTwo()
        {
            _i.Add(new Section("sec"));
            TestContext.WriteLine(_i.ToString());
            Assert.AreEqual(2, _i.Sections().Count());
        }

        [TestMethod]
        public void Ini_WhenAddSectionOptionOverwriteExistingSetOnANewSectionWithUniqueName_SectionsCountIsThree()
        {
            _i.Add(new Section("unique"), AddSection.OverwriteExisting);
            TestContext.WriteLine(_i.ToString());
            Assert.AreEqual(3, _i.Sections().Count());
        }

        [TestMethod]
        public void
            Ini_WhenAddSectionOptionOverwriteExistingSetOnANewSectionWithExistingName_TheExistingSectionIsOverwrittenWithTheNewSectionAndSectionsCountIsTwo()
        {
            _i.Add(new Section("sec"), AddSection.OverwriteExisting);
            TestContext.WriteLine(_i.ToString());
            Assert.AreEqual(2, _i.Sections().Count());
            Assert.AreEqual(0, _i.Section("sec").Properties().Count());
        }

        [TestMethod]
        public void Ini_WhenAddSectionOptionMergeWithExistingSetOnANewSectionWithUniqueName_SectionsCountIsThree()
        {
            _i.Add(new Section("unique"), AddSection.MergeWithExisting);
            TestContext.WriteLine(_i.ToString());
            Assert.AreEqual(3, _i.Sections().Count());
        }

        [TestMethod]
        public void
            Ini_WhenAddSectionOptionMergeWithExistingSetOnANewSectionWithExistingName_TheUniquePropertiesFromTheNewSectionIsAddedToTheExistingSectionAndSectionsCountIsTwo()
        {
            var s = new Section("sec");
            s.Add(new Property("key1", "val1"));
            s.Add(new Property("key1", "happy new year"));
            s.Add(new Property("add", "me"));
            _i.Add(s, AddSection.MergeWithExisting);
            TestContext.WriteLine(_i.ToString());

            Assert.AreEqual(2, _i.Sections().Count());
            Assert.AreEqual(4, _i.Section("sec").Properties().Count());
        }

        [TestMethod]
        public void Ini_WhenAddSectionOptionMergeAndUpdateExistingSetOnANewSectionWithUniqueName_SectionsCountIsThree()
        {
            _i.Add(new Section("unique"), AddSection.MergeAndUpdateExisting);
            TestContext.WriteLine(_i.ToString());
            Assert.AreEqual(3, _i.Sections().Count());
        }

        [TestMethod]
        public void
            Ini_WhenAddSectionOptionMergeAndUpdateExistingSetOnANewSectionWithExistingName_TheUniquePropertiesFromTheNewSectionIsAddedToTheExistingSectionTheExistingPropertiesValuesAreUpdatedAndSectionsCountIsTwo()
        {
            var s = new Section("sec");
            s.Add(new Property("key1", "val1"));
            s.Add(new Property("key1", "happy new year"));
            s.Add(new Property("add", "me"));
            _i.Add(s, AddSection.MergeAndUpdateExisting);
            TestContext.WriteLine(_i.ToString());

            Assert.AreEqual(2, _i.Sections().Count());
            Assert.AreEqual(3, _i.Section("sec").Properties().Count());
            Assert.AreEqual("happy new year", _i.Section("sec").Property("key1").Value);
        }
    }
}