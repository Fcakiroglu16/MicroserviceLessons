using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

namespace Logging;

public static class Logging
{
    public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger =>
        (hostingContext, loggerConfiguration) =>
        {
            var env = hostingContext.HostingEnvironment;

            loggerConfiguration.MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithProperty("ApplicationName", env.ApplicationName)
                .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .WriteTo.Console();

            var elasticSearchUrl =
                hostingContext.Configuration.GetSection("ElkLogging")[
                    "ElasticSearchUrl"];
            var userName =
                hostingContext.Configuration.GetSection("ElkLogging")[
                    "Username"];
            var password =
                hostingContext.Configuration.GetSection("ElkLogging")[
                    "Password"];
            var IsDevPreprodProd =
                hostingContext.Configuration.GetSection("ElkLogging")[
                    "environment"];
            var indexName =
                hostingContext.Configuration.GetSection("ElkLogging")[
                    "IndexName"];
            if (!string.IsNullOrEmpty(elasticSearchUrl))
            {
                loggerConfiguration.WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(elasticSearchUrl))
                    {
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                        IndexFormat = $"microservices-{indexName}-{IsDevPreprodProd}-logs-" + "{0:yyyy.MM.dd}",
                        ModifyConnectionSettings = x => x.BasicAuthentication(userName, password),
                        CustomFormatter = new ElasticsearchJsonFormatter()
                    });
                ;
            }
            else
            {
                throw new Exception("Elastic Search Url can't read from appsettings.json");
            }
        };
}