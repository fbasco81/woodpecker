using System;
using System.Diagnostics;
namespace LotterySimpleClient{
    class ConsoleUtils
    {
        public static void PrintBeginHeader(string methodName, int howMany)
        {
            Console.WriteLine($"---------------------------------------");
            Console.WriteLine($"Start {methodName} for {howMany} times");
        }

        public static void PrintEndHeader(string methodName, int howMany, Stopwatch stopwatch)
        { 
            Console.WriteLine($"End {methodName} for {howMany} time. Elapsed {stopwatch.Elapsed}");
            Console.WriteLine($"---------------------------------------");
        }
    }
}