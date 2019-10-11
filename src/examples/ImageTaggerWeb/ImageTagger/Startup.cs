using System;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.S3;
using ImageTagger.Domain.Contracts;
using ImageTagger.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImageTagger.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddTransient<IImageFileService, S3ImageFileService>();
            services.AddTransient<IImageService, ImageService>();
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            var localOptions = Configuration.GetAWSOptions();
//#if DEBUG
//                // _startupLogs.Add($"No Aws Profile found. Getting Aws Credentials from Environment");
//                Console.WriteLine($"Getting Aws Credentials from Environment");
//                localOptions.Credentials = new BasicAWSCredentials(
//                    Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"),
//                    Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"));
//            localOptions.Region = RegionEndpoint.EUWest1;
//#endif
            services.AddDefaultAWSOptions(localOptions);
            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonDynamoDB>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }
    }
}
