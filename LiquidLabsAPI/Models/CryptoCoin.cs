using System.ComponentModel.DataAnnotations;

namespace LiquidLabsAPI.Models
{
    public class CryptoCoin
    {
        [Key]
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal CurrentPrice { get; set; }
        public long MarketCap { get; set; }
        public int MarketCapRank { get; set; }
        public Int64 FullyDilutedValuation { get; set; }
        public Int64 TotalVolume { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
