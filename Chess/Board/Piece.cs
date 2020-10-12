namespace board
{
    abstract class Piece
    {
        public Position Position { get; set; }
        public Color Color { get; protected set; }
        public int ManyMoves { get; protected set; }
        public Board Board { get; protected set; }

        public Piece()
        {

        }

        public Piece(Color color, Board board)
        {
            Position = null;
            Board = board;
            Color = color;
            ManyMoves = 0;
        }

        public void IncrementManyMoves()
        {
            ManyMoves++;
        }

        public abstract bool[,] PossibleMoviments();
    }
}
