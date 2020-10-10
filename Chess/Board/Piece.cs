﻿using board;

namespace board
{
    class Piece
    {
        public Position Position { get; set; }
        public Color Color { get; protected set; }
        public int ManyMoves { get; protected set; }
        public Board Board { get; protected set; }

        public Piece()
        {

        }

        public Piece(Position position, Color color, Board board)
        {
            Position = position;
            Color = color;
            Board = board;
            ManyMoves = 0;
        }
    }
}
