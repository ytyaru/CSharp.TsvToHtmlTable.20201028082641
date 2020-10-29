using System;
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;

namespace TsvToHtmlTable
{
    public abstract class AConverter
    {
        public AConverter(CommonOptions opt)
        {
            opt.SetLoggingLevels();
        }
        protected void LoadStdin()
        {
            Console.WriteLine("LoadStdin()");
        }
        abstract public string ToHtml();
    }
}
