using System;
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;

namespace TsvToHtmlTable
{
    public enum InnerHeaderType { c, r }

    [Verb("i", HelpText = "ヘッダが内側にある。" )]
    public class InnerHeaderOptions
    {
        [Option('H', "header", Required = false, HelpText = "指定した軸がヘッダになる。")]
        public InnerHeaderType Header { get; set; } = InnerHeaderType.c;
        [Option('s', "start", Required = false, HelpText = "ヘッダ開始位置。")]
        public int Start { get; set; } = 1;
        [Option('S', "step", Required = false, HelpText = "ヘッダを指定数だけ飛ばす。")]
        public int Step { get; set; } = 1;
        [Option('A', "header-attributes", Required = false, HelpText = "全ヘッダに指定した属性を付与する")]
        public string HeaderAttributes { get; set; } = "t";

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
