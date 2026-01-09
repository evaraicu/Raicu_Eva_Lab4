using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Raicu_Eva_Lab4.Data;   
using Raicu_Eva_Lab4.Models; 

namespace Raicu_Eva_Lab4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictionApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PredictionApiController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetPredictions()
        {
            if (_context.PredictionHistory == null)
            {
                return NotFound("Tabelul PredictionHistory nu exista.");
            }

            return Ok(_context.PredictionHistory.ToList());
        }

        // Metoda DELETE: Sterge dupa ID
        [HttpDelete("{id}")]
        public IActionResult DeletePrediction(int id)
        {
            if (_context.PredictionHistory == null)
            {
                return NotFound();
            }

            var prediction = _context.PredictionHistory.Find(id);
            if (prediction == null)
            {
                return NotFound($"Predicția cu ID-ul {id} nu a fost găsită.");
            }

            _context.PredictionHistory.Remove(prediction);
            _context.SaveChanges();

            return Ok($"Predicția cu ID-ul {id} a fost ștearsă.");
        }
    }
}