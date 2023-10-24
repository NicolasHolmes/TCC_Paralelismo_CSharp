using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Connections;
using Repositories.Connections.Interfaces;
using Repositories.API;
using System;
using System.IO;
using Repositories.DataBase;
using Repositories.API.Interfaces;
using Repositories.DataBase.Interfaces;

namespace ExtractBot.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider provider;

        public IServiceProvider Provider => provider;
        public IConfiguration Configuration => _configuration;

        public Startup()
        {
            Console.Title = "ExtractBot";
            BotConfig.Instance.StartClock();
            _configuration = new ConfigurationBuilder()
#if DEBUG
                .SetBasePath(Directory.GetCurrentDirectory() + "/../../..")
#else
                .SetBasePath(Directory.GetCurrentDirectory())
#endif
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            IServiceCollection services = new ServiceCollection();

            services.AddTransient<IDbConnectionExtractBot>(db =>
                new DbConnectionExtractBot(Configuration.GetConnectionString("ExtractBot")
            ));
            services.AddSingleton(_configuration);

            #region ExtractServices
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductDetailService, ProductDetailService>();
            #endregion

            #region Services
            #endregion

            #region ApiRepositories
            services.AddScoped<IProductAPIRepository, ProductAPIRepository>();
            #endregion

            #region SQLRepositories
            services.AddScoped<IProductDBRepository, ProductDBRepository>();
            services.AddScoped<IProductDetailDBRepository, ProductDetailDBRepository>();
            #endregion

            provider = services.BuildServiceProvider();
        }
    }
}
