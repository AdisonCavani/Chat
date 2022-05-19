using Chat.WebApi.Extensions;
using Chat.WebApi.Models.App;
using Chat.WebApi.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

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

        services.AddScoped<EmailService>();
        services.AddScoped<JwtService>();

        services.AddValidators();

        services.ConfigureIdentity();

        services.ConfigureAuthentication(Configuration);

        services.ConfigureDbContext(Configuration);

        services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
        });

        services.AddControllers().AddFluentValidation();

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
        }

        if (context.Database.IsRelational())
            context.Database.Migrate();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
