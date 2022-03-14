using AgentChatCoordinator.Services;
using AgentChatCoordinator.Services.Interfaces;
using AgentChatCoordinator.Workers;
using DataAcccessLayer.InmemoryDataStore;
using DataAcccessLayer.Repositories.Implementations;
using DataAcccessLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using QueueSubscribers.Implementations;
using QueueSubscribers.Interfaces;

namespace AgentChatCoordinator
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AgentChatCoordinator", Version = "v1" });
            });

            services.AddTransient<IAgentsManagerService, AgentsManagerService>();
            services.AddTransient<IQueueSubscriber, ChatSessionSubscriber>(); 
            services.AddTransient<IChatSessionRepository, ChatSessionRepository>();
            services.AddTransient<IAgentsRepository, AgentsRepository>();
            services.AddTransient<IShiftManagerService, ShiftManagerService>();
            services.AddSingleton<IDateStore, InMemoryDataStore>();
            services.AddHostedService<SessnionQueueMonitor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AgentChatCoordinator v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
