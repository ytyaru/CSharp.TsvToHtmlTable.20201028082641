using System;
using System.Text;
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;
using NLog;

namespace TsvToHtmlTable
{
    public class NoneHeader : AConverter
    {
        private Logger logger = NLog.LogManager.GetLogger("AppDefaultLogger");
        public NoneHeaderOptions Options { get; private set; }
        public NoneHeader(NoneHeaderOptions opt):base(opt)
        {
            this.Options = opt;
            ShowArgsConsole(opt);
            ShowArgsNLog(opt);
        }
        public override string ToHtml()
        {
            Console.WriteLine("NoneHeader.ToHtml()");
//            return "<table></table>";
            return Html.Enclose("table", this.MakeTr());
        }
        private string MakeTr()
        {
            var td = new StringBuilder();
            var tr = new StringBuilder();
            foreach (var row in this.Options.SourceList)
            {
                td.Clear();
                foreach (var cell in row)
                {
                    td.Append(Html.Enclose("td", cell.Text));
                }
                tr.Append(Html.Enclose("tr", td.ToString()));
            }
            return tr.ToString();
        }
        private void ShowArgsConsole(NoneHeaderOptions opt)
        {
            Console.WriteLine("NoneHeader");
            Console.WriteLine("----- Arguments -----");
            Console.WriteLine($"File: {opt.File }");
            Console.WriteLine($"Delimiter: {opt.Delimiter}");
            Console.WriteLine($"TableAttributes: {opt.TableAttributes}");
            Console.WriteLine($"LoggingLevel: {opt.LoggingLevel}");
            Console.WriteLine("----------");
        }
        private void ShowArgsNLog(NoneHeaderOptions opt)
        {
            logger.Debug("InnerHeader.ShowArgsNLog()");
            logger.Fatal("Fatal");
            logger.Error("Error");
            logger.Warn("Warn");
            logger.Info("Info");
            logger.Debug("Debug");
            logger.Trace("Trace");
            logger.Debug("----- Arguments -----");
            logger.Debug("File                  : {}", opt.File);
            logger.Debug("Delimiter             : {}", opt.Delimiter);
            logger.Debug("TableAttributes       : {}", opt.TableAttributes);
            logger.Debug("LoggingLevel          : {}", opt.LoggingLevel);
            logger.Debug("Source                :\n{}", opt.Source);
            logger.Debug("----------");
        }

    }
}
