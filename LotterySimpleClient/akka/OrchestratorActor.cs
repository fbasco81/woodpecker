using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Akka.Actor;
using System.Net.Http.Headers;
using Akka.Routing;
namespace LotterySimpleClient.Akka
{
    public class OrchestratorActor : ReceiveActor
    {
       private int _receivedLuckyNumbers = 0;
       private int _howMany;
       private IActorRef _workerActor;
        public OrchestratorActor(int howMany)
        {
            _howMany = howMany;
            Initialize();

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
           
            var props = Props.Create<LotteryActorWorker>(httpClient).WithRouter(new RoundRobinPool(Config.MaxDegreeOfParallelism));
            _workerActor = Context.ActorOf(props);
            
        }
        private void Initialize()
        {
            Receive<Start>(message => 
            {
                // var startingBatchSize = (_howMany > Config.MaxDegreeOfParallelism * 20 
                //     ? Config.MaxDegreeOfParallelism * 20 
                //     : _howMany);
                // for (var i=0; i<startingBatchSize; i++){
                //     _workerActor.Tell(new AskForNumbers());
                // }
                for (var i=0; i<_howMany; i++){
                    _workerActor.Tell(new AskForNumbers());
                }
            });

            Receive<NumbersReceived>(message => 
            {
                _receivedLuckyNumbers++;
                // if (_receivedLuckyNumbers < _howMany){
                //     _workerActor.Tell(new AskForNumbers());
                // }
                //else
                if (_receivedLuckyNumbers == _howMany){
                    Console.WriteLine("Terminate");
                    Context.System.Terminate();
                }
            });
        }
    }
}