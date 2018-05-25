using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AzureWhiteApp;
using AzureWhiteApp.Hub;
using HdrHistogram;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace WhiteApp.Controllers
{
    public class AzureStreamerController : Controller
    {
        private readonly IHubContext<Streamer> context;

        public AzureStreamerController(IHubContext<Streamer> context)
        {
            this.context = context;
        }

        [Route("/api/signalr-streamer/")]
        [HttpGet]
        public void SignalRStreamer()
        {

            var histogram = new LongHistogram(TimeStamp.Hours(1), 3);

            var writer = new StringWriter();

            Task.Run(async () =>
            {
                for (var i = 1; i < 1000;i++)
                {

                    long startTimestamp = Stopwatch.GetTimestamp();

                    var currency = new Currency()
                    {
                        Id = 1,
                        CurrencyType = "EUR/USD",
                        Price = i,
                        Timestamp = startTimestamp.ToString()
                    };

                    var cur = JsonConvert.SerializeObject(currency);
                    await context.Clients.Group("myGroup").SendAsync("broadcastMessage", "currency", cur).ConfigureAwait(false);

                    long elapsed = Stopwatch.GetTimestamp() - startTimestamp;
                    histogram.RecordValue(elapsed);

                    await Task.Delay(50).ConfigureAwait(false);
                }
                var scalingRatio = OutputScalingFactor.TimeStampToMilliseconds;
                histogram.OutputPercentileDistribution(
                    writer,
                    outputValueUnitScalingRatio: scalingRatio);
                System.IO.File.WriteAllText(@"d:\cloud\signalr.txt", writer.ToString());

            });
        }

    }


}