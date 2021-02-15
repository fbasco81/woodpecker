using System;
using Akka.Actor;
using Akka.Routing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
namespace LotterySimpleClient.Akka
{
    public class LotteryActorSystem
    {
        public static ActorSystem myLotteryActorSystem;

        public static async Task Run(int howMany)
        {
             var stopwatch = new System.Diagnostics.Stopwatch(); 
            stopwatch.Start();
           
            ConsoleUtils.PrintBeginHeader("AkkaProcessing", howMany);
            
            myLotteryActorSystem = ActorSystem.Create("MyLotteryActorSystem");

            var propsOrchestrator = Props.Create<OrchestratorActor>(howMany);
            var orchestratorActor = myLotteryActorSystem.ActorOf(propsOrchestrator, "orchestrator");
            orchestratorActor.Tell(new Start());
            await myLotteryActorSystem.WhenTerminated;
            
            stopwatch.Stop();
            ConsoleUtils.PrintEndHeader("AkkaProcessing", howMany, stopwatch);

        }
    }
}