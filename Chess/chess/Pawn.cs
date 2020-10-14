using board;

namespace chess
{
    class Pawn : Piece
    {
        public Pawn(Color color, Board board) : base(color, board)
        {
        }

        public override string ToString()
        {
            return "P";
        }

        private bool IsThereEnemy(Position position)
        {
            Piece p = Board.UniquePiece(position);
            return p != null && p.Color != Color;
        }

        private bool Free(Position position)
        {
            return Board.UniquePiece(position) == null;
        }

        public override bool[,] PossibleMoviments()
        {
            bool[,] mat = new bool[Board.Lines, Board.Columns];
            Position pos = new Position(0, 0);

            if (Color == Color.White)
            {
                pos.DefineValues(Position.Line - 1, Position.Column);
                if (Board.IsPositionValid(pos) && Free(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.DefineValues(Position.Line - 2, Position.Column);
                Position p2 = new Position(Position.Line - 1, Position.Column);
                if (Board.IsPositionValid(p2) && Free(p2)&& Board.IsPositionValid(pos) && Free(pos) && ManyMoves == 0)
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.DefineValues(Position.Line - 1, Position.Column - 1);
                if (Board.IsPositionValid(pos) && IsThereEnemy(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.DefineValues(Position.Line - 1, Position.Column + 1);
                if (Board.IsPositionValid(pos) && IsThereEnemy(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }
            }
            else
            {
                pos.DefineValues(Position.Line + 1, Position.Column);
                if (Board.IsPositionValid(pos) && Free(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                Position p2 = new Position(Position.Line + 1, Position.Column);
                pos.DefineValues(Position.Line + 2, Position.Column);
                if (Board.IsPositionValid(p2) && Free(p2) && Board.IsPositionValid(pos) && Free(pos) && ManyMoves == 0)
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.DefineValues(Position.Line + 1, Position.Column + 1);
                if (Board.IsPositionValid(pos) && IsThereEnemy(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }

                pos.DefineValues(Position.Line + 1, Position.Column - 1);
                if (Board.IsPositionValid(pos) && IsThereEnemy(pos))
                {
                    mat[pos.Line, pos.Column] = true;
                }
            }
            return mat;
        }

    }
}
