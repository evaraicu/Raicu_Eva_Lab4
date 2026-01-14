using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Raicu_Eva_Lab4.Models;
using Raicu_Eva_Lab4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raicu_Eva_Lab4.Controllers
{
    public class PredictionController : Controller
    {
        private readonly AppDbContext _context;

        public PredictionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Price()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Price(Raicu_Eva_Lab4.PricePredictionModel.ModelInput input)
        {
            MLContext mlContext = new MLContext();
            ITransformer mlModel = mlContext.Model.Load(@"PricePredictionModel.mlnet", out var modelInputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<Raicu_Eva_Lab4.PricePredictionModel.ModelInput, Raicu_Eva_Lab4.PricePredictionModel.ModelOutput>(mlModel);
            Raicu_Eva_Lab4.PricePredictionModel.ModelOutput result = predEngine.Predict(input);

            ViewBag.Price = result.Score;

            var history = new PredictionHistory
            {
                PassengerCount = input.Passenger_count,
                TripTimeInSecs = input.Trip_time_in_secs,
                TripDistance = input.Trip_distance,
                PaymentType = input.Payment_type,
                PredictedPrice = result.Score,
                CreatedAt = DateTime.Now
            };

            _context.PredictionHistory.Add(history);
            await _context.SaveChangesAsync();

            return View(input);
        }

        [HttpGet]
        public async Task<IActionResult> History()
        {
            var history = await _context.PredictionHistory
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(history);
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var totalPredictions = await _context.PredictionHistory.CountAsync();

            var paymentTypeStats = await _context.PredictionHistory
                .GroupBy(p => p.PaymentType)
                .Select(g => new PaymentTypeStat
                {
                    PaymentType = g.Key,
                    AveragePrice = (double)g.Average(x => x.PredictedPrice),
                    Count = g.Count()
                })
                .ToListAsync();

            var allPredictions = await _context.PredictionHistory
                .Select(p => (double)p.PredictedPrice)
                .ToListAsync();

            var buckets = new List<PriceBucketStat>
            {
                new PriceBucketStat { Label = "0 - 10", Count = 0 },
                new PriceBucketStat { Label = "10 - 20", Count = 0 },
                new PriceBucketStat { Label = "20 - 30", Count = 0 },
                new PriceBucketStat { Label = "30 - 50", Count = 0 },
                new PriceBucketStat { Label = "> 50", Count = 0 }
            };

            foreach (var price in allPredictions)
            {
                if (price < 10) buckets[0].Count++;
                else if (price < 20) buckets[1].Count++;
                else if (price < 30) buckets[2].Count++;
                else if (price < 50) buckets[3].Count++;
                else buckets[4].Count++;
            }

            var vm = new DashboardViewModel
            {
                TotalPredictions = totalPredictions,
                PaymentTypeStats = paymentTypeStats,
                PriceBuckets = buckets
            };

            return View(vm);
        }
    }
}