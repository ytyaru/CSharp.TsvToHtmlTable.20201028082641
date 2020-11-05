using NUnit.Framework;
using System;
using System.Text;
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;
using NLog;
using TsvToHtmlTable;

namespace TsvToHtmlTable.Tests
{
    [TestFixture]
    public class HtmlTest
    {
        /*
        [Test]
        public void TestElementOnly()
        {
            Assert.AreEqual(@"<html></html>", Html.Enclose("html"));
        }
        */
//        [TestCase(null, string.Empty)]
//        [TestCase(string.Empty, string.Empty)]
        [TestCase("", "")]
        [TestCase(null!, "")]
        [TestCase(@"br", @"<br />")]
        public void TestEncloseElement(string element, string expected)
        {
            Assert.AreEqual(expected, Html.Enclose(element));
        }
        [TestCase("", "", "")]
        [TestCase(null!, "", "")]
        [TestCase("", null!, "")]
        [TestCase(null!, null!, "")]
        [TestCase(@"html", "", @"<html></html>")]
        [TestCase(@"p", "some text.", @"<p>some text.</p>")]
        public void TestEncloseElementText(string element, string text, string expected)
        {
            Assert.AreEqual(expected, Html.Enclose(element, text));
        }
    }
}
