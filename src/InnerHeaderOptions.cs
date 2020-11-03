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
    public class InnerHeaderOptions : CommonOptions
    {
        [Option('H', "header", Required=false, Default=InnerHeaderType.c, HelpText = "指定した軸がヘッダになる。")]
        public InnerHeaderType Header { get; set; } = InnerHeaderType.c;
        [Option('s', "start", Required=false, Default=1, HelpText="ヘッダ開始位置。")]
        public int Start { get; set; } = 1;
        [Option('S', "step", Required=false, Default=1, HelpText="ヘッダを指定数だけ飛ばす。")]
        public int Step { get; set; } = 1;
        [Option('A', "header-attribute", Required=false, HelpText="全ヘッダに指定した属性を付与する")]
        public string HeaderAttribute { get; set; } = "";
    }
}
