using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
namespace LotterySimpleClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            SimpleRepeat(1);

            SimpleRepeat(1000);

            SimpleRepeat(50000);

        }

        private static void SimpleRepeat(int howMany){
            Console.WriteLine($"Start repating for {howMany} times");

            var stopwatch = new System.Diagnostics.Stopwatch(); 
            stopwatch.Start();
            for (var i=0; i<howMany; i++){
                var result = GetLuckyNumber().Result;
            }
            stopwatch.Stop();
            Console.WriteLine($"End repating for {stopwatch.Elapsed} time. Elapsed {stopwatch.Elapsed}");
            
        }

        private static async Task<string> GetLuckyNumber()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var stringTask = client.GetStringAsync("https://localhost:5001/WeatherForecast");

    var msg = await stringTask;
            return msg;
        }
    }
}
