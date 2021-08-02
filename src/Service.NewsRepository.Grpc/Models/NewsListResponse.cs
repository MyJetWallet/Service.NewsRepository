using System.Collections.Generic;
using System.Runtime.Serialization;
using Service.NewsRepository.Domain.Models;

namespace Service.NewsRepository.Grpc.Models
{
    [DataContract]
    public class NewsListResponse
    {
        [DataMember(Order = 1)]
        public List<News> NewsList { get; set; }
    }
}