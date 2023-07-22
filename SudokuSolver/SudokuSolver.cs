using System;
using System.Collections.Generic;

namespace Sudoku
{
    class SudokuSolver
    {
        private static Random _random = new Random();
        private static List<int> _validRowList = new List<int>();
        private static List<int> _validColList = new List<int>();
        public static List<int[,]> _solutionList = new List<int[,]>();

        private static void FindSudokuSolutions(int validIndex, int[,] sudokuArray)
        {
            if (validIndex == _validRowList.Count)
            {
                int[,] solution = new int[9, 9];
                Array.Copy(sudokuArray, solution, 81);
                _solutionList.Add(solution);
                return;
            }

            int row = _validRowList[validIndex];
            int col = _validColList[validIndex];
            List<int> randomNumList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            while (randomNumList.Count > 0)
            {
                int randomNum = randomNumList[_random.Next(randomNumList.Count)];
                randomNumList.Remove(randomNum);

                if (SudokuCommon.IsValid(randomNum, row, col, sudokuArray))
                {
                    sudokuArray[row, col] = randomNum;

                    FindSudokuSolutions(validIndex + 1, sudokuArray);

                    sudokuArray[row, col] = 0;
                }
            }
        }

        private static void SolveSudoku(int[,] sudokuArray)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (sudokuArray[i, j] == 0)
                    {
                        _validRowList.Add(i);
                        _validColList.Add(j);
                    }
                }
            }

            FindSudokuSolutions(0, sudokuArray);
        }

        static void Main(string[] args)
        {
            SudokuGenerator.InitializeSudoku();
            Console.WriteLine("\nSolution:\n");
            SolveSudoku(SudokuGenerator.SudokuArray);

            foreach (var solution in _solutionList)
            {
                SudokuCommon.PrintSudoku(solution);
                Console.WriteLine();
            }
            Console.Read();
        }
    }
}
