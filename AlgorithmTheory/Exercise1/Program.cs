using System;

namespace Exercise1
{
    class Program
    {
        static void Main(string[] args)
        {
            new BinaryWordsCalculator().Power(4);
        }
    }

    public abstract class Matrix<T>
    {
        protected T[,] baseMatrix;
        
        /*
        public Matrix(T[,] baseMatrix)
        {
            matrix = baseMatrix;
        }
        */

        /*
        public void Calculate(int )
        {
            
        }
        */
    }

    public class BinaryWordsCalculator : Matrix<int>
    {
        public BinaryWordsCalculator()
        {
            baseMatrix = new[,]
            {
                { 0, 1, 0 }, 
                { 0, 0, 1 },
                { 1, 0, 1 }
            };
        }

        public void Power(int pow)
        {
            int[,] matrix = baseMatrix;

            int[] rowOne = new[] { baseMatrix[0, 0], baseMatrix[0, 1], baseMatrix[0, 2] };
            int[] rowTwo = new[] { baseMatrix[1, 0], baseMatrix[1, 1], baseMatrix[1, 2] };
            int[] rowThree = new[] { baseMatrix[2, 0], baseMatrix[2, 1], baseMatrix[2, 2] };

            for (int j = 0; j < pow; j++)
            {
                switch (j % 3)
                {
                    case 0:
                        matrix[0, 0] = matrix[0, 1] + matrix[0, 2];
                        matrix[1, 0] = matrix[1, 1] + matrix[1, 2];
                        matrix[2, 0] = matrix[2, 1] + matrix[2, 2];

                        rowOne[0] = rowTwo[0] + rowThree[0];
                        rowOne[1] = rowTwo[1] + rowThree[1];
                        rowOne[2] = rowTwo[2] + rowThree[2];
                        break;
                    case 1:
                        matrix[0, 1] = matrix[0, 0] + matrix[0, 2];
                        matrix[1, 1] = matrix[1, 0] + matrix[1, 2];
                        matrix[2, 1] = matrix[2, 0] + matrix[2, 2];
                        
                        rowTwo[0] = rowOne[0] + rowThree[0];
                        rowTwo[1] = rowOne[1] + rowThree[1];
                        rowTwo[2] = rowOne[2] + rowThree[2];
                        break;
                    case 2: 
                        matrix[0, 2] = matrix[0, 0] + matrix[0, 1];
                        matrix[1, 2] = matrix[1, 0] + matrix[1, 1];
                        matrix[2, 2] = matrix[2, 0] + matrix[2, 1];
                        
                        rowThree[0] = rowOne[0] + rowTwo[0];
                        rowThree[1] = rowOne[1] + rowTwo[1];
                        rowThree[2] = rowOne[2] + rowTwo[2];
                        break;
                }
            }

            Console.WriteLine(rowOne[0] + ", " + rowOne[1] + ", " + rowOne[2]);
            Console.WriteLine(rowTwo[0] + ", " + rowTwo[1] + ", " + rowTwo[2]);
            Console.WriteLine(rowThree[0] + ", " + rowThree[1] + ", " + rowThree[2]);

            //Print(matrix);
        }

        private void Print(int[,] m)
        {
            string s = String.Empty;
            Console.WriteLine($"[{m[0,0]}, {m[1,0]}, {m[2,0]}]\n" +
                              $"[{m[0,1]}, {m[1,1]}, {m[2,1]}]\n" +
                              $"[{m[0,2]}, {m[1,2]}, {m[2,2]}]");
        }
    }
}