using board;
using Microsoft.VisualBasic;
using System;

namespace Chess
{
    class Screen
    {
        public static void PrintBoard(Board board)
        {
            for(int i = 0; i < board.Lines; i++)
            {
                for(int j = 0; j < board.Columns; j++)
                {
                    if (board.UniquePiece(i, j) == null)
                    {
                        Console.Write("-");
                    }
                    Console.Write(board.UniquePiece(i, j) + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
