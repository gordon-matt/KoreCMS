using System.Configuration;
using Autofac;

namespace Kore.Data.ElasticSearch
{
    public class ElasticSearchModule : Module
    {
        private readonly string connectionString;

        public ElasticSearchModule()
        {
            connectionString = ConfigurationManager.ConnectionStrings["ElasticConnection"].ConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(ElasticProvider<>))
                .As(typeof(IElasticSearchProvider<>))
                .WithParameter(new NamedParameter("connectionString", connectionString))
                .InstancePerLifetimeScope();
        }
    }
}