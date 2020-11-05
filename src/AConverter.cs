using System;
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NLog;
namespace TsvToHtmlTable
{
    public abstract class AConverter
    {
        private Logger logger = NLog.LogManager.GetLogger("AppDefaultLogger");
        public AConverter(CommonOptions opt)
        {
            opt.SetLoggingLevels();
            opt.GetInput();
            this.MakeSourceList(opt);
        }
        private void MakeSourceList(CommonOptions opt)
        {
            opt.SourceList.Clear();
            using (opt.Source) {
                List<string> lines = ReadLines(opt.Source.ReadToEnd());
                RemoveBlankLines(ref lines);
                RemoveShebang(ref lines);
                logger.Debug("lines.Count: {}", lines.Count);
                opt.Delimiter = InferDelimiter(lines);
                logger.Debug("Delimiter: {}", opt.Delimiter);
                logger.Debug("InferDelimiter(lines): {}", InferDelimiter(lines));
                foreach (string line in lines)
                {
                    logger.Debug(line);
                    opt.SourceList.Add(new List<Cell>());
                    foreach (string col in line.Split(opt.Delimiter))
                    {
                        opt.SourceList.Last().Add(new Cell { Text=col });
                    }
                }
            }
        }
        private List<string> ReadLines(string text)
        {
            return new List<string>(text.Split(Environment.NewLine));
        }
        private void RemoveBlankLines(ref List<string> lines)
        {
            lines.RemoveAll(s => s == "");
        }
        private void RemoveShebang(ref List<string> lines)
        {
            if (null == lines || 0 == lines.Count) { return; }
            if (lines[0].StartsWith("#!")) { lines.RemoveAt(0); }
        }
        private string InferDelimiter(List<string> lines)
        {
            if (null == lines || 0 == lines.Count) { return string.Empty; }
            int t = lines[0].CountOf("\t");
            int c = lines[0].CountOf(",");
            if (1 == lines.Count) {
                if (t < c) { return ","; }
                return "\t";
            }
            if (0 < c && c == lines[1].CountOf(",")) { return ","; }
            return "\t";
        }


        /*
        private void MakeSourceList(CommonOptions opt)
        {
            opt.SourceList.Clear();
            using (opt.Source) {
                string line = "";
                while((line = opt.Source.ReadLine()) != null)  
                {  
                    logger.Debug(line);
                    opt.SourceList.Add(new List<Cell>());
                    foreach (string col in line.Split(opt.Delimiter))
                    {
                        opt.SourceList.Last().Add(new Cell { Text=col });
                    }
                }  
            }
        }
        */
        abstract public string ToHtml();
    }
}
