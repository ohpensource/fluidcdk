using System;
using Amazon.CDK;
using ImageTagger.Infra.Constructs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HelloCdk
{
    static class Program
    {
        static IConfiguration _config;

        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var app = new App();

            serviceProvider.GetService<IImageTaggerStackBuilder>().GetInstance(app);

            app.Synth();

        }

        static void ConfigureServices(ServiceCollection services)
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            
            services.AddSingleton<IConfiguration>(_config);
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            services.AddSingleton<IImageTaggerStackBuilder, ImageTaggerStackBuilder>();
            services.AddSingleton<IImageBucketBuilder, ImageBucketBuilder>();
            services.AddSingleton<ITaggerFunctionBuilder, TaggerFunctionBuilder>();
            services.AddSingleton<IWebAppFunctionBuilder, WebAppFunctionBuilder>();
            services.AddSingleton<IWebAppRestApiBuilder, WebAppRestApiBuilder>();

        }

    }
}
