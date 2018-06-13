using System;
using api.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace api
{
    public class Startup
    {
        private readonly IHostingEnvironment hostingEnvironment;
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAny",
                        x => x
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    if (hostingEnvironment.IsDevelopment())
                    {
                        options.SerializerSettings.Formatting = Formatting.Indented;
                    }

                    options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;

                    // Camel or Pascal Json?
                    // new DefaultContractResolver();
                    // options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();

                });

            // create only singleton filedb to syncronize it to all request
            services.AddSingleton<IFileDb>((IServiceProvider serviceProvider) =>
            {
                var fileDb = new FileDb();
                // initialize the document
                fileDb.Initialize();

                return fileDb;
            });
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
                app.UseHsts();
            }
 
            app.UseCors("AllowAny");
            // app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
