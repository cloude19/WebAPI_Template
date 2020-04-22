using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Pratice.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace API_Pratice
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
            services.AddControllers();

            /*Data Source: name of the Server(can be obtained by going to the SQL Server viewer)
             Initial Catalog: name of Database: we made that name on the spot.*/

            //DbContextOptionsBuilder options = new DbContextOptionsBuilder(); 
            services.AddDbContext<QuotesDbContext>(options => options.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QuotesDb"));

            /*service generated from the Microsoft.AspNetCore.Mvc.Formatters.Xml nuget class
             This will convert the json return format to xml if requested by the client*/
            services.AddMvc().AddXmlDataContractSerializerFormatters();
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

            /*this will apply migration to the DB if there is one
             Note: this will not create migrations*/
            //quotesDbContext.Database.Migrate();

            //quotesDbContext.Database.EnsureCreated();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
