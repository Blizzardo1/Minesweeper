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
        private Board _board;
        
        [TestMethod]
        public void BoardTest() {
            // Assert that a default board is not null
            // Use 12x12 and a default difficulty of 0.2d (Level 2).
            Assert.IsNotNull(_board = new Board(new Size(12, 12)), "Board is null");
        }

        [TestMethod]
        public void SetupLiveNeighborsTest()
        {
            _board.SetupLiveNeighbors();
            Assert.IsTrue(_board.Cells != null, "Cells are null");
            Assert.IsTrue(_board.Cells.Length > 0, "Board.Cells is empty");
        }

        [TestMethod]
        public void CalculateLiveNeighborsTest() {
            _board.CalculateLiveNeighbors();
            int count = _board.Cells.Cast< Cell >().Count(boardCell => boardCell.LiveBomb); // Bomb Count
            Assert.IsTrue(count > 0, "No bombs were found.");
        }

        [TestMethod]
        public void VisitNeighborsTest()
        {
            _board.VisitNeighbors(_board.Cells[0, 0]);
            Assert.IsTrue(_board.Cells[0, 0].Visited, "Cell was not visited");
        }

        [TestMethod]
        public void ResetTest()
        {
            _board.Reset();
            Assert.IsTrue(_board.Cells != null, "Cells is null");
        }

        [TestMethod]
        public void ResizeTest()
        {
            _board.Resize(new Size(15,15));
            Assert.IsTrue(_board.Size.Width == 15, "Width is not 15");
        }

        [TestMethod]
        public void CheckWinTest() {
            Assert.IsFalse(_board.CheckWin(), "Board is not in a winning state.");
        }

        [TestMethod]
        public void FlagCellTest()
        {
            _board.FlagCell(1,1);
            Assert.IsTrue(_board.Cells[1, 1].Flagged, "Cell was not flagged");
        }
    }
}