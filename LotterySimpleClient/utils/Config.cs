using System;
using System.Diagnostics;
namespace LotterySimpleClient{
    class Config
    {
        public static string ApiUrl = "https://lottery-web-api.azurewebsites.net/LuckyNumbers";
        public static int MaxDegreeOfParallelism = 10;
    }
}
