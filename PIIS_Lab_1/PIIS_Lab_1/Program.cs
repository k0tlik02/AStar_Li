using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;

namespace PIIS_Lab_1
{   

    class Program
    {

        
        static void Main(string[] args)
        {
            int[,] field = { { 0, 0, 0, 0, 1, 0, 1, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, { 0, 0, 1, 0, 1, 1, 0, 0, 1, 0 }, { 0, 1, 0, 1, 0, 1, 0, 0, 1, 1 }, { 0, 1, 0, 0, 0, 1, 1, 0, 0, 0 }, { 0, 1, 0, 1, 0, 0, 1, 1, 0, 1 }, { 1, 0, 0, 1, 1, 1, 0, 0, 1, 0 }, { 0, 0, 0, 1, 1, 0, 0, 1, 0, 0 }, { 0, 1, 0, 0, 1, 1, 1, 0, 1, 0 }, { 1, 0, 0, 0, 1, 0, 1, 0, 1, 0 } };

            int n = 0;
            n = Convert.ToInt32(Console.ReadLine());
            int[,] Field = new int[n, n];
            List<(int, int)> solution = new List<(int, int)>();

          //  Write_field(ref field, 2, 3, 3, 5);
            GenerationOfTheField(ref n, ref Field);

            WriteField(ref Field, 15, 5, ref solution);

            Algo_Li(ref Field, ref solution, 7, 13, 15, 5);

            Console.WriteLine("\n\n\n");

            WriteField(ref Field, 15, 5, ref solution);

          //  Console.WriteLine("\n\n\n");

            
        }

        static void GenerationOfTheField(ref  int n, ref int[,] Field)
        {
            Random rnd = new Random();
            
           
            double d_blocks = 0.17 * n * n;

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    Field[i,j] = 0;
                }
            Math.Round(d_blocks);
            while (d_blocks>0)
            {
                int i = rnd.Next(0, 23);
                int j = rnd.Next(0, 23);
                if (Field[i, j] == 0)
                    Field[i, j] = -1;
                else
                {
                    while (Field[i, j] != 0)
                    {
                        i = rnd.Next(0, 23);
                        j = rnd.Next(0, 23);
                    }
                    Field[i, j] = -1;
                }
                d_blocks--;
            }
        }


        static void Algo_Li(ref int[,] Field, ref List<(int, int)> solution, int a1, int a2, int b1, int b2)
        {
            int i = a1;
            int j = a2;
            int count = 0;
            int border = Field.GetUpperBound(0);
           
            if (Field[i + 1, j] <= border && Field[i + 1, j] != -1) Field[i + 1, j] = 1;
            if (Field[i - 1, j] >= 0 && Field[i - 1, j] != -1) Field[i - 1, j] = 1;
            if (Field[i, j + 1] <= border && Field[i, j + 1] != -1) Field[i, j + 1] = 1;
            if (Field[i, j - 1] >= 0 && Field[i, j - 1] != -1) Field[i, j - 1] = 1;

            bool b = true;
            while (b)
            {
                count++;
                for (int i1 = 1; i1 < border; i1++)
                {
                    for (int i2 = 1; i2 < border; i2++)
                    {
                        if (Field[i1, i2] == count)
                        {
                            if (((i1 + 1) == b1 && i2 == b2) || ((i1 - 1) == b1 && i2 == b2) || (i1 == b1 && (i2 + 1) == b2) || (i1 == b1 && (i2 - 1) == b2))
                            {
                                b = false;
                                break; 
                            }


                            if (Field[i1 + 1, i2] == 0) Field[i1 + 1, i2] = count + 1;
                            if (Field[i1 - 1, i2] == 0) Field[i1 - 1, i2] = count + 1;
                            if (Field[i1, i2 + 1] == 0) Field[i1, i2 + 1] = count + 1;
                            if (Field[i1, i2 - 1] == 0) Field[i1, i2 - 1] = count + 1;
                        }
                    }
                }

            }

            i = b1;
            j = b2;
            //solution.Add((b1, b2));
            
            while (count>0)
            {
                if (Field[i + 1, j] == count) { solution.Add((i + 1, j)); i += 1; count--; continue; }
                if (Field[i - 1, j] == count) { solution.Add((i - 1, j)); i -= 1; count--; continue; }
                if (Field[i, j + 1] == count) { solution.Add((i, j + 1)); j += 1; count--; continue; }
                if (Field[1, j - 1] == count) { solution.Add((i, j - 1)); j += 1; count--; continue; }
            }
        }

        static void WriteField(ref int[,] Field, int b1, int b2, ref List<(int, int)> solution)
        {
            int rows = Field.GetUpperBound(0) + 1;  
            int columns = Field.Length / rows;
            bool b = true;
            for (int i=0; i<rows; i++)
            {
                for (int j=0; j<columns; j++)
                {
                    b = true;
                    for (int k = 0; k < solution.Count; k++)
                    {
                        if (i == solution[k].Item1 && j == solution[k].Item2)
                        {
                            b = false;
                            if (Field[i, j] / 10 == 0)
                            {
                                Console.BackgroundColor = ConsoleColor.Yellow;
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write(" " + Field[i, j]);
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Yellow;
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write(Field[i, j]);
                                Console.ResetColor();
                            }
                        }
                    }
                    if (b == true)
                    {
                        if (i == 7 && j == 13)
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(" A");
                            Console.ResetColor();
                        }
                        else if (i == b1 && j == b2)
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(" B");
                            Console.ResetColor();
                        }
                        else if (Field[i, j] == 0)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write(" 0");
                            Console.ResetColor();
                        }
                        else if (Field[i, j] < 0)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("  ");
                            Console.ResetColor();
                        }
                        else if (Field[i, j] / 10 == 0)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write(" " + Field[i, j]);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write(Field[i, j]);
                            Console.ResetColor();
                        }
                    }
                    else continue;

                }
                Console.WriteLine();
            }
        }
    }
}
