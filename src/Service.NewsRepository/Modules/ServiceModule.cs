using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using MyJetWallet.Sdk.NoSql;
using Service.NewsRepository.Domain.Models.NoSql;

namespace Service.NewsRepository.Modules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterMyNoSqlWriter<NewsNoSqlEntity>(Program.ReloadedSettings(e => e.MyNoSqlWriterUrl), NewsNoSqlEntity.TableName);
            
        }
    }
}