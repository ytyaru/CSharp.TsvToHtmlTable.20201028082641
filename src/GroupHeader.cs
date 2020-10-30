using System;
//using System.Diagnostics;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;
using NLog;
namespace TsvToHtmlTable
{
    public class GroupHeader : AConverter
    {
//        private Logger logger = LogManager.GetCurrentClassLogger();
        private Logger logger = NLog.LogManager.GetLogger("AppDefaultLogger");
        public GroupHeaderOptions Options { get; private set; }
        public GroupHeader(GroupHeaderOptions opt):base(opt)
        {
            this.Options = opt;
            ShowArgsConsole(opt);
            ShowArgsNLog(opt);
        }
        public override string ToHtml()
        {
            Console.WriteLine("GroupHeader.ToHtml()");
            var cellTable = new CellTable(this.Options.SourceList);
            cellTable.SetBlankToZero();
            var header = new Header();
            header.Infer(this.Options.SourceList, cellTable);
//            logger.Debug("InferColumnHeaderCount: {}", cellTable.InferColumnHeaderCount());
//            logger.Debug("InferRowHeaderCount: {}", cellTable.InferRowHeaderCount());
//            cellTable.InferColumnHeaderCount();
//            cellTable.InferRowHeaderCount();
            return "<table></table>";
        }
        private void ShowArgsConsole(GroupHeaderOptions opt)
        {
            Console.WriteLine("GroupHeader");
            Console.WriteLine("----- Arguments -----");
            Console.WriteLine("Header: {0}", opt.Header);
            Console.WriteLine($"Row: {opt.Row}");
            Console.WriteLine($"Column: {opt.Column}");
            Console.WriteLine($"RowHeaderAttributes: {opt.RowHeaderAttributes}");
            Console.WriteLine($"ColumnHeaderAttributes: {opt.ColumnHeaderAttributes}");
            Console.WriteLine($"File: {opt.File }");
            Console.WriteLine($"Delimiter: {opt.Delimiter}");
            Console.WriteLine($"TableAttributes: {opt.TableAttributes}");
            Console.WriteLine($"LoggingLevel: {opt.LoggingLevel}");
            Console.WriteLine("----------");
        }
        private void ShowArgsNLog(GroupHeaderOptions opt)
        {
            logger.Debug("GroupHeader.ShowArgsNLog()");
            logger.Fatal("Fatal");
            logger.Error("Error");
            logger.Warn("Warn");
            logger.Info("Info");
            logger.Debug("Debug");
            logger.Trace("Trace");
            logger.Debug("----- Arguments -----");
            logger.Debug("Header                : {}", opt.Header);
            logger.Debug("Row                   : {}", opt.Row);
            logger.Debug("Column                : {}", opt.Column);
            logger.Debug("RowHeaderAttributes   : {}", opt.RowHeaderAttributes);
            logger.Debug("ColumnHeaderAttributes: {}", opt.ColumnHeaderAttributes);
            logger.Debug("File                  : {}", opt.File);
            logger.Debug("Delimiter             : {}", opt.Delimiter);
            logger.Debug("TableAttributes       : {}", opt.TableAttributes);
            logger.Debug("LoggingLevel          : {}", opt.LoggingLevel);
            logger.Debug("Source                :\n{}", opt.Source);
            logger.Debug("----------");
        }
    }
    class Header
    {
        private Logger logger = NLog.LogManager.GetLogger("AppDefaultLogger");
        public RowHeader Row { get; private set; } = default!;
        public ColumnHeader Column { get; private set; } = default!;
        public MatrixHeader Matrix { get; private set; } = default!;
        public void Infer(List<List<Cell>> SourceList, CellTable cellTable)
        {
            int colCnt = cellTable.InferColumnHeaderCount();
            int rowCnt = cellTable.InferRowHeaderCount(colCnt);
            logger.Debug("InferColumnHeaderCount: {}", colCnt);
            logger.Debug("InferRowHeaderCount: {}", rowCnt);
            Row = new RowHeader(SourceList, rowCnt, colCnt);
            Column = new ColumnHeader(SourceList, rowCnt, colCnt);
            Matrix = new MatrixHeader(rowCnt, colCnt);
        }

    }
    class RowHeader
    {
        private Logger logger = NLog.LogManager.GetLogger("AppDefaultLogger");
//        public List<List<Cell>> SourceList { get; private set; }
        public int Count { get; private set; }
        public List<List<Cell>> Cells { get; private set; } = new List<List<Cell>>();
        private List<List<Cell>> SourceList = default!;
        private int RowHeaderCount;
        private int ColumnHeaderCount;
        public RowHeader(List<List<Cell>> SourceList, int rowCnt, int colCnt)
        {
            this.SourceList = SourceList;
            this.RowHeaderCount = rowCnt;
            this.ColumnHeaderCount = rowCnt;
            logger.Trace("colCnt: {}", colCnt);
            foreach (var row in SourceList.GetRange(0, rowCnt))
            {
                logger.Trace("row: {}", row);
                this.Cells.Add(row.GetRange(colCnt, row.Count - colCnt));
                logger.Trace("Cells: {}", Cells);
            }
            /*
            */
        }
        private List<List<Cell>> ReversedCells()
        {
            return new List<List<Cell>>();
        }
        /*
        public IEnumerable<List<Cell>> Cells()
        {
            foreach (var row in this.SourceList.GetRange(0, this.RowHeaderCount))
            {
                yield return row.GetRange(colCnt, row.Count - this.ColumnHeaderCount);
            }
        }
        */
        public void Infer(List<List<Cell>> SourceList)
        {

        }
        private int InferRowHeaderCount()
        {
            return 0;
        }
    }
    class ColumnHeader
    {
        private Logger logger = NLog.LogManager.GetLogger("AppDefaultLogger");
        public ColumnHeader(List<List<Cell>> SourceList, int rowCnt, int colCnt)
        {
        }
    }
    class MatrixHeader
    {
        private Logger logger = NLog.LogManager.GetLogger("AppDefaultLogger");
        public MatrixHeader(int rowCnt, int colCnt)
        {
        }

    }
}
