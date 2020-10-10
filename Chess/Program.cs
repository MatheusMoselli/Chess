using board;
using chess;
using System;

namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            Board b = new Board(8, 8);

            b.PutPiece(new Tower(Color.Black, b), new Position(0, 0));
            b.PutPiece(new Tower(Color.Black, b), new Position(1, 3));
            b.PutPiece(new King(Color.Black, b), new Position(2, 4));

            Screen.PrintBoard(b);

            Console.ReadLine();
        }
    }
}
