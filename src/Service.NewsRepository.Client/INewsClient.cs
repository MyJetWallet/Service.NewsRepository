using System.Collections.Generic;
using System.Threading.Tasks;
using Service.NewsRepository.Domain.Models;
using Service.NewsRepository.Grpc.Models;

namespace Service.NewsRepository.Client;

public interface INewsClient
{
    List<News> GetNews(GetNewsRequest request);
}