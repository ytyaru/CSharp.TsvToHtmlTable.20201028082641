using System.Reflection;
using System.Resources; // NeutralResourcesLanguage
using CommandLine.Text;
[assembly:AssemblyVersionAttribute("0.0.1")]
[assembly:AssemblyTitle("tsv2table")]
[assembly:AssemblyDescription("TSVをHTMLのtableタグに変換する。")]
[assembly:AssemblyCopyright("(C) 2020 ytyaru")]
[assembly:NeutralResourcesLanguage("ja-JP")]

[assembly:AssemblyLicense("CC0 : https://creativecommons.org/publicdomain/zero/1.0/deed.ja")]
[assembly:AssemblyUsage("  TSVをHTMLのtableタグに変換する。", 
                        "  tsv2talbe [g|i|n]",
                        "  tsv2talbe g [-H a|r|c|m] [-r t|b|B] [-c l|r|B]",
                        "  tsv2talbe i [-H c|r] [-s N] [-S N]",
                        "  tsv2talbe n")]
