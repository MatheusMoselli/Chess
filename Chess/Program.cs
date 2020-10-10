using Board;
using System;

namespace Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            Position p = new Position(3, 4);

            Console.WriteLine("Position: " + p);

            Console.ReadLine();
        }
    }
}
