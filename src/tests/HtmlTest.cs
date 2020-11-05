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

        [TestCase("", "", "", "")]
        [TestCase(null!, "", "", "")]
        [TestCase("", null!, "", "")]
        [TestCase("", "", null!, "")]
        [TestCase("td", "rowspan=\"2\"", "some text.", "<td rowspan=\"2\">some text.</td>")]
        [TestCase("",   "rowspan=\"2\"", "some text.", "")]
        [TestCase("td", "rowspan=\"2\" colspan=\"3\"", "some text.", "<td rowspan=\"2\" colspan=\"3\">some text.</td>")]
        [TestCase("td", "", "some text.", "<td>some text.</td>")]
        [TestCase("td", "", "", "<td></td>")]
        [TestCase("td", "rowspan=\"2\"", "", "<td rowspan=\"2\"></td>")]
        [TestCase("input", "type=\"text\"", null!, "<input type=\"text\" />")]
        public void TestEncloseElementAttrText(string element, string attr, string text, string expected)
        {
            Assert.AreEqual(expected, Html.Enclose(element, attr, text));
        }
    }
}
