using System;
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;

namespace TsvToHtmlTable
{
    public enum HeaderType { a, r, c, m }
    public enum RowHeaderPosType { t, b, B }
    public enum ColumnHeaderPosType { l, r, B }
    public enum LoggingLevelType { c, e, w, i, d }

    [Verb("g", true, HelpText = "ヘッダが外側にある。" )]
    public class GroupHeaderOptions
    {
        [Option('H', "header", Required = false, HelpText = "ヘッダ。")]
        public HeaderType Header { get; set; } = HeaderType.a;
        [Option('r', "row", Required = false, HelpText = "行ヘッダ位置。")]
        public RowHeaderPosType Row { get; set; } = RowHeaderPosType.t;
        [Option('c', "column", Required = false, HelpText = "列ヘッダ位置。")]
        public ColumnHeaderPosType Column { get; set; } = ColumnHeaderPosType.l;
        [Option('R', "--row-header-attributes", Required = false, HelpText = "行ヘッダ属性。")]
        public string RowHeaderAttributes { get; set; } = "t";
        [Option('C', "--column-header-attributes", Required = false, HelpText = "列ヘッダ属性。")]
        public string ColumnHeaderAttributes { get; set; } = "t";

        // 共通引数
        [Value(1, MetaName = "file")]
        public string File { get; set; } = "";
        [Option('d', "delimiter", Required = false, HelpText = "入力の区切文字。引数にファイルパスが指定されているときは拡張子で特定する。")]
        public string Delimiter { get; set; } = "\t";
        [Option('T', "table-attributes", Required = false, HelpText = "table要素の属性。")]
        public string TableAttributes { get; set; } = "";
        [Option('l', "logging-level", Required = false, HelpText = "ログ出力する。指定したレベル以上のもののみ出力する。")]
        public LoggingLevelType LoggingLevel { get; set; } = LoggingLevelType.c;
    }
}
