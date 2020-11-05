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

    public class CommonOptions
    {
        [Value(1, MetaName="file", HelpText="入力ファイルパス。")]
        public string File { get; set; } = "";
        [Option('d', "delimiter", Required=false, HelpText="入力の区切文字。引数にファイルパスが指定されているときは拡張子で特定する。初期値:\"\\t\"。未指定のときはファイル内容から\"\\t\"か\",\"のどちらかだと推測する。")]
        public string Delimiter { get; set; } = string.Empty;
        [Option('T', "table-attribute", Required = false, HelpText="table要素の属性。")]
        public string TableAttribute { get; set; } = "";
        [Option('l', "logging-level", Required=false, Default=LoggingLevelType.e, HelpText="ログ出力する。指定したレベル以上のもののみ出力する。")]
        public LoggingLevelType LoggingLevel { get; set; } = LoggingLevelType.e;

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
            foreach (LoggingRule rule in NLog.LogManager.Configuration.LoggingRules)
            {
                rule.SetLoggingLevels(level, LogLevel.Fatal);
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
    }
}
