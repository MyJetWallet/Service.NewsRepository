using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;
using Newtonsoft.Json;
using Service.NewsRepository.Domain.Models;
using Service.NewsRepository.Domain.Models.NoSql;
using Service.NewsRepository.Grpc;
using Service.NewsRepository.Grpc.Models;
using Service.NewsRepository.Settings;

namespace Service.NewsRepository.Services
{
    public class NewsService: INewsService
    {
        private readonly ILogger<NewsService> _logger;
        private readonly IMyNoSqlServerDataWriter<NewsNoSqlEntity> _newsWriter;

        public NewsService(ILogger<NewsService> logger, IMyNoSqlServerDataWriter<NewsNoSqlEntity> newsWriter)
        {
            _logger = logger;
            _newsWriter = newsWriter;
        }


        public async Task<NewsListResponse> GetNews(GetNewsRequest request)
        {
            var data = await _newsWriter.GetAsync();
            
            var news = data.Select(n => n.News);

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

            if (!string.IsNullOrWhiteSpace(request.Asset))
                news = news.Where(t=>!t.AssociatedAssets.Any() || t.AssociatedAssets.Contains(request.Asset.ToUpper()));
            
            if (!string.IsNullOrWhiteSpace(request.Lang))
               news = news.Where(t => t.Lang == request.Lang.ToLower());

            return new NewsListResponse()
            {
                NewsList = news.OrderByDescending(n=>n.Timestamp).ToList()
            };        
        }

        public async Task AddOrUpdateNews(News request)
        {
            _logger.LogInformation("AddOrUpdateNews received request: {newsRequest}", JsonConvert.SerializeObject(request));
            request.Lang = request.Lang.ToLower();
            request.AssociatedAssets = request.AssociatedAssets.Select(asset => asset.ToUpper()).ToList();
            await _newsWriter.InsertOrReplaceAsync(NewsNoSqlEntity.Create(request));
        }

        public async Task DeleteNews(DeleteNewsRequest request)
        {
            await _newsWriter.DeleteAsync(NewsNoSqlEntity.GeneratePartitionKey(request.Topic),
                NewsNoSqlEntity.GenerateRowKey(request.Lang));
        }
    }
}
