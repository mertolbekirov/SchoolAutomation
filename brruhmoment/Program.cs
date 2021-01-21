using System;

namespace brruhmoment
{
    class Program
    {
        static void Main(string[] args)
        {

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.WriteLine($"{j} {i}");
                }
            }
        }
    }
}
