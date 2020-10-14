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

            // #SpecialPlay

            // Castle Kingside
            if(p is King && destiny.Column == origin.Column + 2)
            {
                Position originOfRook = new Position(origin.Line, origin.Column + 3);
                Position destinyOfRook = new Position(origin.Line, origin.Column + 1);

                Piece rook = BoardOfMatch.RemovePiece(originOfRook);
                rook.IncrementManyMoves();
                BoardOfMatch.PutPiece(rook, destinyOfRook);
            }

            // Castle Queenside
            if (p is King && destiny.Column == origin.Column - 2)
            {
                Position originOfRook = new Position(origin.Line, origin.Column - 4);
                Position destinyOfRook = new Position(origin.Line, origin.Column - 1);

                Piece rook = BoardOfMatch.RemovePiece(originOfRook);
                rook.IncrementManyMoves();
                BoardOfMatch.PutPiece(rook, destinyOfRook);
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


            // #SpecialMove
            // Castle Kingside
            if (p is King && destiny.Column == origin.Column + 2)
            {
                Position originOfRook = new Position(origin.Line, origin.Column + 3);
                Position destinyOfRook = new Position(origin.Line, origin.Column + 1);

                Piece rook = BoardOfMatch.RemovePiece(destinyOfRook);
                rook.DecrementManyMoves();
                BoardOfMatch.PutPiece(rook, originOfRook);
            }

            // Castle Queenside
            if (p is King && destiny.Column == origin.Column - 2)
            {
                Position originOfRook = new Position(origin.Line, origin.Column - 4);
                Position destinyOfRook = new Position(origin.Line, origin.Column - 1);

                Piece rook = BoardOfMatch.RemovePiece(destinyOfRook);
                rook.IncrementManyMoves();
                BoardOfMatch.PutPiece(rook, originOfRook);
            }
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

            if (IsCheckmate(AdversaryColor(ActualPlayer)))
            {
                Ended = true;
            } else
            {
                Turn++;
                ChangePlayer();
            }
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
            if (!BoardOfMatch.UniquePiece(origin).PossibleMove(destiny))
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

        public bool IsCheckmate(Color color)
        {
            if (!IsTheKingInCheck(color))
            {
                return false;
            }
            foreach(Piece piece in InGamePieces(color))
            {
                bool[,] mat = piece.PossibleMoviments();
                for(int i = 0; i < BoardOfMatch.Lines; i++)
                {
                    for(int j = 0; j < BoardOfMatch.Columns; j++)
                    {
                        if (mat[i, j])
                        {
                            Position origin = piece.Position;
                            Position destiny = new Position(i, j);
                            Piece takenPiece = ExecuteMoviment(origin, destiny);
                            bool isInCheck = IsTheKingInCheck(color);
                            UndoMoviment(origin, destiny, takenPiece);
                            if (!isInCheck)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void PutNewPiece(char column, int line, Piece piece)
        {
            BoardOfMatch.PutPiece(piece, new ChessPosition(column, line).ToPosition());
            Pieces.Add(piece);
        }

        private void PutPieces()
        {
            PutNewPiece('a', 1, new Rook(Color.White, BoardOfMatch));
            PutNewPiece('b', 1, new Horse(Color.White, BoardOfMatch));
            PutNewPiece('c', 1, new Bishop(Color.White, BoardOfMatch));
            PutNewPiece('d', 1, new Queen(Color.White, BoardOfMatch));
            PutNewPiece('e', 1, new King(Color.White, BoardOfMatch, this));
            PutNewPiece('f', 1, new Bishop(Color.White, BoardOfMatch));
            PutNewPiece('g', 1, new Horse(Color.White, BoardOfMatch));
            PutNewPiece('h', 1, new Rook(Color.White, BoardOfMatch));
            PutNewPiece('a', 2, new Pawn(Color.White, BoardOfMatch));
            PutNewPiece('b', 2, new Pawn(Color.White, BoardOfMatch));
            PutNewPiece('c', 2, new Pawn(Color.White, BoardOfMatch));
            PutNewPiece('d', 2, new Pawn(Color.White, BoardOfMatch));
            PutNewPiece('e', 2, new Pawn(Color.White, BoardOfMatch));
            PutNewPiece('f', 2, new Pawn(Color.White, BoardOfMatch));
            PutNewPiece('g', 2, new Pawn(Color.White, BoardOfMatch));
            PutNewPiece('h', 2, new Pawn(Color.White, BoardOfMatch));

            PutNewPiece('a', 8, new Rook(Color.Black, BoardOfMatch));
            PutNewPiece('b', 8, new Horse(Color.Black, BoardOfMatch));
            PutNewPiece('c', 8, new Bishop(Color.Black, BoardOfMatch));
            PutNewPiece('d', 8, new Queen(Color.Black, BoardOfMatch));
            PutNewPiece('e', 8, new King(Color.Black, BoardOfMatch, this));
            PutNewPiece('f', 8, new Bishop(Color.Black, BoardOfMatch));
            PutNewPiece('g', 8, new Horse(Color.Black, BoardOfMatch));
            PutNewPiece('h', 8, new Rook(Color.Black, BoardOfMatch));
            PutNewPiece('a', 7, new Pawn(Color.Black, BoardOfMatch));
            PutNewPiece('b', 7, new Pawn(Color.Black, BoardOfMatch));
            PutNewPiece('c', 7, new Pawn(Color.Black, BoardOfMatch));
            PutNewPiece('d', 7, new Pawn(Color.Black, BoardOfMatch));
            PutNewPiece('e', 7, new Pawn(Color.Black, BoardOfMatch));
            PutNewPiece('f', 7, new Pawn(Color.Black, BoardOfMatch));
            PutNewPiece('g', 7, new Pawn(Color.Black, BoardOfMatch));
            PutNewPiece('h', 7, new Pawn(Color.Black, BoardOfMatch));
        }
    }
}
