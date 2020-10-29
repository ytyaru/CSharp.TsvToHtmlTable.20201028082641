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
    public class InnerHeader : AConverter
    {
        private Logger logger = NLog.LogManager.GetLogger("AppDefaultLogger");
        public InnerHeaderOptions Options { get; private set; }
        public InnerHeader(InnerHeaderOptions opt):base(opt)
        {
            this.Options = opt;
            ShowArgsNLog(opt);
        }
        public override string ToHtml()
        {
            Console.WriteLine("InnerHeader.ToHtml()");
            var tr = this.Options.Header switch
            {
                InnerHeaderType.c => MakeColumn(),
                InnerHeaderType.r => MakeRow(),
                _ => MakeColumn(),
            };
            return Html.Enclose("table", tr);
        }
        private string MakeRow()
        {
            logger.Debug("MakeRow");
            return "<>";
        }
        private string MakeColumn()
        {
            var tr = new StringBuilder();
            var td = new StringBuilder();
            foreach (var row in this.Options.SourceList)
            {
                td.Clear();
                for (int c=0; c<row.Count; c++)
//                foreach (var cell in row)
                {
                    td.Append(Html.Enclose((IsColumnHeader(c)) ? "th" : "td", row[c].Text));
                }
                tr.Append(Html.Enclose("tr", td.ToString()));
            }
            return tr.ToString();
        }
        private bool IsColumnHeader(int idx)
        {
            if (0 == ((idx + 1 - this.Options.Start) % this.Options.Step)) { return true; }
            else { return false; }
        }

        private string MakeTr()
        {
            return "";
        }
        private void ShowArgsNLog(InnerHeaderOptions opt)
        {
            logger.Debug("InnerHeader.ShowArgsNLog()");
            logger.Fatal("Fatal");
            logger.Error("Error");
            logger.Warn("Warn");
            logger.Info("Info");
            logger.Debug("Debug");
            logger.Trace("Trace");
            logger.Debug("----- Arguments -----");
            logger.Debug("Header                : {}", opt.Header);
            logger.Debug("Start                 : {}", opt.Start);
            logger.Debug("Step                  : {}", opt.Step);
            logger.Debug("HeaderAttributes      : {}", opt.HeaderAttributes);
            logger.Debug("File                  : {}", opt.File);
            logger.Debug("Delimiter             : {}", opt.Delimiter);
            logger.Debug("TableAttributes       : {}", opt.TableAttributes);
            logger.Debug("LoggingLevel          : {}", opt.LoggingLevel);
            logger.Debug("Source                :\n{}", opt.Source);
            logger.Debug("----------");
        }
    }
}
