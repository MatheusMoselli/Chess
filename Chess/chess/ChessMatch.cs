
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
        public bool Check { get; private set; }

        private HashSet<Piece> Pieces;
        private HashSet<Piece> CapturedPieces;

        public ChessMatch()
        {
            BoardOfMatch = new Board(8, 8);
            Turn = 1;
            ActualPlayer = Color.White;
            Ended = false;
            Check = false;
            Pieces = new HashSet<Piece>();
            CapturedPieces = new HashSet<Piece>();
            PutPieces();
        }

        public Piece ExecuteMoviment(Position origin, Position destiny)
        {
            Piece p = BoardOfMatch.RemovePiece(origin);
            p.IncrementManyMoves();
            Piece takenPiece = BoardOfMatch.RemovePiece(destiny);
            BoardOfMatch.PutPiece(p, destiny);
            if(takenPiece != null)
            {
                CapturedPieces.Add(takenPiece);
            }
            return takenPiece;
        }

        public void UndoMoviment(Position origin, Position destiny, Piece takenPiece)
        {
            Piece p = BoardOfMatch.RemovePiece(destiny);
            p.DecrementManyMoves();
            if (takenPiece != null)
            {
                BoardOfMatch.PutPiece(takenPiece, destiny);
                CapturedPieces.Remove(takenPiece);
            }
            BoardOfMatch.PutPiece(p, origin);
        }

        public void PerformPlay(Position origin, Position destiny)
        {
            Piece takenPiece = ExecuteMoviment(origin, destiny);

            if(IsTheKingInCheck(ActualPlayer))
            {
                UndoMoviment(origin, destiny, takenPiece);
                throw new BoardException("[ERROR] Can't put yourself in check");
            }
            if (IsTheKingInCheck(AdversaryColor(ActualPlayer))) {
                Check = true;
            } else
            {
                Check = false;
            }

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

        private Color AdversaryColor(Color color)
        {
            if(color == Color.White)
            {
                return Color.Black;
            } else
            {
                return Color.White;
            }
        }

        private Piece IfKingExisits(Color color)
        {
            foreach(Piece piece in InGamePieces(color))
            {
                if (piece is King)
                {
                    return piece;
                }
            }
            return null;
        }

        public bool IsTheKingInCheck(Color color)
        {
            Piece king = IfKingExisits(color);
            if (king == null)
            {
                throw new BoardException("[ERROR] King of color: " + color + " not found");
            }
            foreach(Piece piece in InGamePieces(AdversaryColor(color)))
            {
                bool[,] mat = piece.PossibleMoviments();
                if (mat[king.Position.Line, king.Position.Column])
                {
                    return true;
                }
            }
            return false;
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
