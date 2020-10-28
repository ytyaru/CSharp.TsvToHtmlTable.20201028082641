using System;
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;
namespace TsvToHtmlTable
{
    enum Hoge
    {
        X, Y, Z
    }
    class Options
    {
        // 基本的な形式
        [Option('a', "aaa", Required = false, HelpText = "AAAA")]
        public string A { get; set; } = "";
        // プリミティブ型であれば、string以外でも受け取ることが可能
        [Option('b', "bbb", Required = false, HelpText = "BBBB")]
        public bool B { get; set; }
        // 複数の値を受け取ることが可能。区切り文字はSeparatorで指定
        [Option('c', "ccc", Separator = ',')]
        public IEnumerable<string> C { get; set; } = new List<string>();
        // enumを受け取ることも可能(指定にはenumの名前を指定する)
        [Option('d', "ddd")]
        public Hoge D { get; set; }
        // オプション以外の引数を受け取るための属性
        [Value(1, MetaName = "remaining")]
        public IEnumerable<string> Remaining { get; set; } = new List<string>();
    }
}
