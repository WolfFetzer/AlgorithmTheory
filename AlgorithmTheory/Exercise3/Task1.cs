using System;
using System.Collections.Generic;
using System.Text;

namespace Execrise3
{
    public enum EditOperations
    {
        Insert, Delete, Substitute, None
    }
    
    public static class Task1
    {
        private static int[,] Matrix;
        private static Stack<EditOperations> EditStack;
        
        
        public static void Execute()
        {
            CompareStrings("informatics", "interpolation");
            CompareStrings("abac", "baac");
        }

        private static void CompareStrings(string s0, string s1)
        {
            EditStack = new Stack<EditOperations>();
            Matrix = GetEditDistance(s0, s1);
            PrintMatrix(Matrix, $"Edit-Distance ({s0} to {s1}): ");
            EditOperations(Matrix.GetLength(0) - 1, Matrix.GetLength(1) - 1);
            PrintPath(Matrix, $"Path ({s0} to {s1}): ");
            PrintStack(EditStack, s0, s1);
            
            PrintSeparation();

            Matrix = null;
            EditStack = null;
        }

        private static int[,] GetEditDistance(string s0, string s1)
        {
            int columns = s0.Length;
            int rows = s1.Length;
            int[,] matrix = new int[columns + 1, rows + 1];
            
            //Fill first row and column with raising numbers
            matrix[0, 0] = 0;
            for (int x = 1; x <= columns; x++) matrix[x, 0] = x;
            for (int y = 1; y <= rows; y++) matrix[0, y] = y;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    matrix[x + 1, y + 1] = MinInt(
                        matrix[x, y + 1] + 1, 
                        matrix[x + 1, y] + 1, 
                        matrix[x, y] + (s0[x] != s1[y] ? 1 : 0)
                    );
                }
            }

            return matrix;
        }
        
        private static void EditOperations(int x, int y)
        {
            if(x == 0 && y == 0) return;
            if (x != 0 && Matrix[x, y] == Matrix[x - 1, y] + 1)
            {
                // Delete
                Matrix[x, y] = 90;
                EditStack.Push(Execrise3.EditOperations.Delete);
                EditOperations(x - 1, y);
            }
            else if (y != 0 && Matrix[x, y] == Matrix[x, y - 1] + 1)
            {
                // Insert
                Matrix[x, y] = 80;
                EditStack.Push(Execrise3.EditOperations.Insert);
                EditOperations(x, y - 1);
            }
            else if(Matrix[x, y] != Matrix[x - 1, y - 1])
            {
                // Replace
                Matrix[x, y] = 70;
                EditStack.Push(Execrise3.EditOperations.Substitute);
                EditOperations(x - 1, y - 1);
            }
            else
            {
                Matrix[x, y] = 60;
                EditStack.Push(Execrise3.EditOperations.None);
                EditOperations(x - 1, y - 1);
            }
            
        }

        private static int MinInt(int i0, int i1, int i2)
        {
            return Math.Min(Math.Min(i0, i1), i2);
        }

        private static void PrintMatrix(int[,] matrix, string label = "Print: ")
        {
            int columns = matrix.GetLength(0);
            int rows = matrix.GetLength(1);

            string s = label + "\n";
            for (int y = 0; y < rows; y++)
            {
                s += "[";
                
                for (int x = 0; x < columns; x++)
                {
                    if (matrix[x, y] < 10) s += "  ";
                    else s += " ";
                    s +=  matrix[x, y] + "  ";
                }
                
                s += "]\n";
            }

            Console.WriteLine(s);
        }
        
        private static void PrintPath(int[,] matrix, string label = "Print: ")
        {
            int columns = matrix.GetLength(0);
            int rows = matrix.GetLength(1);

            string s = label + "\n";
            for (int y = 0; y < rows; y++)
            {
                s += "[";
                
                for (int x = 0; x < columns; x++)
                {
                    if (matrix[x, y] < 10) s += "  ";
                    else s += " ";

                    if (matrix[x, y] == 90)
                    {
                        s +=  "Le  ";
                    }
                    else if (matrix[x, y] == 80)
                    {
                        s +=  "Up  ";
                    }
                    else if (matrix[x, y] == 70)
                    {
                        s +=  "D1  ";
                    }
                    else if (matrix[x, y] == 60)
                    {
                        s +=  "D0  ";
                    }
                    else
                    {
                        s +=  matrix[x, y] + "  ";
                    }
                }
                
                s += "]\n";
            }

            Console.WriteLine(s);
        }

        private static void PrintStack(Stack<EditOperations> stack, string s0, string s1)
        {
            string s = $"Edit operations to convert {s0} to {s1}\n";
            int index = 0;
            StringBuilder stringBuilder = new StringBuilder(s0);
            
            foreach (EditOperations editOperation in stack)
            {
                switch (editOperation)
                {
                    case Execrise3.EditOperations.Insert:
                        stringBuilder.Insert(index, s1[index]);
                        index++;
                        break;
                    case Execrise3.EditOperations.Delete:
                        stringBuilder.Remove(index, 1);
                        break;
                    case Execrise3.EditOperations.Substitute:
                        stringBuilder[index] = s1[index];
                        index++;
                        break;
                    case Execrise3.EditOperations.None:
                        index++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                s += $"{editOperation}: {stringBuilder}\n";
            }

            Console.WriteLine(s);
        }

        private static void PrintSeparation()
        {
            Console.WriteLine("|\n--------------------------------------------------------------------------------------------------------------------------------------\n|");
        }
    }
}