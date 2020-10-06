using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AuditSourcesSample
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
            services.AddScoped<IAuditSourcesProvider, AuditSourcesProvider>();
            //services.AddScoped<IAuditSourcesProvider, CustomeAuditSourcesProvider>();

            services.AddDbContext<ApplicationDbContext>((serviceProvider, optionBuilder) =>
            {
                 optionBuilder.UseInMemoryDatabase("testDb");
               // optionBuilder.UseSqlServer("Data Source=.;Initial Catalog=AuditSourcesSample;Integrated Security=true");

                var auditSourcesProvider = serviceProvider.GetRequiredService<IAuditSourcesProvider>();
                var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

                optionBuilder.AddInterceptors(new AuditSaveChangesInterceptor(httpContextAccessor),
                new AuditSourcesSaveChangesInterceptor(auditSourcesProvider));
            });



            services.AddHttpContextAccessor();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<ClientVersionHeaderFilter>();

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuditSourcesSample", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuditSourcesSample v1"));
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
