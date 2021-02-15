using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Akka.Actor;
using System.Net.Http.Headers;

namespace LotterySimpleClient.Akka
{
    public class LotteryActorWorker : ReceiveActor
    {
        private readonly HttpClient _httpClient;
      
        public LotteryActorWorker(HttpClient httpClient)
        {
            _httpClient = httpClient;
            
            Initialize();
        }

        /// <summary>
        /// Used to define all of our <see cref="Receive"/> hooks for <see cref="HttpDownloaderActor"/>
        /// </summary>
        private void Initialize()
        {
            Receive<AskForNumbers>(message => 
            {
                _httpClient.GetStringAsync(Config.ApiUrl).ContinueWith(httpRequest =>
                {
                    if (httpRequest.IsFaulted){
                        Console.WriteLine("Error");
                    }
                   var msg = httpRequest.Result;
                    
                    return new NumbersReceived();
                }, TaskContinuationOptions.ExecuteSynchronously).PipeTo(Sender);
            });

        }
    }
}