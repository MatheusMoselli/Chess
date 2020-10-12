﻿using board;
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
                ChessMatch match = new ChessMatch();

                while (!match.Ended)
                {
                    try
                    {
                        Console.Clear();
                        Screen.PrintBoard(match.BoardOfMatch);
                        Console.WriteLine();
                        Console.WriteLine("Turn: " + match.Turn);
                        Console.WriteLine("Waiting for: " + match.ActualPlayer);

                        Console.WriteLine();

                        Console.Write("Origin: ");
                        Position origin = Screen.ReadChessPosition().ToPosition();

                        match.ValidateOriginPosition(origin);

                        bool[,] possiblePositions = match.BoardOfMatch.UniquePiece(origin).PossibleMoviments();

                        Console.Clear();
                        Screen.PrintBoard(match.BoardOfMatch, possiblePositions);

                        Console.WriteLine();
                        Console.Write("Destiny: ");
                        Position destiny = Screen.ReadChessPosition().ToPosition();
                        match.ValidateDestinyPosition(origin, destiny);

                        match.PerformPlay(origin, destiny);
                    } catch (BoardException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }
                }
            }
            catch (BoardException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
