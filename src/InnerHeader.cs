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
            logger.Debug("InnerHeader.ToHtml()");
            var tr = this.Options.Header switch
            {
                InnerHeaderType.c => MakeColumn(),
                InnerHeaderType.r => MakeRow(),
                _ => MakeColumn(),
            };
            return Html.Enclose("table", this.Options.TableAttribute, tr);
        }
        private string MakeRow()
        {
            logger.Debug("MakeRow()");
            var tr = new StringBuilder();
            var td = new StringBuilder();
            for (int r=0; r<this.Options.SourceList.Count; r++)
            {
                td.Clear();
                bool isHeader = IsHeader(r);
                for (int c=0; c<this.Options.SourceList[r].Count; c++)
                {
                    if (isHeader) {
                        td.Append(Html.Enclose("th", this.Options.HeaderAttribute, this.Options.SourceList[r][c].Text));
                    } else {
                        td.Append(Html.Enclose("td", this.Options.SourceList[r][c].Text));
                    }
                }
                tr.Append(Html.Enclose("tr", td.ToString()));
            }
            return tr.ToString();
        }
        private string MakeColumn()
        {
            logger.Debug("MakeColumn()");
            var tr = new StringBuilder();
            var td = new StringBuilder();
            foreach (var row in this.Options.SourceList)
            {
                td.Clear();
                for (int c=0; c<row.Count; c++)
                {
                    if (IsHeader(c)) {
                        td.Append(Html.Enclose("th", this.Options.HeaderAttribute, row[c].Text));
                    } else {
                        td.Append(Html.Enclose("td", row[c].Text));
                    }
                }
                tr.Append(Html.Enclose("tr", td.ToString()));
            }
            return tr.ToString();
        }
        private bool IsHeader(int idx)
        {
//            logger.Trace("{} {} {} {}", idx, this.Options.Start, this.Options.Step, ((idx + 1 - this.Options.Start) % this.Options.Step));
            if (0 == ((idx + 1 - this.Options.Start) % (this.Options.Step + 1))) { return true; }
            else { return false; }
        }
        private void ShowArgsNLog(InnerHeaderOptions opt)
        {
            logger.Debug("InnerHeader.ShowArgsNLog()");
            logger.Debug("----- Arguments -----");
            logger.Debug("Header                : {}", opt.Header);
            logger.Debug("Start                 : {}", opt.Start);
            logger.Debug("Step                  : {}", opt.Step);
            logger.Debug("HeaderAttribute       : {}", opt.HeaderAttribute);
            logger.Debug("File                  : {}", opt.File);
            logger.Debug("Delimiter             : {}", opt.Delimiter);
            logger.Debug("TableAttribute        : {}", opt.TableAttribute);
            logger.Debug("LoggingLevel          : {}", opt.LoggingLevel);
            logger.Debug("Source                :\n{}", opt.Source);
            logger.Debug("----------");
        }
    }
}
