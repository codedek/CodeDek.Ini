using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ini.Net.Tests
{
    [TestClass]
    public class PropertyTests
    {
        private Property _p;

        // Naming Convention
        // Method under test
        // Scenario which is tested
        // Expected behavior/state/result

        [TestInitialize]
        public void Setup()
        {
            _p = new Property("Jon", "Doe");
        }

        [TestMethod]
        public void Property_WhenInstantiatedWithValidKeyAndValue_ReturnsProperToString()
        {
            var p = new Property("key", "value");
            Assert.AreEqual("key=value", p.ToString());
        }

        [TestMethod]
        public void Property_WhenPassInAnotherPropertyToConstructor_ReturnsPropertyCopy()
        {
            var p1 = new Property("key", "value");
            var p2 = new Property(p1);
            Assert.AreNotEqual(p1, p2);
        }

        [TestMethod]
        public void Property_WhenParseAValidString_ReturnsProperToString()
        {
            var expected = $@"
Key=Value";
            var p1 = Property.Parse(expected);
            Assert.AreEqual(expected.Trim(), p1.ToString());
            // parse
        }

        [TestMethod]
        public void Property_CreatedWithJonAsKey_PropertyKeyIsJon() => Assert.AreEqual("Jon", _p.Key);

        [TestMethod]
        public void Property_CreatedWithDoeAsValue_PropertyValueIsDoe() => Assert.AreEqual("Doe", _p.Value);

        [TestMethod]
        public void Property_CreatedWithJonAsKeyDoeAsValue_ToStringIsJonEqualDoe() => Assert.AreEqual("Jon=Doe", _p.ToString());

        [TestMethod]
        public void Property_CreatedWithNullKey_ThrowArgumentNullException() => Assert.ThrowsException<ArgumentException>(() => new Property(null, "Doe"));

        [TestMethod]
        public void Property_CreatedWithEmptyKey_ThrowArgumentNullException() => Assert.ThrowsException<ArgumentException>(() => new Property(string.Empty, "Doe"));

        [TestMethod]
        public void Property_CreatedWithSpaceInKeyAndNotQuoted_IsRecognizedFine()
        {
            _p = new Property("Sir Jon", "Doe");
            Assert.AreEqual("Sir Jon", _p.Key);
        }

        [TestMethod]
        public void Property_CreatedWithDoubleQuotedKeyWithSpaces_KeyIsDoubleQuoted()
        {
            _p = new Property("\"Sir Jon\"", "Doe");
            Assert.AreEqual("\"Sir Jon\"", _p.Key);
        }

        [TestMethod]
        public void Property_CreatedWithSingleQuotedKeyWithSpaces_KeyIsSingleQuoted()
        {
            _p = new Property("'Sir Jon'", "Doe");
            Assert.AreEqual("'Sir Jon'", _p.Key);
        }

        //[TestMethod]
        //public void Property_InstantiatedThroughTheConstructorAlone_PropertyIsEnableIsTrue() => Assert.IsTrue(_p.IsEnable);

        //[TestMethod]
        //public void Property_InstantiatedThroughTheConstructorAlone_PropertyCommentIsBlank() => Assert.AreEqual("", string.Join("\n", _p.Comment));

        //[TestMethod]
        //public void Property_CreatedWhereGreetingsWasAddedAsComment_CommentIsGreetings()
        //{
        //    _p.Comment.Add("Greetings");
        //    _p.Comment.Add("Earthling!");

        //    var expected = "Greetings" + Environment.NewLine + "Earthling!";
        //    var actual = string.Join(Environment.NewLine, _p.Comment);

        //    Assert.AreEqual(expected, actual);
        //    Assert.AreEqual($"# Greetings{Environment.NewLine}# Earthling!{Environment.NewLine}Jon=Doe", _p.String);

        //    _p.Comment.Clear();
        //    Assert.AreEqual("", string.Join(Environment.NewLine, _p.Comment));
        //}

        //[TestMethod]
        //public void Property_CreatedWhereGreetingsWasAddedAsComment_ToStringHasCommentAndProperty()
        //{
        //    _p.Comment.Add("Greetings");
        //    _p.Comment.Add("Earthling!");
        //}

        //[TestMethod]
        //public void Property_CreatePropertyNoModifyWithSpaceInKey_TrowInvalidArgException()
        //{
        //    Assert.ThrowsException<ArgumentException>(() => Property.CreateNoModify("Key ", "Value"));
        //}

        //[TestMethod]
        //public void Property_CreatePropertyNoModifyWithoutCommentIndicator_TrowInvalidArgException()
        //{
        //    Assert.ThrowsException<ArgumentException>(() => Property.CreateNoModify("Key ", "Value", true, "This is a comment."));
        //}

        //[TestMethod]
        //public void Property_DeConstruction()
        //{
        //    var (key, value, comment, property) = _p;

        //    Assert.AreEqual("Jon", key);
        //    Assert.AreEqual("Doe", value);
        //    Assert.AreEqual("", comment);
        //    Assert.AreEqual("Jon=Doe", property);
        //}
    }
}
