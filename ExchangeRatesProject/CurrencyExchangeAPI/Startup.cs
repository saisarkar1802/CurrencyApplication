using CurrencyExchangeDAL.Context;
using CurrencyExchangeDAL.DataInterface;
using CurrencyExchangeDAL.DataService;
using CurrencyExchangeHelpers.HelperInterface;
using CurrencyExchangeHelpers.HelperService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddControllers();
            services.AddMvc(opt =>
            {
                opt.Filters.Add(new DecoderFallbackExceptionFilter());
            });
            services.AddDbContextPool<CurrencyExchangeDBContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ExchangeRatesDBConnection"));
            });
            services.AddTransient<ICalculationHelperService, CalculationHelperService>();
            services.AddTransient<IAPIInteractionService, APIInteractionService>();
            services.AddScoped<IExchangeRatesService, ExchangeRatesService>();
            services.AddTransient<APIHelperService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
