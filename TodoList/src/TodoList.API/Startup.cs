using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OData.Edm;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using TodoList.Domain.TarefaModel.Interfaces;
using TodoList.Domain.TarefaModel.Queries;
using TodoList.Infrastructure;
using TodoList.Infrastructure.Context;

namespace TodoList.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllers();
            services.AddControllers(mvcOptions => mvcOptions.EnableEndpointRouting = false);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "TodoList", 
                    Version = "v1",
                    Description = "Um exemplo de API de tarefas",
                    Contact = new OpenApiContact 
                    {
                        Name = "Jhone Eleotério",
                        Email = "jeleoterio@img.com.br"
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });

            // Register
            RegisterServices(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
                              IWebHostEnvironment env, 
                              ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddFile("Logs/tarefa-api-(Date).txt");

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoList API V1");
                //c.RoutePrefix = string.Empty;
            });

            app.UseCors(option => option.AllowAnyOrigin());

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.Select().Filter();
                routeBuilder.MapODataServiceRoute("api", "api", GetEdmModel());
                routeBuilder.EnableDependencyInjection();

            });

            UseHealthCheck(app);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
        }

        IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<TarefaViewModel>("Tarefas");

            return odataBuilder.GetEdmModel();
        }

        void RegisterServices(IServiceCollection services)
        {
            // Add Mediatr
            var assembly = AppDomain.CurrentDomain.Load("TodoList.Application");
            services.AddMediatR(assembly);

            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ODataContext")));

            // Verificando a disponibilidade dos bancos de dados da aplicação através de Health Checks
            services.AddHealthChecks().AddSqlServer(Configuration.GetConnectionString("ODataContext"), name: "baseSql");
            services.AddHealthChecksUI();

            // Add OData
            services.AddOData();

            // Repository
            services.AddScoped<ITarefaRepository, TarefaRepository>();

            services.AddMvcCore(op =>
            {
                foreach (var formatter in op.OutputFormatters.OfType<ODataOutputFormatter>().Where(it => !it.SupportedMediaTypes.Any()))
                {
                    formatter.SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/prs.mock-odata"));
                }

                foreach (var formatter in op.InputFormatters.OfType<ODataInputFormatter>().Where(it => !it.SupportedMediaTypes.Any()))
                {
                    formatter.SupportedMediaTypes.Add(new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/prs.mock-odata"));
                }
            });
        }

        void UseHealthCheck(IApplicationBuilder app)
        {   
                // Ativando o middlweare de Health Check
                //app.UseHealthChecks("/status");
                app.UseHealthChecks("/status",
                   new HealthCheckOptions()
                   {
                       ResponseWriter = async (context, report) =>
                       {
                           var result = JsonConvert.SerializeObject(
                               new
                               {
                                   statusApplication = report.Status.ToString(),
                                   healthChecks = report.Entries.Select(e => new
                                   {
                                       check = e.Key,
                                       ErrorMessage = e.Value.Exception?.Message,
                                       status = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                                   })
                               });
                           context.Response.ContentType = MediaTypeNames.Application.Json;
                           await context.Response.WriteAsync(result);
                       }
                   });

                // Gera o endpoint que retornará os dados utilizados no dashboard
                app.UseHealthChecks("/healthchecks-data-ui", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                // Ativa o dashboard para a visualização da situação de cada Health Check
                app.UseHealthChecksUI();
        }
    }
}
