using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeDek.Ini.Tests
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
            _p = new Property("key", "val");
        }

        [TestMethod]
        public void Property_WhenInstantiatedWithKeyAndVal_ToStringReturnsKeyEqualsVal()
        {
            Assert.AreEqual("key=val", _p.ToString());
        }

        [TestMethod]
        public void Property_WhenInstantiatedWithAnotherProperty_ReturnsCopyOfThatProperty()
        {
            var p = new Property(_p);
            Assert.AreNotEqual(p, _p);
        }

        [TestMethod]
        public void Property_WhenInstantiatedWithKeyAndBlankValue_ToStringReturnsKeyEquals()
        {
            var p = new Property("key","");
            Assert.AreEqual("key=", p.ToString());
        }

        [TestMethod]
        public void Property_WhenInstantiatedWithKeyAndNullValue_ToStringReturnsKeyEquals()
        {
            var p = new Property("key",null);
            Assert.AreEqual("key=", p.ToString());
        }

        [TestMethod]
        public void Property_WhenParseStringWithKeyEqualsVal_ToStringReturnsKeyEqualsVal()
        {
            var expected = $"key=val";
            var p = Property.Parse(expected);
            Assert.AreEqual(expected.Trim(), p.ToString());
        }

        [TestMethod]
        public void Property_CreatedWithNullKey_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentException>(() => new Property(null, "val"));
        }

        [TestMethod]
        public void Property_CreatedWithEmptyKey_ThrowArgumentNullException()
        {
            Assert.ThrowsException<ArgumentException>(() => new Property(string.Empty, "val"));
        }

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
