using CustomerLeaderboard.Api.Models;

namespace CustomerLeaderboard.Tests
{
    public class LeaderboardServiceTests: TestBase
    {
     
        public LeaderboardServiceTests()
        {

        }

        [Fact]
        public async Task UpdateScoreAsync_Should_UpdateCustomerScoreAndRankCorrectly()
        {
            var datas = GetCustomerData;
            foreach (var data in datas)
            {
                await LeaderboardService.UpdateScoreAsync(data.CustomerId, data.Score);
            }

            var leaderboard = await LeaderboardService.GetCustomersByRankAsync(0, 1);
            // Assert
            Assert.Equal(96144320, leaderboard.First().CustomerId);  // 第一名
            Assert.Equal(54814111, leaderboard.Last().CustomerId);   // 第二名

        }

        [Fact]
        public async Task GetCustomerAndNeighborsAsync_Should_ReturnCorrectCustomerAndNeighbors()
        {
            // Arrange
            var datas = GetCustomerData;
            foreach (var data in datas)
            {
                await LeaderboardService.UpdateScoreAsync(data.CustomerId, data.Score);
            }

            var customerId = 53274324; // 需要测试的客户ID
            var high = 2; // 需要获取的上邻近客户数量
            var low = 2; // 需要获取的下邻近客户数量

            // Act
            var result = await LeaderboardService.GetCustomerAndNeighborsAsync(customerId, high, low);

            // Assert
            var customer = result.SingleOrDefault(c => c.CustomerId == customerId);
            Assert.NotNull(customer); // 确保返回的结果中包含了指定的客户

            // 检查客户是否在列表中，并且排名位置是否正确
            var rankIndex = result.FindIndex(c => c.CustomerId == customerId);
            Assert.True(rankIndex >= 0); // 客户应该在返回的列表中

            // 检查前邻近客户
            var higherNeighbors = result.Take(rankIndex).ToList();
            Assert.Equal(high, higherNeighbors.Count); // 确保获取的上邻近客户数量正确
            Assert.All(higherNeighbors, c => Assert.True(c.Rank < customer.Rank)); // 确保上邻近客户的排名高于指定客户

            // 检查后邻近客户
            var lowerNeighbors = result.Skip(rankIndex + 1).ToList();
            Assert.Equal(low, lowerNeighbors.Count); // 确保获取的下邻近客户数量正确
            Assert.All(lowerNeighbors, c => Assert.True(c.Rank > customer.Rank)); // 确保下邻近客户的排名低于指定客户
        }



        public static IEnumerable<Customer> GetCustomerData = new List<Customer>
        {
             new Customer { CustomerId= 76786448, Score= 78 },
             new Customer { CustomerId = 254814111, Score = 65 },
             new Customer { CustomerId = 53274324, Score = 64 },
             new Customer { CustomerId = 6144320, Score = 32 },
             new Customer { CustomerId = 7786448, Score = 31 },
             new Customer { CustomerId = 54814111, Score = 87 },
             new Customer { CustomerId = 96144320, Score = 99 },
        };
    }
}
