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
            IEnumerable<News> news = null;

            if (string.IsNullOrWhiteSpace(request.Lang))
                request.Lang = "en";

            request.Lang = request.Lang.ToLower();

            if (!string.IsNullOrWhiteSpace(request.Ticker))
            {
                var data = await _newsWriter.GetAsync(NewsNoSqlEntity.GeneratePartitionKey(request.Ticker, request.Lang));
                news = data.Select(e => e.News);
            }
            else
            {
                var data = await _newsWriter.GetAsync();
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
            
            var noSqlEntities = request.AssociatedAssets
                .Select(ticker => NewsNoSqlEntity.Create(request, ticker))
                .ToList();
            await _newsWriter.BulkInsertOrReplaceAsync(noSqlEntities);
            foreach (var noSqlEntity in noSqlEntities)
            {
                await _newsWriter.CleanAndKeepLastRecordsAsync(noSqlEntity.PartitionKey, Program.Settings.CleanAndKeepLastRecordsCount);
            }
        }

        public async Task AddOrUpdateNewsCollection(AddOrUpdateNewsCollectionRequest request)
        {
            _logger.LogInformation("AddOrUpdateNewsCollection received request: {newsRequest}", JsonConvert.SerializeObject(request));

            if (request?.NewsCollection == null)
            {
                return;
            }

            var noSqlEntities = new List<NewsNoSqlEntity>();
            foreach (var news in request.NewsCollection)
            {
                news.Lang = news.Lang.ToLower();
                news.AssociatedAssets = news.AssociatedAssets.Select(asset => asset.ToUpper()).ToList();

                noSqlEntities.AddRange(news.AssociatedAssets
                        .Select(ticker => NewsNoSqlEntity.Create(news, ticker)));
            }

            await _newsWriter.BulkInsertOrReplaceAsync(noSqlEntities);

            foreach (var noSqlEntity in noSqlEntities)
            {
                await _newsWriter.CleanAndKeepLastRecordsAsync(noSqlEntity.PartitionKey, Program.Settings.CleanAndKeepLastRecordsCount);
            }
        }

        public async Task DeleteNews(DeleteNewsRequest request)
        {
            _logger.LogInformation("DeleteNews receive message: {requestJson}", JsonConvert.SerializeObject(request));
            try
            {
                await _newsWriter.DeleteAsync(NewsNoSqlEntity.GeneratePartitionKey(request.Ticker, request.Lang),
                    NewsNoSqlEntity.GenerateRowKey(request.Date));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
