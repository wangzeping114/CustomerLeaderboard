using CustomerLeaderboard.Api.Repositories;
using CustomerLeaderboard.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerLeaderboard.Tests
{
    public class TestBase
    {
        protected readonly ILeaderboardService LeaderboardService;
        protected readonly ServiceProvider ServiceProvider;
        public TestBase()
        {
            // 创建 ServiceCollection
            var services = new ServiceCollection();

            // 注册依赖实例
            services.AddScoped<ILeaderboardService, LeaderboardService>();
            services.AddScoped<ICustomerRepository, InMemoryCustomerRepository>();

            // 构建服务提供者
            ServiceProvider = services.BuildServiceProvider();

            // 获取需要测试的服务实例
            LeaderboardService = ServiceProvider.GetRequiredService<ILeaderboardService>();
        }
    }
}
