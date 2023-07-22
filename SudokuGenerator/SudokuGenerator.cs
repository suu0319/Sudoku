using System;
using System.Collections.Generic;

namespace Sudoku
{
    public class SudokuCommon
    {
        public Random Random = new Random();
        public List<int> ValidRowList = new List<int>();
        public List<int> ValidColList = new List<int>();
        public List<int[,]> SolutionList = new List<int[,]>();

        public static bool IsValid(int num, int row, int col, int[,] sudokuArray)
        {
            int startRow = row / 3 * 3;
            int startCol = col / 3 * 3;

            for (int i = 0; i < 8; i++)
            {
                if (sudokuArray[row, i] == num)
                {
                    return false;
                }
            }

            for (int i = 0; i < 8; i++)
            {
                if (sudokuArray[i, col] == num)
                {
                    return false;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (sudokuArray[startRow + i, startCol + j] == num)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static void PrintSudoku(int[,] sudokuArray)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write($"{sudokuArray[i, j]} ");
                }

                Console.WriteLine();
            }
        }
    }

    public static class SudokuGenerator
    {
        private static SudokuCommon _sudokuCommom = new SudokuCommon();
        public static int[,] SudokuArray = new int[9, 9];

        private static bool CanGenerateNumInSudoku(int row, int col)
        {
            if (row == 9)
            {
                Console.Write("Empty Count:");
                CanRandomResetSudoku(0, int.Parse(Console.ReadLine()), SudokuArray);
                Console.WriteLine();
                SudokuCommon.PrintSudoku(SudokuArray);
                Console.WriteLine("\n0 is empty");
                return true;
            }

            if (col == 9)
            {
                return CanGenerateNumInSudoku(row + 1, 0);
            }

            if (SudokuArray[row, col] != 0)
            {
                return CanGenerateNumInSudoku(row, col + 1);
            }

            List<int> randomNumList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            while (randomNumList.Count > 0)
            {
                int randomNum = randomNumList[_sudokuCommom.Random.Next(randomNumList.Count)];
                randomNumList.Remove(randomNum);

                if (IsValid(randomNum, row, col))
                {
                    SudokuArray[row, col] = randomNum;

                    if (CanGenerateNumInSudoku(row, col + 1))
                    {
                        return true;
                    }

                    SudokuArray[row, col] = 0;
                }
            }

            return false;
        }

        private static void FindSudokuSolutions(int validIndex, int[,] sudokuArray)
        {
            if (validIndex == _sudokuCommom.ValidRowList.Count)
            {
                int[,] solution = new int[9, 9];
                Array.Copy(sudokuArray, solution, 81);
                _sudokuCommom.SolutionList.Add(solution);

                if (_sudokuCommom.SolutionList.Count > 1)
                {
                    return;
                }

                return;
            }

            int row = _sudokuCommom.ValidRowList[validIndex];
            int col = _sudokuCommom.ValidColList[validIndex];
            List<int> randomNumList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            while (randomNumList.Count > 0)
            {
                int randomNum = randomNumList[_sudokuCommom.Random.Next(randomNumList.Count)];
                randomNumList.Remove(randomNum);

                if (SudokuCommon.IsValid(randomNum, row, col, sudokuArray))
                {
                    sudokuArray[row, col] = randomNum;

                    FindSudokuSolutions(validIndex + 1, sudokuArray);

                    sudokuArray[row, col] = 0;
                }
            }
        }

        private static bool IsValid(int num, int row, int col)
        {
            int startRow = row / 3 * 3;
            int startCol = col / 3 * 3;

            for (int i = 0; i < 8; i++)
            {
                if (SudokuArray[row, i] == num)
                {
                    return false;
                }
            }

            for (int i = 0; i < 8; i++)
            {
                if (SudokuArray[i, col] == num)
                {
                    return false;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (SudokuArray[startRow + i, startCol + j] == num)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool CanRandomResetSudoku(int emptyCount, int targetCount, int[,] sudokuArray)
        {
            if (emptyCount == targetCount)
            {
                return true;
            }

            for (int i = emptyCount; i < targetCount; i++)
            {
                int randomRow = _sudokuCommom.Random.Next(0, 9);
                int randomCol = _sudokuCommom.Random.Next(0, 9);
                int cacheNum;

                while (sudokuArray[randomRow, randomCol] == 0)
                {
                    randomRow = _sudokuCommom.Random.Next(0, 9);
                    randomCol = _sudokuCommom.Random.Next(0, 9);
                }

                cacheNum = sudokuArray[randomRow, randomCol];
                sudokuArray[randomRow, randomCol] = 0;
                _sudokuCommom.SolutionList.Clear();
                FindSudokuSolutions(0, sudokuArray);

                if (_sudokuCommom.SolutionList.Count == 1)
                {
                    if (CanRandomResetSudoku(emptyCount + 1, targetCount, sudokuArray))
                    {
                        return true;
                    }
                }

                sudokuArray[randomRow, randomCol] = cacheNum;
            }

            return false;
        }

        public static void InitializeSudoku()
        {
            CanGenerateNumInSudoku(0, 0);
        }

        static void Main(string[] args)
        {
            InitializeSudoku();
            Console.Read();
        }
    }
}
