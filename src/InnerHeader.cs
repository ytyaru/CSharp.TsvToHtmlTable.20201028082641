using System;
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;
using NLog;
using NLog.Config;
namespace TsvToHtmlTable
{
    public class InnerHeader : AConverter
    {
        private Logger logger = NLog.LogManager.GetLogger("AppDefaultLogger");
        public InnerHeader(InnerHeaderOptions opt):base(opt)
        {
            SetLoggingLevel(opt);
            ShowArgsConsole(opt);
            ShowArgsNLog(opt);
        }
        public override string ToHtml()
        {
            Console.WriteLine("InnerHeader.ToHtml()");
            return "<table></table>";
        }
        private void SetLoggingLevel(InnerHeaderOptions opt)
        {
            var level = opt.LoggingLevel switch
            {
                LoggingLevelType.f => LogLevel.Fatal,
                LoggingLevelType.e => LogLevel.Error,
                LoggingLevelType.w => LogLevel.Warn,
                LoggingLevelType.i => LogLevel.Info,
                LoggingLevelType.d => LogLevel.Debug,
                LoggingLevelType.t => LogLevel.Trace,
                _ => LogLevel.Fatal,
            };
            Console.WriteLine(level);
            foreach (LoggingRule rule in NLog.LogManager.Configuration.LoggingRules)
            {
                rule.DisableLoggingForLevel(level);
            }
        }
        private void ShowArgsConsole(InnerHeaderOptions opt)
        {
            Console.WriteLine("InnerHeader.ShowArgsConsole()");
            Console.WriteLine("----- Arguments -----");
            Console.WriteLine($"Header: {opt.Header}");
            Console.WriteLine($"Start: {opt.Start}");
            Console.WriteLine($"Step: {opt.Step}");
            Console.WriteLine($"HeaderAttributes: {opt.HeaderAttributes}");
            Console.WriteLine($"File: {opt.File }");
            Console.WriteLine($"Delimiter: {opt.Delimiter}");
            Console.WriteLine($"TableAttributes: {opt.TableAttributes}");
            Console.WriteLine($"LoggingLevel: {opt.LoggingLevel}");
            Console.WriteLine("----------");
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
            logger.Debug("----------");
        }
    }
}
