using System.ServiceModel;
using System.Threading.Tasks;
using Service.NewsRepository.Domain.Models;
using Service.NewsRepository.Grpc.Models;

namespace Service.NewsRepository.Grpc
{
    [ServiceContract]
    public interface INewsService
    {

        [OperationContract]
        Task<NewsListResponse> GetNews(GetNewsRequest request);
        
        [OperationContract]
        Task AddOrUpdateNews(News request);
        
        [OperationContract]
        Task DeleteNews(DeleteNewsRequest request);
    }
}