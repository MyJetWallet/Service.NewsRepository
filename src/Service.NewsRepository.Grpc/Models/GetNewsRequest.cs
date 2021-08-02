using System.Runtime.Serialization;

namespace Service.NewsRepository.Grpc.Models
{
    [DataContract]
    public class GetNewsRequest
    {
        [DataMember(Order = 1)]
        public string Lang { get; set; }
        
        [DataMember(Order = 2)]
        public string Asset { get; set; }
    }
}