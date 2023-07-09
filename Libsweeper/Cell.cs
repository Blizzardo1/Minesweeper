namespace Libsweeper
{
    public class Cell {
        private int _row;
        private int _column;
        private bool _liveBomb;
        private int _liveNeighbors;
        private bool _visited;
        private bool _flagged;

        public int Row
        {
            get => _row;
            set => _row = value;
        }

        public int Column
        {
            get => _column;
            set => _column = value;
        }

        public bool LiveBomb
        {
            get => _liveBomb;
            set => _liveBomb = value;
        }

        public int LiveNeighbors {
            get => _liveNeighbors;
            set => _liveNeighbors = value;
        }

        public bool Visited
        {
            get => _visited;
            set => _visited = value;
        }

        public bool Flagged
        {
            get => _flagged;
            set => _flagged = value;
        }

        public Cell(int row, int column, bool live) {
            _row = row;
            _column = column;
            _liveBomb = live;
        }

        #region Overrides of Object

        /// <inheritdoc />
        public override string ToString() {

            return _liveNeighbors == 0 ? " " : _liveNeighbors.ToString();
        }

        #endregion
    }
}
