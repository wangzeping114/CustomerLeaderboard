namespace CustomerLeaderboard.Api.Models
{
    public class LeaderboardEntry
    {
        /// <summary>
        /// CustomerId
        /// </summary>
        public long CustomerId { get; set; }

        /// <summary>
        /// Score
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// Rank
        /// </summary>
        public int Rank { get; set; } = 0;
    }
}
