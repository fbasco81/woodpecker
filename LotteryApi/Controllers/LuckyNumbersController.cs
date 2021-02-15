using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LotteryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LuckyNumbersController : ControllerBase
    {
        
        private readonly ILogger<LuckyNumbersController> _logger;

        public LuckyNumbersController(ILogger<LuckyNumbersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<int> Get()
        {
            var allNumbers = new List<int>();
            var rng = new Random();
                
            while (allNumbers.Count < 6){
                var n = rng.Next(1, 90);
                if (!allNumbers.Any(x => x == n)){
                    allNumbers.Add(n);
                }
                
            }
            return allNumbers.ToArray();
        }
    }
}
