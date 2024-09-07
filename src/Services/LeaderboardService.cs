using CustomerLeaderboard.Api.Models;
using CustomerLeaderboard.Api.Repositories;

namespace CustomerLeaderboard.Api.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly ICustomerRepository _customerRepository;
        private static readonly SortedSet<Customer> _leaderboard = new SortedSet<Customer>(new ScoreComparer());
        private static readonly object _lock = new object();

        public LeaderboardService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            InitializeLeaderboard();
        }

        private void InitializeLeaderboard()
        {
            lock (_lock)
            {
                foreach (var customer in _customerRepository.GetAllCustomers())
                {
                    _leaderboard.Add(customer);
                }
                UpdateRanks();
            }
        }

        public async Task UpdateScoreAsync(long customerId, decimal scoreDelta)
        {
            await Task.Run(() =>
            {
                lock (_lock)
                {
                    // 获取客户信息
                    var customer = _customerRepository.GetCustomer(customerId);

                    if (customer != null)
                    {
                        // 如果客户存在，首先从排行榜中移除
                        _leaderboard.Remove(customer);

                        // 更新客户分数
                        customer.Score += scoreDelta;

                        // 根据新分数决定是否重新加入排行榜
                        if (customer.Score > 0)
                        {
                            _leaderboard.Add(customer);  // 重新添加到排行榜
                            _customerRepository.AddOrUpdateCustomer(customer);  // 更新存储库
                        }
                        else
                        {
                            _customerRepository.RemoveCustomer(customerId);  // 从存储库移除客户
                        }
                    }
                    else
                    {
                        // 如果客户不存在且分数增量大于0，则创建新客户
                        if (scoreDelta > 0)
                        {
                            customer = new Customer { CustomerId = customerId, Score = scoreDelta };
                            _leaderboard.Add(customer);  // 添加到排行榜
                            _customerRepository.AddOrUpdateCustomer(customer);  // 添加到存储库
                        }
                    }

                    // 更新排名
                    UpdateRanks();
                }
            });
        }

        private void UpdateRanks()
        {
            int rank = 1;
            foreach (var customer in _leaderboard)
            {
                customer.Rank = rank++;
            }
        }

        public async Task<List<LeaderboardEntry>> GetCustomersByRankAsync(int startRank, int endRank)
        {
            return await Task.Run(() =>
            {
                lock (_lock)
                {
                    return _leaderboard.Skip(startRank - 1).Take(endRank - startRank + 1)
                        .Select(c => new LeaderboardEntry
                        {
                            CustomerId = c.CustomerId,
                            Score = c.Score,
                            Rank = c.Rank
                        })
                        .ToList();
                }
            });
        }

        public async Task<List<LeaderboardEntry>> GetCustomerAndNeighborsAsync(long customerId, int high, int low)
        {
            return await Task.Run(() =>
            {
                lock (_lock)
                {
                    var customer = _customerRepository.GetCustomer(customerId);
                    if (customer == null)
                    {
                        return new List<LeaderboardEntry>();
                    }

                    var rankedCustomers = _leaderboard.ToList();
                    var index = rankedCustomers.IndexOf(customer);

                    if (index == -1)
                    {
                        return new List<LeaderboardEntry>();
                    }

                    var higherNeighbors = rankedCustomers
                        .Take(index)
                        .Reverse()
                        .Take(high)
                        .Select(c => new LeaderboardEntry
                        {
                            CustomerId = c.CustomerId,
                            Score = c.Score,
                            Rank = c.Rank
                        })
                        .ToList();

                    var lowerNeighbors = rankedCustomers
                        .Skip(index + 1)
                        .Take(low)
                        .Select(c => new LeaderboardEntry
                        {
                            CustomerId = c.CustomerId,
                            Score = c.Score,
                            Rank = c.Rank
                        })
                        .ToList();

                    var result = higherNeighbors
                        .Concat(new List<LeaderboardEntry>
                        {
                            new LeaderboardEntry
                            {
                                CustomerId = customer.CustomerId,
                                Score = customer.Score,
                                Rank = customer.Rank
                            }
                        })
                        .Concat(lowerNeighbors)
                        .OrderBy(entry => entry.Rank)
                        .ToList();

                    return result;
                }
            });
        }
    }
}
