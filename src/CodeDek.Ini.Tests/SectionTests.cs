using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeDek.Ini.Tests
{
  [TestClass]
  public class SectionTests
  {
    private Section _s;
    string _ini;

    public TestContext TestContext { get; set; }

    [TestInitialize]
    public void Setup()
    {
      _s = new Section("sec");
      _s.Add(new Property("key", "val"));
      _s.Add(new Property("key1", "val1"));
      _s.Add(new Property("kEy2", "val2"));
      _ini = _s.ToString();
      //_s.Comment.Add("This is a section comment.");
      //p.Comment.Add("This is a property comment.");
    }

    [TestMethod]
    public void Section_PropertiesRegxSearchForKeyWithNumber_ReturnsTwo()
    {
      var actual = _s.Properties(@"\d+$").ToList();
      TestContext.WriteLine($"1: {actual[0].Key} | 2: {actual[1].Key}");
      Assert.AreEqual(2, actual.Count);
    }

    [TestMethod]
    public void Section_PropertiesIgnoreCaseIsFilter_ReturnsThree()
    {
      _s.Add(new Property("key", "val1"));
      _s.Add(new Property("KEY", "val3"));
      Assert.AreEqual(3, _s.Properties(Filter.IgnoreCaseIs, "key").Count());
    }

    [TestMethod]
    public void Section_PropertiesIsFilter_ReturnsTwo()
    {
      _s.Add(new Property("key", "val1"));
      _s.Add(new Property("KEY", "val3"));
      Assert.AreEqual(2, _s.Properties(Filter.Is, "key").Count());
    }

    [TestMethod]
    public void Section_PropertiesIgnoreCaseContainsFilter_ReturnsThree()
    {
      Assert.AreEqual(3, _s.Properties(Filter.IgnoreCaseContains, "Y").Count());
    }

    [TestMethod]
    public void Section_PropertiesContainsFilter_ReturnsOne()
    {
      Assert.AreEqual(1, _s.Properties(Filter.Contains, "E").Count());
    }

    [TestMethod]
    public void Section_PropertiesIgnoreCaseEndsWithFilter_ReturnsOne()
    {
      Assert.AreEqual(1, _s.Properties(Filter.IgnoreCaseEndsWith, "Y").Count());
    }

    [TestMethod]
    public void Section_PropertiesEndsWithFilter_ReturnsOne()
    {
      Assert.AreEqual(1, _s.Properties(Filter.EndsWith, "2").Count());
    }

    [TestMethod]
    public void Section_PropertiesIgnoreCaseStartsWithFilter_ReturnsThree()
    {
      Assert.AreEqual(3, _s.Properties(Filter.IgnoreCaseStartsWith, "kEy").Count());
    }

    [TestMethod]
    public void Section_PropertiesStartsWithFilter_ReturnsOne()
    {
      Assert.AreEqual(1, _s.Properties(Filter.StartsWith, "kEy").Count());
    }

    [TestMethod]
    public void
      Section_WhenParseStringWithOneSectionAndTwoProperties_ReturnsPropertyCountOfTwoAndToStringOfSecKeyEqualsValAndKeyOneEqualsValOne()
    {
      var actual = Section.Parse(_s.ToString());
      Assert.AreEqual(3, actual.Properties().Count());
      Assert.AreEqual(_ini, actual.ToString());
    }

    [TestMethod]
    public void Section_WhenInstintiatedWithAnotherSection_CreatesAProperCopy()
    {
      var s = new Section(_s);
      Assert.AreNotEqual(s, _s);
    }

    [TestMethod]
    public void Section_WhenInstantiatedWithInvalidName_ThrowsArgumentException()
    {
      Section sec = null;
      Assert.ThrowsException<ArgumentException>(() => new Section(sec));
      Assert.ThrowsException<ArgumentException>(() => new Section(""));
    }

    [TestMethod]
    public void Section_PropertyWithArgsKeyAndIgnoreCaseTrue_ReturnsVal()
    {
      Assert.AreEqual("val", _s.Property("Key", true).Value);
    }

    [TestMethod]
    public void Section_PropertyWithArgsKeyAndIgnoreCaseFalse_ReturnsVal1()
    {
      _s.Add(new Property("Key", "val1"));
      Assert.AreEqual("val1", _s.Property("Key", false).Value);
    }

    [TestMethod]
    public void Section_FindPropertyByKey_ReturnsPropertyIfExists()
    {
      var expected = "kEy2=val2";
      Assert.AreEqual(expected, _s.Property("key2").ToString());
    }

    [TestMethod]
    public void Section_FindPropertyByKeyAndValue_ReturnsPropertyIfExists()
    {
      var expected = "kEy2=val2";
      Assert.AreEqual(expected, _s.Property("key2", "val2").ToString());
    }

    [TestMethod]
    public void Section_WhenPropertyIsPassedInRemove_PropertyIsRemovedIfFound()
    {
      var p = _s.Property("key2", "val2");
      _s.Remove(p);
      Assert.AreEqual(2, _s.Properties().Count());
    }

    [TestMethod]
    public void Section_WhenKeyAndValueIsPassedInRemove_PropertyIsRemovedIfFound()
    {
      // todo: make value optional for remove
      _s.Remove("key2", "val2");
      Assert.AreEqual(2, _s.Properties().Count());
    }

    [TestMethod]
    public void Section_AddPropertyOptionIfKeyAndValueIsUniqueOnAUniqueProperty_PropertiesCountIsFour()
    {
      _s.Add(new Property("key3", "val3"));
      Assert.AreEqual(4, _s.Properties().Count());
    }

    [TestMethod]
    public void Section_AddPropertyOptionIfKeyAndValueIsUniqueOnAUniqueValueButNotKey_PropertiesCountIsThree()
    {
      _s.Add(new Property("key2", "val3"));
      TestContext.WriteLine(_s.ToString());
      Assert.AreEqual(4, _s.Properties().Count());
    }

    [TestMethod]
    public void Section_AddPropertyOptionIfKeyIsUniqueOnAUniqueKeyProperty_PropertiesCountIsFour()
    {
      _s.Add(new Property("key3", "val3"), PropertyAddOption.KeyIsUnique);
      TestContext.WriteLine(_s.ToString());
      Assert.AreEqual(4, _s.Properties().Count());
    }

    [TestMethod]
    public void Section_AddPropertyOptionIfKeyIsUniqueOnANonUniqueKeyProperty_PropertiesCountIsThree()
    {
      _s.Add(new Property("key2", "val3"), PropertyAddOption.KeyIsUnique);
      TestContext.WriteLine(_s.ToString());
      Assert.AreEqual(3, _s.Properties().Count());
    }

    [TestMethod]
    public void Section_AddPropertyOptionUpdateValueIfPropertyExistOnUniqueProperty_PropertiesCountIsFour()
    {
      _s.Add(new Property("key4", "val4"), PropertyAddOption.Overwrite);
      TestContext.WriteLine(_s.ToString());
      Assert.AreEqual(4, _s.Properties().Count());
    }

    [TestMethod]
    public void Section_AddPropertyOptionUpdateValueIfPropertyExistOnExistingProperty_PropertiesCountIsThree()
    {
      _s.Add(new Property("key", "val++"), PropertyAddOption.Overwrite);
      TestContext.WriteLine(_s.ToString());
      Assert.AreEqual(3, _s.Properties().Count());
    }

    [TestMethod]
    public void Section_ParseEmptyString_ReturnsNullSection()
    {
      Assert.IsNull(Section.Parse(""));
    }

    [TestMethod]
    public void Section_ParseNullString_ReturnsNullSection()
    {
      Assert.IsNull(Section.Parse(null));
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