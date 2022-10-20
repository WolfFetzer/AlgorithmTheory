using System;

namespace Exercise2
{
    public class Tasks
    {
        public void Execute()
        {
            Exercise1();
            //Exercise2();
        }
        
        private void BottomUp(int[] matrixSequence)
        {
            int size = matrixSequence.Length - 1;
            int[,] memory = new int[size, size];
            int[,] split = new int[size, size];

            for (int lenMinusOne = 1; lenMinusOne < size; lenMinusOne++)
            {
                for (int i = 0; i < size - lenMinusOne; i++)
                {
                    int j = i + lenMinusOne;
                    memory[i, j] = int.MaxValue;

                    for (int k = i; k < j; k++)
                    {
                        int cost = memory[i, k] + memory[k + 1, j] +
                                   matrixSequence[i] * matrixSequence[k + 1] * matrixSequence[j + 1];

                        if (cost < memory[i, j])
                        {
                            memory[i, j] = cost;
                            split[i, j] = k + 1;
                        }
                    }
                }
            }

            string s = String.Empty;
            for (int row = 0; row < size; row++)
            {
                for (int column = 0; column < size; column++)
                {
                    s += "" + split[row, column] + ", ";
                }

                s += "\n";
            }

            Console.WriteLine(s);
            
            int start = split[0, size - 1];
            Console.WriteLine("Start: " + start);

            Console.WriteLine("Lower: " + split[0, start - 1]);
            Console.WriteLine("Upper: " + split[start, size - 1]);
        }
        
        
        private void Exercise1()
        {
            //int[] matrixSequence = { 10, 4, 5, 4, 5, 4, 5, 4, 5, 10, 4, 5, 4, 5, 4, 5, 7 };
            int[] matrixSequence = { 30,35,15,5,10,20,25 };
            
            Console.WriteLine("Exercise 1:");
            BottomUp(matrixSequence);
        }
        
        private void Exercise2()
        {
            
        }
    }
}