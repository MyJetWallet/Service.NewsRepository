﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;
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

            if (!string.IsNullOrWhiteSpace(request.Asset))
                news = news.Where(t=>!t.AssociatedAssets.Any() || t.AssociatedAssets.Contains(request.Asset));
            
            if (!string.IsNullOrWhiteSpace(request.Lang))
               news = news.Where(t => t.Lang == request.Lang);

            return new NewsListResponse()
            {
                NewsList = news.OrderByDescending(n=>n.Timestamp).ToList()
            };        
        }

        public async Task AddOrUpdateNews(News request) => await _newsWriter.InsertOrReplaceAsync(NewsNoSqlEntity.Create(request));
        
        public async Task DeleteNews(DeleteNewsRequest request)
        {
            await _newsWriter.DeleteAsync(NewsNoSqlEntity.GeneratePartitionKey(request.Topic),
                NewsNoSqlEntity.GenerateRowKey(request.Lang));
        }
    }
}
