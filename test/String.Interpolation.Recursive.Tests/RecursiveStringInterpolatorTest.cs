using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace String.Interpolation.Recursive.Tests
{
    public class RecursiveStringInterpolatorTest
    {
        [Test]
        public void ThreeLevelRecursionTest() {
            const string testString = "{a}{b}{c}";
            const string resultString = "aaa";

            var stringInterpolator = new RecursiveStringInterpolator("{","}");

            var keyValues = new Dictionary<string, string>();
            keyValues.Add("a", "a");
            keyValues.Add("b", "{a}");
            keyValues.Add("c", "{b}");

            var result = stringInterpolator.Interpolate(testString, keyValues);

            Assert.AreEqual(result, resultString);
        }

        [Test]
        public void MultiResultTest() {
            const string testString = "{a}{b}{c}";
            const string resultString = "aaaaaa";

            var stringInterpolator = new RecursiveStringInterpolator("{","}");

            var keyValues = new Dictionary<string, string>();
            keyValues.Add("a", "a");
            keyValues.Add("b", "{a}{a}");
            keyValues.Add("c", "{b}{a}");

            var result = stringInterpolator.Interpolate(testString, keyValues);

            Assert.AreEqual(result, resultString);
        }

        [Test]
        public void TagsTest() {
            IEnumerable<(string openingTag, string closingTag)>
                GetTags() {
                yield return ("{", "}");
                yield return ("${", "}");
                yield return ("(", ")");
                yield return ("$(", ")");
                yield return ("%", "%");
                yield return ("$$$", "$$$");
                yield return ("Test", "Test");
                yield return ("TestStart", "TestEnd");
                yield return ("<", ">");
                yield return ("�", "�");
            }

            const string resultString = "a";

            var keyValues = new Dictionary<string, string>();
            keyValues.Add("a", "a");

            foreach (var tags in GetTags()) {
                var testString = tags.openingTag + resultString + tags.closingTag;
                var stringInterpolator = new RecursiveStringInterpolator(tags.openingTag,tags.closingTag);
                var result = stringInterpolator.Interpolate(testString, keyValues);
                Assert.AreEqual(result, resultString);
            }
        }

        [Test]
        public void EmptyParametersThrowExceptionTest() {
            Assert.Throws<ArgumentException>(
                () => new RecursiveStringInterpolator("", "")
            );
        }

        [Test]
        public void LoopThrowsExceptionTest() {
            const string testString = "{a}";
            var stringInterpolator = new RecursiveStringInterpolator("{","}");

            var keyValues = new Dictionary<string, string>();
            keyValues.Add("a", "{a}");

            Assert.Throws<InterpolationException>(
                () => stringInterpolator.Interpolate(testString, keyValues)
            );
        }
    }
}