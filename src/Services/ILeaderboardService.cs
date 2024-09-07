using CustomerLeaderboard.Api.Models;

namespace CustomerLeaderboard.Api.Services
{
    public interface ILeaderboardService
    {
        Task UpdateScoreAsync(long customerId, decimal scoreDelta);
        Task<List<LeaderboardEntry>> GetCustomersByRankAsync(int startRank, int endRank);
        Task<List<LeaderboardEntry>> GetCustomerAndNeighborsAsync(long customerId, int high, int low);
    }
}
