using Chat.Core;
using Chat.WebApi.Extensions;
using Chat.WebApi.Models.App;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Chat.WebApi;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureSettings(Configuration);

        services.RegisterServices();

        services.AddValidators();

        services.ConfigureIdentity();

        services.ConfigureAuthentication(Configuration);

        services.ConfigureDbContext(Configuration);

        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(CustomValidationAttribute));
        }).AddFluentValidation();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddSwaggerGen(c =>
        {
            c.DescribeAllParametersInCamelCase();
            c.SwaggerDoc("v1", new() { Title = "Chat.WebApi", Version = "v1" });
            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new()
            {
                Description = "JWT Authorization header using the bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            c.OperationFilter<AuthOperationFilter>();
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext context)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chat.WebApi v1"));
            app.UseExceptionHandler(a => a.Run(async httpContext =>
            {
                var exceptionHandlerPathFeature = httpContext.Features.Get<IExceptionHandlerPathFeature>();
                var errorMsg = exceptionHandlerPathFeature?.Error.Message;

                if (!string.IsNullOrEmpty(errorMsg))
                    await httpContext.Response.WriteAsJsonAsync(new ApiResponse
                    {
                        Errors = new[] { errorMsg }
                    });
            }));
        }

        if (context.Database.IsRelational())
            context.Database.Migrate();

        app.UseHttpsRedirection();

        app.UseExceptionHandler(a => a.Run(async httpContext =>
        {
            await httpContext.Response.WriteAsJsonAsync(new ApiResponse
            {
                Errors = new[] { "Oops! Something went wrong" }
            });
        }));

        app.UseRouting();

        app.UseWebSockets(); // TODO: configure WS

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
