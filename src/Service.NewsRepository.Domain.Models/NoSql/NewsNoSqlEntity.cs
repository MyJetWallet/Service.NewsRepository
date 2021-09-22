using System;
using System.Globalization;
using MyNoSqlServer.Abstractions;

namespace Service.NewsRepository.Domain.Models.NoSql
{
    public class NewsNoSqlEntity : MyNoSqlDbEntity
    {
        public const string TableName = "myjetwallet-news";
        public static string GeneratePartitionKey(string ticker, string lang) => ticker + "-" + lang;
        public static string GenerateRowKey(DateTime date) => date.ToString(CultureInfo.InvariantCulture);

        public News News { get; set; }

        public static NewsNoSqlEntity Create(News news, string ticker)
        {
            return new NewsNoSqlEntity()
            {
                PartitionKey = GeneratePartitionKey(ticker, news.Lang),
                RowKey = GenerateRowKey(news.Timestamp),
                News = news
            };
        }
    }
}