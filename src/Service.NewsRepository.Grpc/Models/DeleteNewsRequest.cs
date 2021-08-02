using System.Runtime.Serialization;

namespace Service.NewsRepository.Grpc.Models
{
    [DataContract]
    public class DeleteNewsRequest
    {
        [DataMember(Order = 1)]
        public string Topic { get; set; }
        
        [DataMember(Order = 2)]
        public string Lang { get; set; }
    }
}