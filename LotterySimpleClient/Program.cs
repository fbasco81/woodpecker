using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using LotterySimpleClient.Akka;
namespace LotterySimpleClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            
            // SimpleRepeat(1);
            // ParallelRepeat(1);
            // LotteryActorSystem.Run(1);

             SimpleRepeat(10000);
             ParallelRepeat(10000);
            await LotteryActorSystem.Run(10000);

            // SimpleRepeat(50000);
            // ParallelRepeat(50000);
            //await LotteryActorSystem.Run(50000);

        }

        private static void SimpleRepeat(int howMany){
            Console.WriteLine($"Start SimpleRepeat for {howMany} times");

            var stopwatch = new System.Diagnostics.Stopwatch(); 
            stopwatch.Start();
            for (var i=0; i<howMany; i++){
                var result = GetLuckyNumber().Result;
            }
            stopwatch.Stop();
            Console.WriteLine($"End SimpleRepeat for {howMany} time. Elapsed {stopwatch.Elapsed}");
            
        }

        private static async void ParallelRepeat(int howMany){
            Console.WriteLine($"Start ParallelRepeat for {howMany} times");

            var stopwatch = new System.Diagnostics.Stopwatch(); 
            stopwatch.Start();
            
            var tasks = Enumerable.Range(0, howMany)
                        .Select(i => GetLuckyNumber());
            await Task.WhenAll(tasks);

            // Parallel.For(0,howMany, (a) => {
            //     var result = GetLuckyNumber().Result;

            // });
            
            stopwatch.Stop();
            Console.WriteLine($"End ParallelRepeat for {stopwatch.Elapsed} time. Elapsed {stopwatch.Elapsed}");
            
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
