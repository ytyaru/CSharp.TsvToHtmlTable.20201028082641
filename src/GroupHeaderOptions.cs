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

    [Verb("g", true, HelpText = "ヘッダが外側にある。" )]
    public class GroupHeaderOptions : CommonOptions
    {
        [Option('H', "header", Required=false, Default=HeaderType.a, HelpText="ヘッダ。")]
        public HeaderType Header { get; set; } = HeaderType.a;
        [Option('r', "row", Required=false, Default=RowHeaderPosType.t, HelpText="行ヘッダ位置。")]
        public RowHeaderPosType Row { get; set; } = RowHeaderPosType.t;
        [Option('c', "column", Required=false, Default=ColumnHeaderPosType.l, HelpText="列ヘッダ位置。")]
        public ColumnHeaderPosType Column { get; set; } = ColumnHeaderPosType.l;
        [Option('R', "--row-header-attributes", Required=false, HelpText="行ヘッダ属性。")]
        public string RowHeaderAttributes { get; set; } = "";
        [Option('C', "--column-header-attributes", Required=false, HelpText="列ヘッダ属性。")]
        public string ColumnHeaderAttributes { get; set; } = "";
    }
}
