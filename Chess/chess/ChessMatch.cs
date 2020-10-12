using board;

namespace chess
{
    class ChessMatch
    {
        public Board BoardOfMatch { get; private set; }
        public int Turn { get; private set; }
        public Color ActualPlayer { get; private set; }
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

        public void PerformPlay(Position origin, Position destiny)
        {
            ExecuteMoviment(origin, destiny);
            Turn++;
            ChangePlayer();
        }

        public void ValidateOriginPosition(Position pos)
        {
            if (BoardOfMatch.UniquePiece(pos) == null)
            {
                throw new BoardException("[ERROR] There isn't any piece in this position");
            }
            if (ActualPlayer != BoardOfMatch.UniquePiece(pos).Color)
            {
                throw new BoardException("[ERROR] Please choose a piece of the right team");
            }
            if (!BoardOfMatch.UniquePiece(pos).IsTherePossibleMoves())
            {
                throw new BoardException("[ERROR] There isn't any possible moves for that piece");
            }
        }

        public void ValidateDestinyPosition(Position origin, Position destiny)
        {
            if (!BoardOfMatch.UniquePiece(origin).CanMoveFor(destiny))
            {
                throw new BoardException("[ERROR] Invalid destiny position");
            }
        }

        private void ChangePlayer()
        {
            if (ActualPlayer == Color.White)
            {
                ActualPlayer = Color.Black;
            } else {
                ActualPlayer = Color.White;
            }
        }

        private void PutPieces()
        {
            BoardOfMatch.PutPiece(new Tower(Color.White, BoardOfMatch), new ChessPosition('c', 1).ToPosition());
            BoardOfMatch.PutPiece(new Tower(Color.White, BoardOfMatch), new ChessPosition('c', 2).ToPosition());
            BoardOfMatch.PutPiece(new Tower(Color.White, BoardOfMatch), new ChessPosition('d', 2).ToPosition());
            BoardOfMatch.PutPiece(new Tower(Color.White, BoardOfMatch), new ChessPosition('e', 1).ToPosition());
            BoardOfMatch.PutPiece(new Tower(Color.White, BoardOfMatch), new ChessPosition('e', 2).ToPosition());

            BoardOfMatch.PutPiece(new King(Color.White, BoardOfMatch), new ChessPosition('d', 1).ToPosition());

            BoardOfMatch.PutPiece(new Tower(Color.Black, BoardOfMatch), new ChessPosition('c', 8).ToPosition());
            BoardOfMatch.PutPiece(new Tower(Color.Black, BoardOfMatch), new ChessPosition('c', 7).ToPosition());
            BoardOfMatch.PutPiece(new Tower(Color.Black, BoardOfMatch), new ChessPosition('d', 7).ToPosition());
            BoardOfMatch.PutPiece(new Tower(Color.Black, BoardOfMatch), new ChessPosition('e', 8).ToPosition());
            BoardOfMatch.PutPiece(new Tower(Color.Black, BoardOfMatch), new ChessPosition('e', 7).ToPosition());

            BoardOfMatch.PutPiece(new King(Color.Black, BoardOfMatch), new ChessPosition('d', 8).ToPosition());
        }
    }
}
