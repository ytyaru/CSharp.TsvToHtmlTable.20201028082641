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
        [Option('R', "--row-header-attribute", Required=false, HelpText="行ヘッダ属性。")]
        public string RowHeaderAttribute { get; set; } = "";
        [Option('C', "--column-header-attribute", Required=false, HelpText="列ヘッダ属性。")]
        public string ColumnHeaderAttribute { get; set; } = "";
        [Option('M', "--matrix-header-attribute", Required=false, HelpText="行列ヘッダ属性。")]
        public string MatrixHeaderAttribute { get; set; } = "";

        /*
//        [Usage(ApplicationAlias = "yourapp")]
        [Usage]
        public static IEnumerable<Example> Examples {
            get {
                yield return new Example("Normal scenario", new GroupHeaderOptions {});
                yield return new Example("Normal scenario", new GroupHeaderOptions { Row=RowHeaderPosType.b });
                yield return new Example("Normal scenario", new GroupHeaderOptions { Row=RowHeaderPosType.B });
//                yield return new Example("Logging warnings", UnParserSettings.WithGroupSwitchesOnly() , new Options { InputFile = "file.bin", LogWarning = true });
//                yield return new Example("Logging errors", new[] { UnParserSettings.WithGroupSwitchesOnly(), UnParserSettings.WithUseEqualTokenOnly() }, new Options { InputFile = "file.bin", LogError = true });
            }
        }
        */
    }
}
