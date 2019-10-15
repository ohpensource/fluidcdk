using System;
using Amazon.CDK;
using ImageTagger.Infra;
using ImageTagger.Infra.Constructs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using InfraContext = ImageTagger.Infra.InfraContext;

namespace HelloCdk
{
    static class Program
    {

        static void Main()
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
            var _config = new ConfigurationBuilder()
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

            services.AddSingleton<IInfraContext>(BuildInfraContext(_config));
            services.AddSingleton<IImageTaggerStackBuilder, ImageTaggerStackBuilder>();
            services.AddSingleton<IImageBucketBuilder, ImageBucketBuilder>();
            services.AddSingleton<ITaggerFunctionBuilder, TaggerFunctionBuilder>();
            services.AddSingleton<IWebAppFunctionBuilder, WebAppFunctionBuilder>();
            services.AddSingleton<IWebAppRestApiBuilder, WebAppRestApiBuilder>();

        }

        static InfraContext BuildInfraContext(IConfiguration config)
        {
            var section = config.GetSection("Infrastructure");

            return new InfraContext()
            {
                StackName = section.GetValue<string>("ImageTaggerStackName"),
                Region = section.GetValue<string>("Region"),
                AssetFileFolder = config.GetValue<string>("ASSET_FOLDER"),
                WebApiRestApiName = section.GetValue<string>("WebApiRestApiName"),
                ImageTaggerFunctionName = section.GetValue<string>("ImageTaggerFunctionName"),
                Account = section.GetValue<string>("Account"),
                ImageBucketName = section.GetValue<string>("ImageBucketName"),
                RestApiFunctionName = section.GetValue<string>("RestApiFunctionName")
            };

        }

    }
}
