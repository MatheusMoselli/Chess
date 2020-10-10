namespace board
{
    class Board
    {
        public int Lines { get; set; }
        public int Column { get; set; }
        public Piece[,] Piece;

        public Board()
        {

        }

        public Board(int lines, int column)
        {
            Lines = lines;
            Column = column;
            Piece = new Piece[lines, column];
        }
    }
}
