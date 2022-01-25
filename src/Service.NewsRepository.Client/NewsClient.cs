using System;
using System.Collections.Generic;
using System.Linq;
using MyNoSqlServer.Abstractions;
using Service.NewsRepository.Domain.Models;
using Service.NewsRepository.Domain.Models.NoSql;
using Service.NewsRepository.Grpc.Models;

namespace Service.NewsRepository.Client;

public class NewsClient: INewsClient
{
    private readonly IMyNoSqlServerDataReader<NewsNoSqlEntity> _reader;

    public NewsClient(IMyNoSqlServerDataReader<NewsNoSqlEntity> reader)
    {
        _reader = reader;
    }
    
    public List<News> GetNews(GetNewsRequest request)
    {
        IEnumerable<News> news = null;

        if (string.IsNullOrWhiteSpace(request.Lang))
            request.Lang = "en";

        request.Lang = request.Lang.ToLower();

        if (!string.IsNullOrWhiteSpace(request.Ticker))
        {
            var data = _reader.Get(NewsNoSqlEntity.GeneratePartitionKey(request.Ticker, request.Lang));
            news = data.Select(e => e.News);
        }
        else
        {
            var data = _reader.Get();
            news = data
                .Select(e => e.News)
                .GroupBy(e => e.UrlAddress)
                .Select(e => e.First());
        }

        if (request.LastDate != DateTime.MinValue)
        {
            news = news.Where(e => e.Timestamp < request.LastDate)
                .OrderByDescending(e => e.Timestamp)
                .Take(request.BatchSize);
        }
        else
        {
            news = news.OrderByDescending(e=> e.Timestamp)
                .Take(request.BatchSize);
        }

        return news.OrderByDescending(n=>n.Timestamp).ToList();   
    }
}