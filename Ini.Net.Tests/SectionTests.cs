using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Ini.Net.Tests
{
    [TestClass]
    public class SectionTests
    {
        private Section _s;

        public TestContext TestContext { get; set; }

        // ctor1
        // ctor2
        // remove
        // property

        // properties
        // add
        // parse

        [TestInitialize]
        public void Setup()
        {
            _s = new Section("NewSection");
            //_s.Comment.Add("This is a section comment.");
            var p = new Property("Jon", "Doe");
            //p.Comment.Add("This is a property comment.");
            _s.Add(p);
        }

        [TestMethod]
        public void Section_WhenParseAStringWithOneSectionAndTwoProperties_ReturnsOneSectionAndTwoPropertiesCount()
        {
            var ini = $"[section]{Environment.NewLine}key=val{Environment.NewLine}key1=val1";
            var actual = Section.Parse(ini);
            Assert.AreEqual(2, actual.Properties().Count());
            Assert.AreEqual(ini, actual.ToString());
        }

        [TestMethod]
        public void Section_WhenInstantiatedWithValidName_ReturnsProperToString()
        {
            var s = new Section("sec");
            Assert.AreEqual("[sec]", s.ToString());
        }

        [TestMethod]
        public void Section_WhenPassInAnotherSection_MakesAProperCopy()
        {
            var s1 = new Section("hello");
            var s2 = new Section(s1);
            Assert.AreNotEqual(s1, s2);
        }

        [TestMethod]
        public void Section_WhenInstantiatedWithInvalidName_ThrowsArgumentException()
        {
            Section sec = null;
            Assert.ThrowsException<ArgumentException>(() => new Section(sec));
            Assert.ThrowsException<ArgumentException>(() => new Section(""));
        }

        [TestMethod]
        public void Section_WhenPassInAKeyToProperty_ReturnsTheAppropriatePropertyIfExists()
        {
            var p1 = new Property("key", "value");
            var p2 = new Property("key", "value");
            var p3 = new Property("key2", "value2");
            var s = new Section("sec1");
            s.Add(p1);
            s.Add(p2);
            s.Add(p3);
            var expected = "key2=value2";
            Assert.AreEqual(expected, s.Property("key2").ToString());
        }

        [TestMethod]
        public void Section_OnlyAddsUniquePropertiesToSection_PropertiesCountIsAccurate()
        {
            var p1 = new Property("key", "value");
            var p2 = new Property("key", "value");
            var p3 = new Property("key2", "value2");
            var s = new Section("sec1");
            s.Add(p1);
            s.Add(p2);
            s.Add(p3);
            Assert.AreEqual(2, s.Properties().Count());
        }

        [TestMethod]
        public void Section_WhenPropertyIsGivenForRemove_PropertyIsRemovedIfFound()
        {
            var p1 = new Property("key", "value");
            var p2 = new Property("key1", "value1");
            var p3 = new Property("key2", "value2");
            var s = new Section("sec1");
            s.Add(p1);
            s.Add(p2);
            s.Add(p3);
            s.Remove(p1);
            Assert.AreEqual(2, s.Properties().Count());
        }

        [TestMethod]
        public void Section_WhenKeyAndValueIsGivenForRemove_PropertyIsRemovedIfFoundWithSameKeyAndValue()
        {
            var p1 = new Property("key", "value");
            var p2 = new Property("key1", "value1");
            var p3 = new Property("key2", "value2");
            var s = new Section("sec1");
            s.Add(p1);
            s.Add(p2);
            s.Add(p3);
            s.Remove("key1", "value1");
            Assert.AreEqual(2, s.Properties().Count());
        }

        [TestMethod]
        public void Section_CanOnlyAddUniquePropertiesToASection_ToStringOutputIsAccurate()
        {
            var s = new Section("SectionOne");
            var p1 = new Property("Key", "Value");
            var p2 = new Property("Key", "Value3");
            var p3 = new Property("Key", "value3");

            s.Add(p1);
            s.Add(p2);
            s.Add(p3);
            s.Property("key").Value = "Value2";

            var expected = $@"
[SectionOne]
Key=Value2
Key=Value3";
            Assert.AreEqual(expected.Trim(), s.ToString());
        }

        [TestMethod]
        public void Section_GivenAValidSectionStringToParse_ReturnsCorrectToString()
        {
            var expected = $@"
[SectionOne]
-=del1
-=del2
";
            Assert.AreEqual(expected.Trim(), Section.Parse(expected).ToString());
        }

        [TestMethod]
        public void Section_WhenCreate_StringReturnsSection()
        {
            var expected = $""
                           //+ $"# This is a section comment.{Environment.NewLine}"
                           + $"[NewSection]{Environment.NewLine}"
                           //+ $"# This is a property comment.{Environment.NewLine}"
                           + "Jon=Doe";
            Assert.AreEqual(expected, _s.ToString());
        }

        //[TestMethod]
        //public void Section_WhenCreate_StringWithCommentsRemoved()
        //{
        //    var expected = $"[NewSection]{Environment.NewLine}"
        //                 + "Jon=Doe";
        //    _s.Comment.Clear();
        //    _s.Properties.ToList().ForEach(p => p.Comment.Clear());
        //    Assert.AreEqual(expected, _s.String);
        //}

        [TestMethod]
        public void Section_WhenAddProperty_StringReturnsProperty()
        {
            Assert.AreEqual($"Jon=Doe", _s.Property("Jon").ToString());
        }

        //[TestMethod]
        //public void Section_WhenAddDisabledPropertyThenDisableAndEnableSection_PropertyIsStillDisabled()
        //{
        //    // prerequisites
        //    _s.Comment.Clear();
        //    _s.Properties.ToList().ForEach(p => p.Comment.Clear());
        //    _s.Properties.Remove(_s.Property("Jon"));

        //    // create disabled property
        //    var disabledProperty = new Property("enabled", "false");
        //    disabledProperty.IsEnable = false;
        //    var expected = "#disabledProperty: enabled=false";

        //    // add it to enabled section then disable section
        //    _s.Properties.Add(disabledProperty);
        //    _s.IsEnabled = false;
        //    var expected2 = $"#disabledSection: [NewSection]{Environment.NewLine}#disabledSection: {expected}";
        //    Assert.AreEqual(expected2, _s.String);

        //    // re - enable the section
        //    _s.IsEnabled = true;
        //    var expected3 = $"[NewSection]{Environment.NewLine}{expected}";
        //    Assert.AreEqual(expected3, _s.String);
        //}
    }
}