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
    }
}
