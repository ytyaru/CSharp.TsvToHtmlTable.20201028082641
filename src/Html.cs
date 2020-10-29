using System;
using System.Text;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;
//using System.Collections.Specialized; // OrderedDictionary
namespace TsvToHtmlTable
{
    public static class Html
    {
        public static string Enclose(string element)
        {
            var builder = new StringBuilder();
            builder.Append('<');
            builder.Append(element);
            builder.Append(' ');
            builder.Append('/');
            builder.Append('>');
            return builder.ToString();
        }
        public static string Enclose(string element, string text="")
        {
            var builder = new StringBuilder();
            builder.Append('<');
            builder.Append(element);
            builder.Append('>');
            builder.Append(text);
            builder.Append('<');
            builder.Append('/');
            builder.Append(element);
            builder.Append('>');
            return builder.ToString();
        }
        public static string Enclose(string element, Dictionary<string, string> attrs)
        {
            var builder = new StringBuilder();
            builder.Append('<');
            builder.Append(element);
            builder.Append(' ');
            builder.Append(MakeAttrs(attrs));
            builder.Append(' ');
            builder.Append('/');
            builder.Append('>');
            return builder.ToString();
        }
//        public string Enclose(string element, string text="", OrderedDictionary attrs)
        public static string Enclose(string element, string text, Dictionary<string, string> attrs)
        {
//            "<" + element + MakeAttrs(attrs) + ">" + text + "</" + element + ">"
            var builder = new StringBuilder();
            builder.Append('<');
            builder.Append(element);
            builder.Append(' ');
            builder.Append(MakeAttrs(attrs));
            builder.Append('>');
            builder.Append(text);
            builder.Append('<');
            builder.Append('/');
            builder.Append(element);
            builder.Append('>');
            return builder.ToString();
        }
//        private string MakeAttrs(OrderedDictionary attrs)
        private static string MakeAttrs(Dictionary<string, string> attrs)
        {
            var builder = new StringBuilder();
            foreach (var pair in attrs)
            {
                builder.Append(pair.Key);
                builder.Append('=');
                builder.Append('"');
                builder.Append(pair.Value);
                builder.Append('"');
            }
            return builder.ToString();
        }
    }
}
