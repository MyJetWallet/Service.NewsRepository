using System;
using System.Linq;
using System.Threading.Tasks;
using MyNoSqlServer.DataReader;
using MyNoSqlServer.DataWriter;
using ProtoBuf.Grpc.Client;
using Service.NewsRepository.Client;
using Service.NewsRepository.Domain.Models.NoSql;
using Service.NewsRepository.Grpc.Models;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var writer =
                new MyNoSqlServerDataWriter<NewsNoSqlEntity>(() => "http://192.168.70.80:5123",
                    NewsNoSqlEntity.TableName, true);
            
            



            var data = writer.GetAsync(NewsNoSqlEntity.GeneratePartitionKey("BTC", "en")).GetAwaiter().GetResult();
            
            Console.WriteLine($"Data count: {data.Count()}");

            var index = 0;
            foreach (var entity in data.OrderBy(e => e.RowKey))
            {
                Console.WriteLine($"  {++index}: {entity.News.Topic}");
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("End");
        }
    }
}
