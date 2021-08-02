using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;
using Service.NewsRepository.Grpc;

namespace Service.NewsRepository.Client
{
    [UsedImplicitly]
    public class NewsRepositoryClientFactory: MyGrpcClientFactory
    {
        public NewsRepositoryClientFactory(string grpcServiceUrl) : base(grpcServiceUrl)
        {
        }

        public INewsService GetNewsService() => CreateGrpcService<INewsService>();
    }
}
