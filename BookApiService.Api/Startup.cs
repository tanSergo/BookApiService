using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.IO;
using AutoMapper;
using BookApiService.Data;
using BookApiService.Services;
using BookApiService.Core.Services;
using BookApiService.Core;
using BookApiService.Api.Routing;

namespace BookApiService.Api
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
            services.AddControllers(options =>
            {
                options.Conventions.Insert(0, new RoutingConvention("Base Route Prefix", "api/bookservice/"));
                options.ModelBinderProviders.Insert(0, new FilterBinderProvider());
            }
            );
            services.AddEntityFrameworkNpgsql().AddDbContext<BookDBContext>(opt => opt.UseNpgsql(Configuration.GetConnectionString("Postgresql")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IBookService, BookService>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Book Service", Version = "v1" });
                
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddAutoMapper(typeof(Startup));
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

            app.UseSwagger(options => options.RouteTemplate = $"/api/bookservice/swagger/{{documentName}}/swagger.json"); 
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "/api/bookservice/swagger";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Service V1");
            });
        }
    }
}
