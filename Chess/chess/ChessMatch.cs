using board;

namespace chess
{
    class ChessMatch
    {
        public Board BoardOfMatch { get; private set; }
        private int Turn;
        private Color ActualPlayer;
        public bool Ended { get; private set; }

        public ChessMatch()
        {
            BoardOfMatch = new Board(8, 8);
            Turn = 1;
            ActualPlayer = Color.White;
            Ended = false;
            PutPieces();
        }

        public void ExecuteMoviment(Position origin, Position destiny)
        {
            Piece p = BoardOfMatch.RemovePiece(origin);
            p.IncrementManyMoves();
            Piece takenPiece = BoardOfMatch.RemovePiece(destiny);
            BoardOfMatch.PutPiece(p, destiny);
        }

        private void PutPieces()
        {
            BoardOfMatch.PutPiece(new Tower(Color.Black, BoardOfMatch), new ChessPosition('c', 1).ToPosition());
            BoardOfMatch.PutPiece(new Tower(Color.Black, BoardOfMatch), new ChessPosition('a', 5).ToPosition());
            BoardOfMatch.PutPiece(new King(Color.White, BoardOfMatch), new ChessPosition('f', 3).ToPosition());
            BoardOfMatch.PutPiece(new King(Color.White, BoardOfMatch), new ChessPosition('h', 5).ToPosition());
        }
    }
}
