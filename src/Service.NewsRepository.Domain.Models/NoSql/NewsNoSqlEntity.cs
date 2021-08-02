using MyNoSqlServer.Abstractions;

namespace Service.NewsRepository.Domain.Models.NoSql
{
    public class NewsNoSqlEntity : MyNoSqlDbEntity
    {
        public const string TableName = "myjetwallet-news";
        public static string GeneratePartitionKey(string topic) => topic;
        public static string GenerateRowKey(string lang) => lang;

        public News News { get; set; }

        public static NewsNoSqlEntity Create(News news)
        {
            return new NewsNoSqlEntity()
            {
                PartitionKey = GeneratePartitionKey(news.Topic),
                RowKey = GenerateRowKey(news.Lang),
                News = news
            };
        }
    }
}