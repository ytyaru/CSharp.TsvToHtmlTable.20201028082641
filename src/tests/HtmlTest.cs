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
        [Test]
        public void Test()
        {
            Assert.AreEqual(@"<html></html>", Html.Enclose("html"));
        }
    }
}
