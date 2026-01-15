using Raicu_Eva_Lab4.Models;
using Raicu_Eva_Lab4;
namespace Raicu_Eva_Lab4.Models
{
    public class BatchPredictionViewModel
    {
        public IFormFile? File { get; set; }
        // Rezultatele (predicțiile) pentru fiecare rând din fișier
        public List<Raicu_Eva_Lab4.PricePredictionModel.ModelOutput>? Predictions { get; set; }
        public string? ErrorMessage { get; set; }
    }

  }
