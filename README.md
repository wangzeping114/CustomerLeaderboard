# Customer Leaderboard API

## 项目简介

Customer Leaderboard API 是一个用于管理客户分数排名的服务。每个客户都有一个唯一的客户ID和分数，API 提供了实时更新客户分数、获取客户排名和邻近客户的功能。客户的分数会影响其在排行榜中的排名，分数高的客户排在前面。当分数相同时，ID较小的客户会排在前面。

## 功能说明

1. **更新客户分数**
   - **请求**：`POST /customer/{customerid}/score/{score}`
   - 根据客户ID更新分数，分数变化后会实时更新排名。
   - 若客户不存在，系统将会自动创建客户。
   
2. **获取客户排名及邻近客户**
   - **请求**：`GET /leaderboard/{customerid}?high={high}&low={low}`
   - 根据客户ID获取该客户及其前后邻近客户的排名信息。
   
3. **获取排行榜中的客户**
   - **请求**：`GET /leaderboard`
   - 获取所有参与排名的客户。

4. **分数排名规则**
   - 客户分数越高，排名越靠前；
   - 如果分数相同，客户ID较小的排在前面；
   - 分数为 0 的客户不参与排名。

## 技术栈

- **ASP.NET Core**: 项目基于 ASP.NET Core 框架构建，支持高并发场景下的 Web API 服务。
- **Concurrent Collections**: 使用 `ConcurrentDictionary` 和 `SortedSet` 实现内存中的客户数据和分数管理，支持线程安全的并发操作。
- **Docker**: 提供容器化支持，使项目能够在任何平台上快速部署和运行。

## 本地开发环境

### 先决条件

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Docker](https://www.docker.com/get-started)

### 克隆仓库

```bash
git clone https://github.com/your-username/customer-leaderboard-api.git
cd customer-leaderboard-api
```

## 运行项目
使用以下命令来运行项目：
```
dotnet build
dotnet run
```
应用将运行在 http://localhost:5000

## Docker 部署
### 使用 Docker 构建和运行
1. 构建 Docker 镜像
在项目根目录下执行以下命令构建 Docker 镜像：
```
docker build -t customer-leaderboard-api .
```
2. 运行 Docker 容器
使用以下命令运行构建好的 Docker 镜像：
```
docker run -d -p 5000:80 --name leaderboard-api customer-leaderboard-api
```
3. 查看容器状态
使用以下命令查看运行中的容器状态：
```
docker ps
```
4.停止容器
```
docker stop leaderboard-api
```
5.移除容器
```
docker rm leaderboard-api
```
## 贡献指南
欢迎提交问题 (Issues) 和贡献代码 (Pull Requests)。如果你有任何改进建议，请随时联系我们。