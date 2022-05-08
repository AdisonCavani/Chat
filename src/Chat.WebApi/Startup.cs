using Chat.Core.ApiModels.Contacts;
using Chat.Core.ApiModels.LoginRegister;
using Chat.Core.ApiModels.UserProfile;
using Chat.Core.DI.Interfaces;
using Chat.WebApi.Email.SendGrid;
using Chat.WebApi.Email.Templates;
using Chat.WebApi.Models.App;
using Chat.WebApi.Models.Settings;
using Chat.WebApi.Models.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Chat.WebApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<DbSettings>(Configuration.GetSection(nameof(DbSettings)));
        services.Configure<AuthSettings>(Configuration.GetSection(nameof(AuthSettings)));

        services.AddScoped<UserManager<ApplicationUser>>();
        services.AddTransient<IEmailSender, SendGridEmailSender>();
        services.AddTransient<IEmailTemplateSender, EmailTemplateSender>();

        services.AddScoped<IValidator<LoginCredentialsDto>, LoginCredentialsDtoValidator>();
        services.AddScoped<IValidator<RegisterCredentialsDto>, RegisterCredentialsDtoValidator>();
        services.AddScoped<IValidator<VerifyUserEmailDto>, VerifyUserEmailDtoValidator>();
        services.AddScoped<IValidator<UpdateUserProfileDto>, UpdateUserProfileDtoValidator>();
        services.AddScoped<IValidator<UpdateUserPasswordDto>, UpdateUserPasswordDtoValidator>();
        services.AddScoped<IValidator<SearchUsersDto>, SearchUsersDtoValidator>();

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration["DbSettings:SqlConnectionString"], sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
            }));

        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = "Bearer";
            options.DefaultChallengeScheme = "Bearer";
            options.DefaultAuthenticateScheme = "Bearer";
        }).AddJwtBearer(jwtOptions =>
        {
            jwtOptions.SaveToken = true;
            jwtOptions.RequireHttpsMetadata = true;
            jwtOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero, // FIX: might cause issues, if auth is out of sync
                ValidIssuer = Configuration["AuthSettings:Issuer"],
                ValidAudience = Configuration["AuthSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthSettings:SecretKey"])),
            };
        });

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 5;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.User.RequireUniqueEmail = true;
        });

        services.AddControllers().AddFluentValidation();

        services.AddSwaggerGen(c =>
        {
            c.DescribeAllParametersInCamelCase();
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Chat.WebApi", Version = "v1" });
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
        {
            context.Database.Migrate();
        }

        app.UseHsts();
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