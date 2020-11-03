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
            ShowArgsNLog(opt);
        }
        public override string ToHtml()
        {
            logger.Debug("NoneHeader.ToHtml()");
            return Html.Enclose("table", this.MakeTr(), this.Options.TableAttribute);
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
        private void ShowArgsNLog(NoneHeaderOptions opt)
        {
            logger.Debug("InnerHeader.ShowArgsNLog()");
            logger.Debug("----- Arguments -----");
            logger.Debug("File                  : {}", opt.File);
            logger.Debug("Delimiter             : {}", opt.Delimiter);
            logger.Debug("TableAttribute        : {}", opt.TableAttribute);
            logger.Debug("LoggingLevel          : {}", opt.LoggingLevel);
            logger.Debug("Source                :\n{}", opt.Source);
            logger.Debug("----------");
        }
    }
}
