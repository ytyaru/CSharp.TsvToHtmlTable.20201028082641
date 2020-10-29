using System;
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;

namespace TsvToHtmlTable
{
    [Verb("n", HelpText = "ヘッダがない。" )]
    public class NoneHeaderOptions
    {
        // 共通引数
        [Value(1, MetaName = "file")]
        public string File { get; set; } = "";
        [Option('d', "delimiter", Required = false, HelpText = "入力の区切文字。引数にファイルパスが指定されているときは拡張子で特定する。")]
        public string Delimiter { get; set; } = "\t";
        [Option('T', "table-attributes", Required = false, HelpText = "table要素の属性。")]
        public string TableAttributes { get; set; } = "";
        [Option('l', "logging-level", Required=false, Default=LoggingLevelType.f, HelpText="ログ出力する。指定したレベル以上のもののみ出力する。")]
        public LoggingLevelType LoggingLevel { get; set; } = LoggingLevelType.f;
    }
}
