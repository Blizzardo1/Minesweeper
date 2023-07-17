using Microsoft.VisualStudio.TestTools.UnitTesting;
using Libsweeper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libsweeper.Tests
{
    [TestClass]
    public class BoardTests {

        [TestMethod]
        public void BoardTest() {
            // Assert that a default board is not null
            // Use 12x12 and a default difficulty of 0.2d (Level 2).
            Board board = new (new Size(12, 12));
            Assert.IsNotNull(board, "Board is null");
        }

        [TestMethod]
        public void SetupLiveNeighborsTest()
        {
            Board board = new(new Size(12, 12));
            
            board.SetupLiveNeighbors();
            Assert.IsTrue(board.Cells != null, "Cells are null");
            Assert.IsTrue(board.Cells.Length > 0, "Board.Cells is empty");
        }

        [TestMethod]
        public void CalculateLiveNeighborsTest() {
            Board board = new(new Size(12, 12));
            board.SetupLiveNeighbors();
            board.CalculateLiveNeighbors();
            int count = board.Cells.Cast< Cell >().Count(boardCell => boardCell.LiveBomb); // Bomb Count
            Assert.IsTrue(count > 0, "No bombs were found.");
        }

        [TestMethod]
        public void VisitNeighborsTest()
        {
            Board board = new(new Size(12, 12));
            board.SetupLiveNeighbors();
            board.VisitNeighbors(board.Cells[0, 0]);
            Assert.IsTrue(board.Cells[0, 0].Visited, "Cell was not visited");
        }

        [TestMethod]
        public void ResetTest()
        {
            Board board = new(new Size(12, 12));
            board.Reset();
            Assert.IsTrue(board.Cells != null, "Cells is null");
        }

        [TestMethod]
        public void ResizeTest()
        {
            Board board = new(new Size(12, 12));
            board.Resize(new Size(15,15));
            Assert.IsTrue(board.Size.Width == 15, "Width is not 15");
        }

        [TestMethod]
        public void CheckWinTest() {
            Board board = new(new Size(12, 12));
            board.SetupLiveNeighbors();
            board.CalculateLiveNeighbors();
            Assert.IsFalse(board.CheckWin(), "Board is not in a winning state.");
        }

        [TestMethod]
        public void FlagCellTest()
        {
            Board board = new(new Size(12, 12));
            board.SetupLiveNeighbors();
            board.FlagCell(1,1);
            Assert.IsTrue(board.Cells[1, 1].Flagged, "Cell was not flagged");
        }
    }
}