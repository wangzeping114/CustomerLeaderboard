namespace CustomerLeaderboard.Api.Controllers
{
    using CustomerLeaderboard.Api.Services;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }


        [HttpPost("/customer/{customerId}/score/{score}")]
        [SwaggerOperation(Summary = "更新客户分数", Description = "更新特定客户的分数。")]
        [SwaggerResponse(200, "返回更新后的分数", typeof(decimal))]
        [SwaggerResponse(400, "如果分数或客户 ID 无效")]
        public async Task<IActionResult> UpdateScore(long customerId, decimal score)
        {
            // 验证分数范围
            if (score < -1000 || score > 1000)
            {
                return BadRequest("Score must be between -1000 and +1000.");
            }
            await _leaderboardService.UpdateScoreAsync(customerId, score);
            // 返回成功响应
            return Ok();
        }


        [HttpGet("leaderboard")]
        [SwaggerOperation(Summary = "按排名获取客户", Description = "根据指定范围的排名检索客户列表。")]
        [SwaggerResponse(200, "返回客户及其排名和分数的列表。")]
        [SwaggerResponse(400, "如果起始或结束排名无效。")]
        public async Task<IActionResult> GetCustomersByRank([FromQuery] int start, [FromQuery] int end)
        {
            var customers = await _leaderboardService.GetCustomersByRankAsync(start, end);
            return Ok(customers);
        }


        [HttpGet("leaderboard/{customerId}")]
        [SwaggerOperation(Summary = "获取客户及其邻近客户", Description = "根据提供的高排名和低排名邻居数量，检索指定客户及其邻近客户。")]
        [SwaggerResponse(200, "返回客户及其邻近客户的排名和分数。")]
        [SwaggerResponse(400, "如果客户 ID 无效或高/低参数超出范围。")]
        public async Task<IActionResult> GetCustomerAndNeighbors(long customerId, [FromQuery] int high, [FromQuery] int low)
        {
            var customer = await _leaderboardService.GetCustomerAndNeighborsAsync(customerId, high, low);
            return Ok(customer);
        }
    }
}
