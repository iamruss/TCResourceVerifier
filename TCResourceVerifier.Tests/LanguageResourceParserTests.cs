using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TCResourceVerifier.Extensions;

// ReSharper disable InconsistentNaming

namespace TCResourceVerifier.Tests
{
    [TestFixture]
    public class LanguageResourceParserTests
    {
        [TestCase(TokenTestCases.Tokens_0, 0)]
        [TestCase(TokenTestCases.Tokens_1, 1)]
        [TestCase(TokenTestCases.Tokens_2, 2)]
        [TestCase(TokenTestCases.Tokens_3, 3)]
        [TestCase(TokenTestCases.Tokens_33, 3)]
        [TestCase(TokenTestCases.CurlyTokens_0, 0)]
        [TestCase(TokenTestCases.CurlyTokens_1, 1)]
        [TestCase(TokenTestCases.CurlyTokens_2, 2)]
        public void ReturnsProperDistinctCollection(string content, int count)
        {
            IEnumerable<string> tokens = content.ParseLanguageToken();

            Assert.AreEqual(count, tokens.Count());
        }

        [TestCase(TokenTestCases.Tokens_1, "Token_1")]
        [TestCase(TokenTestCases.Tokens_2, "Token_2")]
        [TestCase(TokenTestCases.Tokens_3, "Token_3")]
        [TestCase(TokenTestCases.Tokens_33, "Token_3")]
        public void ReturnsProperTokenCollection(string content, string token)
        {
            IEnumerable<string> tokens = content.ParseLanguageToken();

            Assert.AreEqual(token, tokens.OrderBy(a=>a).Last());
        }


        [TestCase(TokenTestCases.Tokens_1, new []{ "Token_1"})]
        [TestCase(TokenTestCases.Tokens_2, new []{ "Token_1", "Token_2"})]
        [TestCase(TokenTestCases.Tokens_3, new[] { "Token_1",  "Token_2", "Token_3" })]
        [TestCase(TokenTestCases.Tokens_33, new []{ "Token_1",  "Token_2", "Token_3"})]
        [TestCase(TokenTestCases.CurlyTokens_1, new[] { "Blogs_AboutBlog_Name1" })]
        [TestCase(TokenTestCases.CurlyTokens_2, new[] { "Blogs_AboutBlog_Name1", "Blogs_AboutBlog_Name2" })]
        public void ReturnsProperTokenCollection(string content, string[] expectedTokens)
        {
            IEnumerable<string> tokens = content.ParseLanguageToken();

            Assert.IsTrue(new HashSet<string>(tokens).SetEquals(expectedTokens));
        }
    }
}

// ReSharper restore InconsistentNaming
