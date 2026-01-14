namespace Raicu_Eva_Lab4.Models
{
    public class PriceBucketStat
    {
        public string Label { get; set; } = string.Empty; // Trebuie să fie Label, nu Range
        public int Count { get; set; }
    }
}