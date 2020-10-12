
using board;
using System.Collections.Generic;

namespace chess
{
    class ChessMatch
    {
        public Board BoardOfMatch { get; private set; }
        public int Turn { get; private set; }
        public Color ActualPlayer { get; private set; }
        public bool Ended { get; private set; }
        private HashSet<Piece> Pieces;
        private HashSet<Piece> CapturedPieces;

        public ChessMatch()
        {
            BoardOfMatch = new Board(8, 8);
            Turn = 1;
            ActualPlayer = Color.White;
            Ended = false;
            Pieces = new HashSet<Piece>();
            CapturedPieces = new HashSet<Piece>();
            PutPieces();
        }

        public void ExecuteMoviment(Position origin, Position destiny)
        {
            Piece p = BoardOfMatch.RemovePiece(origin);
            p.IncrementManyMoves();
            Piece takenPiece = BoardOfMatch.RemovePiece(destiny);
            BoardOfMatch.PutPiece(p, destiny);
            if(takenPiece != null)
            {
                CapturedPieces.Add(takenPiece);
            }
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
        
        public HashSet<Piece> CapturedPiecesOfTeam (Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();

            foreach(Piece piece in CapturedPieces)
            {
                if(piece.Color == color)
                {
                    aux.Add(piece);
                }
            }

            return aux;
        }

        public HashSet<Piece> InGamePieces(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();

            foreach (Piece piece in Pieces)
            {
                if (piece.Color == color)
                {
                    aux.Add(piece);
                }
            }

            aux.ExceptWith(CapturedPiecesOfTeam(color));

            return aux;
        }

        public void PutNewPiece(char column, int line, Piece piece)
        {
            BoardOfMatch.PutPiece(piece, new ChessPosition(column, line).ToPosition());
            Pieces.Add(piece);
        }

        private void PutPieces()
        {
            PutNewPiece('c', 1, new Tower(Color.White, BoardOfMatch));
            PutNewPiece('c', 2, new Tower(Color.White, BoardOfMatch));
            PutNewPiece('d', 2, new Tower(Color.White, BoardOfMatch));
            PutNewPiece('e', 1, new Tower(Color.White, BoardOfMatch));
            PutNewPiece('e', 2, new Tower(Color.White, BoardOfMatch));

            PutNewPiece('d', 1, new King(Color.White, BoardOfMatch));

            PutNewPiece('c', 8, new Tower(Color.Black, BoardOfMatch));
            PutNewPiece('c', 7, new Tower(Color.Black, BoardOfMatch));
            PutNewPiece('d', 7, new Tower(Color.Black, BoardOfMatch));
            PutNewPiece('e', 8, new Tower(Color.Black, BoardOfMatch));
            PutNewPiece('e', 7, new Tower(Color.Black, BoardOfMatch));

            PutNewPiece('d', 8, new King(Color.Black, BoardOfMatch));
        }
    }
}
