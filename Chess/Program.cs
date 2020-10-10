using board;
using chess;
using System;

namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Board board = new Board(8, 8);

                board.PutPiece(new Tower(Color.Black, board), new Position(0, 0));
                board.PutPiece(new Tower(Color.Black, board), new Position(5, 2));
                board.PutPiece(new King(Color.White, board), new Position(3, 4));
                board.PutPiece(new King(Color.White, board), new Position(0, 7));

                Screen.PrintBoard(board);
            }
            catch(BoardException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
