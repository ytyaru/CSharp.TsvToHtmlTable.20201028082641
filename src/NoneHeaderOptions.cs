using System;
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;

namespace TsvToHtmlTable
{
    [Verb("n", HelpText = "ヘッダがない。" )]
    public class NoneHeaderOptions : CommonOptions
    {
    }
}
