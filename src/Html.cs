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
            if (string.IsNullOrEmpty(element)) { return string.Empty; }
            var builder = new StringBuilder();
            builder.Append('<');
            builder.Append(element);
            builder.Append(' ');
            builder.Append('/');
            builder.Append('>');
            return builder.ToString();
        }
        public static string Enclose(string element, string text)
        {
            if (string.IsNullOrEmpty(element)) { return string.Empty; }
            var builder = new StringBuilder();
            builder.Append('<');
            builder.Append(element);
            builder.Append('>');
            if (null != text) { builder.Append(text); }
            builder.Append('<');
            builder.Append('/');
            builder.Append(element);
            builder.Append('>');
            return builder.ToString();
        }
        public static string Enclose(string element, string attr, string? text)
        {
            if (string.IsNullOrEmpty(element)) { return string.Empty; }
            var builder = new StringBuilder();
            builder.Append('<');
            builder.Append(element);
            if (!string.IsNullOrEmpty(attr)) {
                builder.Append(' ');
                builder.Append(attr);
            }
            /*
            builder.Append('>');
            if (!string.IsNullOrEmpty(text)) { builder.Append(text); }
            builder.Append('<');
            builder.Append('/');
            builder.Append(element);
            builder.Append('>');
            */
            if (null == text) { 
                builder.Append(' ');
                builder.Append('/');
                builder.Append('>');
            } else {
                builder.Append('>');
                builder.Append(text);
                builder.Append('<');
                builder.Append('/');
                builder.Append(element);
                builder.Append('>');
            }
            /*
            if (string.IsNullOrEmpty(text)) {
                builder.Append(' ');
                builder.Append('/');
                builder.Append('>');
            } else {
                builder.Append('>');
                builder.Append(text);
                builder.Append('<');
                builder.Append('/');
                builder.Append(element);
                builder.Append('>');
            }
            */
            return builder.ToString();
        }

        public static string Enclose(string element, Dictionary<string, string> attrs)
        {
            var builder = new StringBuilder();
            builder.Append('<');
            builder.Append(element);
            if (0 < attrs.Count) {
                builder.Append(' ');
                builder.Append(MakeAttrs(attrs));
            }
            builder.Append(' ');
            builder.Append('/');
            builder.Append('>');
            return builder.ToString();
        }
        public static string Enclose(string element, string text, Dictionary<string, string> attrs)
        {
            var builder = new StringBuilder();
            builder.Append('<');
            builder.Append(element);
            if (0 < attrs.Count) {
                builder.Append(' ');
                builder.Append(MakeAttrs(attrs));
            }
            builder.Append('>');
            builder.Append(text);
            builder.Append('<');
            builder.Append('/');
            builder.Append(element);
            builder.Append('>');
            return builder.ToString();
        }
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
                builder.Append(' ');
            }
            return builder.ToString().Trim(' ');
        }
        public static string MakeAttr(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) { return string.Empty; }
            if (string.IsNullOrEmpty(value)) { return string.Empty; }
            var builder = new StringBuilder();
            builder.Append(key);
            builder.Append('=');
            builder.Append('"');
            builder.Append(value);
            builder.Append('"');
            return builder.ToString();
        }
        public static string MakeAttr(KeyValuePair<string,string> pair)
        {
            return MakeAttr(pair.Key, pair.Value);
        }
        public static string MakeAttrs(List<KeyValuePair<string,string>> pairs)
        {
            var builder = new StringBuilder();
            foreach (var pair in pairs)
            {
                if (string.IsNullOrEmpty(pair.Key)) { continue; }
                if (string.IsNullOrEmpty(pair.Value)) { continue; }
                builder.Append(pair.Key);
                builder.Append('=');
                builder.Append('"');
                builder.Append(pair.Value);
                builder.Append('"');
                builder.Append(' ');
            }
            return builder.ToString().Trim(' ');
        }
        public static string MakeAttrs(params string[] keyvalues)
        {
            var builder = new StringBuilder();
            if (0 != (keyvalues.Length % 2)) { return string.Empty; }
            for (int i=0; i<keyvalues.Length; i+=2)
            {
                if (string.IsNullOrEmpty(keyvalues[i])) { continue; }
                if (string.IsNullOrEmpty(keyvalues[i+1])) { continue; }
                builder.Append(keyvalues[i]);
                builder.Append('=');
                builder.Append('"');
                builder.Append(keyvalues[i+1]);
                builder.Append('"');
                builder.Append(' ');
            }
            return builder.ToString().Trim(' ');
        }
        public static string Quote(string value) { return '"' + value + '"'; }
    }
}
