namespace CustomerLeaderboard.Api.Models
{
    public class Customer
    {
        public long CustomerId { get; set; }
        public decimal Score { get; set; } = 0;
        public int Rank { get; set; }
    }
}
