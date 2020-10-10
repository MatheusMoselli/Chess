using board;
using System;

namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            Board b = new Board(8, 8);

            Screen.PrintBoard(b);

            Console.ReadLine();
        }
    }
}
