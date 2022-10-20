using System;

namespace Execrise3
{
    public static class Task4
    {
        public static void Execute()
        {
            int[,] values = Task4Values.SmallTestValues;
            int rows = values.GetLength(0);
            int maxColumns = values.GetLength(1);

            int[] memory = new int[maxColumns - 1];
            for (int x = 0; x < maxColumns - 1; x++)
            {
                memory[x] = Math.Max(values[rows - 1, x], values[rows - 1, x + 1]);
            }

            maxColumns--;
            
            for (int y = rows - 2; y > 0; y--)
            {
                for (int x = 0; x < maxColumns - 1; x++)
                {
                    memory[x] = Math.Max(values[y, x] + memory[x], values[y, x + 1] + memory[x + 1]);
                }

                maxColumns--;
            }

            Console.WriteLine("Ergebnis: " + (memory[0] + values[0, 0]));
        }
    }

    public static class Task4Values
    {
        public static int[,] SmallTestValues = new int[,]
        {
            { 3, 0, 0, 0 },
            { 7, 4, 0, 0 },
            { 2, 4, 6, 0 },
            { 8, 5, 9, 3 }
        };

    } 
}