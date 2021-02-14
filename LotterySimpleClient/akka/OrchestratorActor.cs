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
       private int _luckyNumbers = 0;
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
           
            var props = Props.Create<LotteryActorWorker>(httpClient).WithRouter(new RoundRobinPool(10));
            _workerActor = Context.ActorOf(props, "worker");
            
        }

        /// <summary>
        /// Used to define all of our <see cref="Receive"/> hooks for <see cref="HttpDownloaderActor"/>
        /// </summary>
        private void Initialize()
        {
            Receive<Start>(message => 
            {
                for (var i=0; i<_howMany; i++){
                    _workerActor.Tell(new AskForNumbers());
                }
            });

            Receive<NumbersReceived>(message => 
            {
                _luckyNumbers ++;
                if (_luckyNumbers == _howMany){
                    Context.System.Terminate();
                }
            });
        }
    }
}