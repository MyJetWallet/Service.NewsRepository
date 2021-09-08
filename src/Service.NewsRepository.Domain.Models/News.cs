using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Service.NewsRepository.Domain.Models
{
    [DataContract]
    public class News
    {
        [DataMember(Order = 1)] public string Source { get; set; }
        [DataMember(Order = 2)] public string Topic { get; set; }
        [DataMember(Order = 3)] public string Description { get; set; }
        [DataMember(Order = 4)] public string Lang { get; set; }
        [DataMember(Order = 5)] public DateTime Timestamp { get; set; }
        [DataMember(Order = 6)] public string UrlAddress { get; set; }
        [DataMember(Order = 7)] public List<string> AssociatedAssets { get; set; } = new();
        [DataMember(Order = 8)] public string ImageUrl { get; set; }
        [DataMember(Order = 9)] public string Sentiment { get; set; }
    }
}