using System;
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NLog;
using System.Text.RegularExpressions;
namespace TsvToHtmlTable
{
    public static partial class StringExtensions
    {
        /// <summary>
        /// 指定した文字列が存在する数を返す。
        /// </summary>
        public static int CountOf(this string self, params string[] strArray)
        {
            int count = 0;
            foreach (string str in strArray)
            {
                int index = self.IndexOf (str, 0);
                while (index != -1)
                {
                    count++;
                    index = self.IndexOf(str, index + str.Length);
                }
            }
            return count;
        }
    }
}
