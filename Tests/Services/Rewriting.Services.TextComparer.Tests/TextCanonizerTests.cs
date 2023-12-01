using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.TextComparer.Tests
{
    [TestFixture]
    public class TextCanonizerTests
    {
        private ITextCanonizer _textCanonizer;

        [SetUp]
        public void SetUp()
        {
            _textCanonizer = new TextCanonizer();
        }

        [TestCase("abcd abcd abcd", new[] { "abcd", "abcd", "abcd" })]
        [TestCase("abcd", new[] { "abcd" })]
        [TestCase("abcd\nabcd\nabcd", new[] { "abcd", "abcd", "abcd" })]
        [TestCase("abcd\tabcd\tabcd", new[] { "abcd", "abcd", "abcd" })]
        [TestCase("abcd ", new[] { "abcd" })]
        [TestCase("abcd  ", new[] { "abcd" })]
        [TestCase(" abcd", new[] { "abcd" })]
        [TestCase("  abcd", new[] { "abcd" })]
        public void Canonize_Split_Test(string text, string[] expected)
        {
            var actual = _textCanonizer.Canonize(text);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestCase("abcd, abcd. abcd!", new[] { "abcd", "abcd", "abcd" })]
        [TestCase("ab-cd-abcd - abcd", new[] { "abcdabcd", "abcd" })]
        [TestCase("abcd ------- abcd", new[] { "abcd", "abcd" })]
        [TestCase("abcd!@#$%^&&*()_+=-\"'`~.,", new[] { "abcd" })]
        public void Canonize_Punctuation_Test(string text, string[] expected)
        {
            var actual = _textCanonizer.Canonize(text);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestCase("abcd1234567890", new[] { "abcd" })]
        [TestCase("abcd 1234567890", new[] { "abcd" })]
        [TestCase("1234567890 abcd", new[] { "abcd" })]
        public void Canonize_Numbers_Test(string text, string[] expected)
        {
            var actual = _textCanonizer.Canonize(text);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestCase("ABCD", new[] { "abcd" })]
        [TestCase("aBcD", new[] { "abcd" })]
        public void Canonize_Casing_Test(string text, string[] expected)
        {
            var actual = _textCanonizer.Canonize(text);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestCase("a ab abc abcd", new[] { "abcd" })]
        [TestCase("abcd abcde", new[] { "abcd", "abcde" })]
        public void Canonize_SmallWords_Test(string text, string[] expected)
        {
            var actual = _textCanonizer.Canonize(text);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
