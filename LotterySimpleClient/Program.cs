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
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            // SimpleRepeat(1);
            // ParallelRepeat(1);
            // LotteryActorSystem.Run(1);

            await SimpleRepeat(3000);
            await ParallelTask(3000);
            await ParallelByBuckets(3000);
            await LotteryActorSystem.Run(3000);

            // SimpleRepeat(50000);
            // ParallelRepeat(50000);
            //await LotteryActorSystem.Run(50000);

        }

        private static async Task SimpleRepeat(int howMany){
            
            ConsoleUtils.PrintBeginHeader("SimpleRepeat", howMany);
            
            var stopwatch = new System.Diagnostics.Stopwatch(); 
            stopwatch.Start();
            for (var i=0; i<howMany; i++){
                var result = await GetLuckyNumber();
            }
            stopwatch.Stop();

            ConsoleUtils.PrintEndHeader("SimpleRepeat", howMany, stopwatch);
        }

        private static async Task ParallelTask(int howMany){
            ConsoleUtils.PrintBeginHeader("ParallelRepeat", howMany);
            
            var stopwatch = new System.Diagnostics.Stopwatch(); 
            stopwatch.Start();
            
            var tasks = Enumerable.Range(0, howMany)
                        .Select(i => GetLuckyNumber());
            await Task.WhenAll(tasks);

            stopwatch.Stop();
            ConsoleUtils.PrintEndHeader("ParallelRepeat", howMany, stopwatch);
            
        }

        private static async Task ParallelByBuckets(int howMany){
            ConsoleUtils.PrintBeginHeader("ParallelByBuckets", howMany);
            
            var stopwatch = new System.Diagnostics.Stopwatch(); 
            stopwatch.Start();
            
            // Divide into groups.	
            var parallelGroups = Enumerable.Range(0, howMany)
                                            .GroupBy(r => (r % Config.MaxDegreeOfParallelism));	
            var parallelTasks = parallelGroups.Select(groups =>	
            {	    
                return Task.Run(async () =>
                {
                    foreach (var i in groups)
                    {
                        await GetLuckyNumber();
                    }
                });	
            });	
            await Task.WhenAll(parallelTasks);

            stopwatch.Stop();
            ConsoleUtils.PrintEndHeader("ParallelByBuckets", howMany, stopwatch);
            
        }

        private static void ParallelFor(int howMany){
            ConsoleUtils.PrintBeginHeader("ParallelFor", howMany);
            
            var stopwatch = new System.Diagnostics.Stopwatch(); 
            stopwatch.Start();
            
            Parallel.For(0,howMany, async (a) => {
                 var result = await GetLuckyNumber();

            });
            
            stopwatch.Stop();
            ConsoleUtils.PrintEndHeader("ParallelFor", howMany, stopwatch);
            
        }

        private static async Task<string> GetLuckyNumber()
        {
            var stringTask = client.GetStringAsync(Config.ApiUrl);
            var msg = await stringTask;
            return msg;
        }
    }
}
