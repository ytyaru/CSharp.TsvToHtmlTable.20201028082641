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
            var header = new Header(this.Options);
            header.Infer(cellTable);
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
        public GroupHeaderOptions Options { get; private set; }
        public RowHeader Row { get; private set; } = default!;
        public ColumnHeader Column { get; private set; } = default!;
        public MatrixHeader Matrix { get; private set; } = default!;
        public Header(GroupHeaderOptions opt)
        {
            this.Options = opt;
        }
        public void Infer(CellTable cellTable)
        {
            int colCnt = cellTable.InferColumnHeaderCount();
            int rowCnt = cellTable.InferRowHeaderCount(colCnt);
            logger.Debug("InferColumnHeaderCount: {}", colCnt);
            logger.Debug("InferRowHeaderCount: {}", rowCnt);

//            bool hasReverseRowHeader = () ? :;

            Row = new RowHeader(this.Options, cellTable, rowCnt, colCnt);
            Column = new ColumnHeader(this.Options, cellTable, rowCnt, colCnt);
            Matrix = new MatrixHeader(this.Options, rowCnt, colCnt);
        }

    }
    class RowHeader
    {
        private Logger logger = NLog.LogManager.GetLogger("AppDefaultLogger");
        public GroupHeaderOptions Options { get; private set; }
//        public List<List<Cell>> SourceList { get; private set; }
        public CellTable CellTable { get; private set; }
        public int Count { get; private set; }
        public List<List<Cell>> Cells { get; private set; } = new List<List<Cell>>();
        public List<List<Cell>> ReversedCells { get; private set; } = new List<List<Cell>>();
//        private List<List<Cell>> SourceList = default!;
        private int RowHeaderCount;
        private int ColumnHeaderCount;
        public RowHeader(GroupHeaderOptions opt, CellTable cellTable, int rowCnt, int colCnt, bool hasReverseHeader=false)
        {
//            this.SourceList = SourceList;
            this.Options = opt;
            this.CellTable = cellTable;
            this.RowHeaderCount = rowCnt;
            this.ColumnHeaderCount = rowCnt;
            logger.Trace("colCnt: {}", colCnt);
            MakeCells(rowCnt, colCnt);
            logger.Debug("RowHeader");
            if (RowHeaderPosType.b == this.Options.Row || 
                RowHeaderPosType.B == this.Options.Row) { MakeReversedCells(); }
        }
        private void MakeCells(int rowCnt, int colCnt)
        {
            foreach (var row in this.Options.SourceList.GetRange(0, rowCnt))
            {
                logger.Trace("row: {}", row);
                this.Cells.Add(row.GetRange(colCnt, row.Count - colCnt));
                logger.Trace("Cells: {}", Cells);
            }
        }
        private void MakeReversedCells()
        {
            this.DeepCopySourceList();
            this.ReversedCells.Reverse();

            for (int r=0; r<this.ReversedCells.Count; r++)
            {
                for (int c=0; c<this.ReversedCells[r].Count; c++)
                {
                    if ("" == this.ReversedCells[r][c].Text)
                    {
                        SwapText(r, c);
                    }
                }
            }
            /*
            */
            foreach (var row in this.ReversedCells)
            {
                foreach (var cell in row)
                {
                    Console.Write($"{cell.Text}\t");
                }
                Console.WriteLine();
            }
            /*
            foreach ((int r, int c) in this.CellTable.Cells())
            {
//                logger.Debug("{}\t", this.ReversedCells[r][c].Text);
                Console.Write("{}\t", this.ReversedCells[r][c].Text);
            }
            */
        }
        private void DeepCopySourceList()
        {
            this.ReversedCells = new List<List<Cell>>();
            foreach (var row in this.Cells)
            {
                this.ReversedCells.Add(new List<Cell>());
                foreach (var cell in row)
                {
                    Cell n = new Cell();
                    n.Text = cell.Text;
                    n.RowSpan  = cell.RowSpan;
                    n.ColSpan= cell.ColSpan;
                    this.ReversedCells.Last().Add(n);
                }
            }
        }
        private void SwapText(int r, int c)
        {
            for (int R=r; R<this.ReversedCells.Count; R++)
            {
                if ("" != ReversedCells[R][c].Text)
                {
                    string tmp = ReversedCells[r][c].Text;
                    ReversedCells[r][c].Text = ReversedCells[R][c].Text;
                    ReversedCells[R][c].Text = tmp;
                }
            }
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
        public GroupHeaderOptions Options { get; private set; }
        /*
        public ColumnHeader(GroupHeaderOptions opt)
        {
            this.Options = opt;
        }
        */
        public ColumnHeader(GroupHeaderOptions opt, CellTable cellTable, int rowCnt, int colCnt)
        {
            this.Options = opt;
        }
    }
    class MatrixHeader
    {
        private Logger logger = NLog.LogManager.GetLogger("AppDefaultLogger");
        public GroupHeaderOptions Options { get; private set; }
        /*
        public MatrixHeader(GroupHeaderOptions opt)
        {
            this.Options = opt;
        }
        */
        public MatrixHeader(GroupHeaderOptions opt, int rowCnt, int colCnt)
        {
            this.Options = opt;
        }

    }
}
