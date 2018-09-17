namespace DIS
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Hangfire;
    using Hangfire.SQLite;
    using Microsoft.Extensions.Configuration;
    using Hangfire.Mongo;

    //using Swashbuckle.AspNetCore.Swagger;
    //using AutoMapper;


    public class Startup
    {
        public IConfiguration _configuration;


        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            //Mapper.Initialize(cfg => cfg.CreateMap<Models.Files, Models.AllFilesDAO>());
            //Mapper.AssertConfigurationIsValid();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<IConfiguration>(Configuration);
            //GlobalConfiguration.Configuration.UseMongoStorage(_configuration.GetConnectionString("MongoClient"),
                                                              //_configuration.GetConnectionString("DbName"));

            services.AddHangfire(x => x.UseMongoStorage(_configuration.GetConnectionString("MongoClient"),
                                                        _configuration.GetConnectionString("DbName")));
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            //});
            services.AddMvc();
        
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStatusCodePages();
            //GlobalConfiguration.Configuration.UseMongoStorage("mongodb://localhost", "ApplicationDatabase");

            app.UseHangfireServer(new BackgroundJobServerOptions { WorkerCount = 1 });
            var fileWatch = new DIS.FileWatch(_configuration);

            var jobId = BackgroundJob.Enqueue(
                () => fileWatch.Run());
            app.UseHangfireDashboard();
            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //});
            app.UseMvc();

        }
    }
}
