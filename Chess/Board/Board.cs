namespace board
{
    class Board
    {
        public int Lines { get; set; }
        public int Columns { get; set; }
        public Piece[,] Piece;

        public Board()
        {

        }

        public Board(int lines, int columns)
        {
            Lines = lines;
            Columns = columns;
            Piece = new Piece[lines, columns];
        }

        public Piece UniquePiece(int line, int column)
        {
            return Piece[line, column];
        }
    }
}
