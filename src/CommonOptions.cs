using System;
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;
using NLog;
using NLog.Config;
using Microsoft.VisualBasic;
namespace TsvToHtmlTable
{
    public enum LoggingLevelType { f, e, w, i, d, t }
//    public enum LoggingLevelType { c, e, w, i, d }

    public class CommonOptions
    {
        [Value(1, MetaName="file")]
        public string File { get; set; } = "";
        [Option('d', "delimiter", Required=false, HelpText="入力の区切文字。引数にファイルパスが指定されているときは拡張子で特定する。")]
        public string Delimiter { get; set; } = "\t";
        [Option('T', "table-attributes", Required = false, HelpText="table要素の属性。")]
        public string TableAttributes { get; set; } = "";
        [Option('l', "logging-level", Required=false, Default=LoggingLevelType.f, HelpText="ログ出力する。指定したレベル以上のもののみ出力する。")]
        public LoggingLevelType LoggingLevel { get; set; } = LoggingLevelType.f;

        public System.IO.TextReader Source = new System.IO.StringReader("");
        public List<List<Cell>> SourceList = new List<List<Cell>>();
        public void SetLoggingLevels()
        {
            var level = this.LoggingLevel switch
            {
                LoggingLevelType.f => LogLevel.Fatal,
                LoggingLevelType.e => LogLevel.Error,
                LoggingLevelType.w => LogLevel.Warn,
                LoggingLevelType.i => LogLevel.Info,
                LoggingLevelType.d => LogLevel.Debug,
                LoggingLevelType.t => LogLevel.Trace,
                _ => LogLevel.Fatal,
            };
//            Console.WriteLine(level);
            foreach (LoggingRule rule in NLog.LogManager.Configuration.LoggingRules)
            {
                rule.SetLoggingLevels(level, LogLevel.Fatal);
//                rule.DisableLoggingForLevel(level);
            }
//            LogManager.Configuration.Reload();
            LogManager.ReconfigExistingLoggers();
        }
        public void GetInput()
        {
            if (0 == this.File.Length) {
                this.Source = System.Console.In;
//                this.Source = System.Console.In.ReadToEnd();
            } else {
                this.Source = new System.IO.StreamReader(this.File);
//                this.Source = new System.IO.StreamReader(this.File).ReadToEnd();
//                this.Source = new System.IO.StreamReader(this.File,
//                                    System.Text.Encoding.GetEncoding("UTF-8"));
            }
        }
        /*
        private void LoadInput(TextReader reader)
        {
            string line = "";
            while ((line = reader.ReadLine()) != null) {
                
            }
        }
        */
    }
}