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
        private int _asked = 0;
        private int _answered = 0;
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
                _asked ++;
                var actorName = Self.Path.Name;
                if (_asked % 25 == 0){
                    Console.WriteLine(Self.Path.Name + " asked : " + _asked.ToString());
                }
                _httpClient.GetStringAsync(Config.ApiUrl).ContinueWith(httpRequest =>
                {
                    _answered ++;
                    if (_answered % 25 == 0){
                        Console.WriteLine(actorName + " answered : " + _answered.ToString());
                    }
                
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