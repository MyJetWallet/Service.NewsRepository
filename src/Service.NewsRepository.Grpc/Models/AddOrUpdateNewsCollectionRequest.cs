using System.Collections.Generic;
using System.Runtime.Serialization;
using Service.NewsRepository.Domain.Models;

namespace Service.NewsRepository.Grpc.Models
{
    [DataContract]
    public class AddOrUpdateNewsCollectionRequest
    {
        [DataMember(Order = 1)] public List<News> NewsCollection { get; set; }
    }
}