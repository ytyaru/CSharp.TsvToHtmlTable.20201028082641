using System;
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;
using NLog;
namespace TsvToHtmlTable
{
    public class GroupHeader : AConverter
    {
//        private Logger logger = LogManager.GetCurrentClassLogger();
        private Logger logger = NLog.LogManager.GetLogger("AppDefaultLogger");
        public GroupHeader(GroupHeaderOptions opt):base()
        {
            ShowArgsConsole(opt);
            ShowArgsNLog(opt);
        }
        public override string ToHtml()
        {
            Console.WriteLine("GroupHeader.ToHtml()");
            return "<table></table>";
        }
        private void ShowArgsConsole(GroupHeaderOptions opt)
        {
            Console.WriteLine("GroupHeader");
            Console.WriteLine("----- Arguments -----");
            Console.WriteLine("Header: {0}", opt.Header);
            Console.WriteLine($"Row: {opt.Row}");
            Console.WriteLine($"Column: {opt.Column}");
            Console.WriteLine($"RowHeaderAttributes: {opt.RowHeaderAttributes}");
            Console.WriteLine($"ColumnHeaderAttributes: {opt.ColumnHeaderAttributes}");
            Console.WriteLine($"File: {opt.File }");
            Console.WriteLine($"Delimiter: {opt.Delimiter}");
            Console.WriteLine($"TableAttributes: {opt.TableAttributes}");
            Console.WriteLine($"LoggingLevel: {opt.LoggingLevel}");
            Console.WriteLine("----------");
        }
        private void ShowArgsNLog(GroupHeaderOptions opt)
        {
            logger.Debug("GroupHeader.ShowArgsNLog()");
            logger.Fatal("Fatal");
            logger.Error("Error");
            logger.Warn("Warn");
            logger.Info("Info");
            logger.Debug("Debug");
            logger.Trace("Trace");
            logger.Debug("----- Arguments -----");
            logger.Debug("Header                : {}", opt.Header);
            logger.Debug("Row                   : {}", opt.Row);
            logger.Debug("Column                : {}", opt.Column);
            logger.Debug("RowHeaderAttributes   : {}", opt.RowHeaderAttributes);
            logger.Debug("ColumnHeaderAttributes: {}", opt.ColumnHeaderAttributes);
            logger.Debug("File                  : {}", opt.File);
            logger.Debug("Delimiter             : {}", opt.Delimiter);
            logger.Debug("TableAttributes       : {}", opt.TableAttributes);
            logger.Debug("LoggingLevel          : {}", opt.LoggingLevel);
            logger.Debug("----------");
        }
    }
}
