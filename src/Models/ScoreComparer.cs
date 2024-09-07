namespace CustomerLeaderboard.Api.Models
{
    public class ScoreComparer : IComparer<Customer>
    {
        public int Compare(Customer x, Customer y)
        {
            if (x.Score != y.Score)
            {
                // 按分数降序排列
                return y.Score.CompareTo(x.Score);
            }
            // 如果分数相同，按ID升序排列
            return x.CustomerId.CompareTo(y.CustomerId);
        }
    }
}
