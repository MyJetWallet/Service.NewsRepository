using Autofac;
using MyJetWallet.Sdk.NoSql;
using MyNoSqlServer.DataReader;
using Service.NewsRepository.Domain.Models.NoSql;
using Service.NewsRepository.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.NewsRepository.Client
{
    public static class AutofacHelper
    {
        public static void RegisterNewsRepositoryClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new NewsRepositoryClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetNewsService()).As<INewsService>().SingleInstance();
        }
        
        public static void RegisterNewsRepositoryNoSqlReader(this ContainerBuilder builder, MyNoSqlTcpClient myNoSqlClient)
        {
            builder.RegisterMyNoSqlReader<NewsNoSqlEntity>(myNoSqlClient, NewsNoSqlEntity.TableName);
        }

        public static void RegisterNewsClient(this ContainerBuilder builder, MyNoSqlTcpClient myNoSqlClient)
        {
            var reader = builder.RegisterMyNoSqlReader<NewsNoSqlEntity>(myNoSqlClient, NewsNoSqlEntity.TableName);
            var client = new NewsClient(reader);
            builder.RegisterInstance(client).As<INewsClient>().SingleInstance();
        }
    }
}
