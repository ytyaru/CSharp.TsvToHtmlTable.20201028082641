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

        [TestCase("", "", "")]
        [TestCase(null!, null!, "")]
        [TestCase("", null!, "")]
        [TestCase(null!, "", "")]
        [TestCase("class", "tsv2table", "class=\"tsv2table\"")]
        [TestCase("class", "", "")]
        [TestCase("", "tsv2table", "")]
        public void TestMakeAttrString(string key, string value, string expected)
        {
            Assert.AreEqual(expected, Html.MakeAttr(key, value));
        }

        [TestCaseSourceAttribute("MakeAttrPairCases")]
        public void TestMakeAttrPair(KeyValuePair<string,string> pair, string expected)
        {
            Assert.AreEqual(expected, Html.MakeAttr(pair));
        }
        private static readonly object[] MakeAttrPairCases = {
            new object[] { new KeyValuePair<string,string>("", ""), ""},
            new object[] { new KeyValuePair<string,string>(null!, null!), ""},
            new object[] { new KeyValuePair<string,string>("", null!), ""},
            new object[] { new KeyValuePair<string,string>(null!, ""), ""},
            new object[] { new KeyValuePair<string,string>("class", "tsv2table"), "class=\"tsv2table\"" },
            new object[] { new KeyValuePair<string,string>("class", ""), ""},
            new object[] { new KeyValuePair<string,string>("", "tsv2table"), ""}
        };

        [TestCaseSourceAttribute("MakeAttrsStringCases")]
        public void TestMakeAttrsString(string[] keyvalues, string expected)
        {
            Assert.AreEqual(expected, Html.MakeAttrs(keyvalues));
        }
        private static readonly object[] MakeAttrsStringCases = {
            new object[] { new string[] { "", "" }, ""},
            new object[] { new string[] { null!, null! }, ""},
            new object[] { new string[] { "", null! }, ""},
            new object[] { new string[] { null!, "" }, ""},
            new object[] { new string[] { "class", "tsv2table" }, "class=\"tsv2table\""},
            new object[] { new string[] { "class", "" }, ""},
            new object[] { new string[] { "", "tsv2table" }, ""},
        };
        [TestCaseSourceAttribute("MakeAttrsListCases")]
        public void TestMakeAttrsList(List<KeyValuePair<string,string>> pairs, string expected)
        {
            Assert.AreEqual(expected, Html.MakeAttrs(pairs));
        }
        private static readonly object[] MakeAttrsListCases = {
            new object[] { new List<KeyValuePair<string,string>> { new KeyValuePair<string,string>("","") }, ""},
            new object[] { new List<KeyValuePair<string,string>> { new KeyValuePair<string,string>(null!,null!) }, ""},
            new object[] { new List<KeyValuePair<string,string>> { new KeyValuePair<string,string>("",null!) }, ""},
            new object[] { new List<KeyValuePair<string,string>> { new KeyValuePair<string,string>(null!,"") }, ""},
            new object[] { new List<KeyValuePair<string,string>> { new KeyValuePair<string,string>("class","tsv2table") }, "class=\"tsv2table\""},
            new object[] { new List<KeyValuePair<string,string>> { new KeyValuePair<string,string>("class","") }, ""},
            new object[] { new List<KeyValuePair<string,string>> { new KeyValuePair<string,string>("","tsv2table") }, ""}
        };
        //[TestCase(KeyValuePair<string,string>{Key="",Value=""}, "")]
    }
}
