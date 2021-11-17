using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Chat.Web.Server;

/// <summary>
/// The startup class that handles constructing the ASP.Net server services
/// </summary>
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        // Share the configuration
        IoCContainer.Configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Add ApplicationDbContext to DI
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(IoCContainer.Configuration.GetConnectionString("DefaultConnection")));

        // AddIdentity adds cookie based authentication
        // Adds scoped classes for things like UserManager, SignInManager, PasswordHashes ect...
        // NOTE: Automatically adds the validated user from a cookie to the HttpContext.User
        services.AddIdentity<ApplicationUser, IdentityRole>()

            // Adds UserStore and RoleStore from this context
            // That are consumed by the UserManager and RoleManager
            .AddEntityFrameworkStores<ApplicationDbContext>()

            // Adds a provider that generates unique keys and hashes for things like
            // forgot password links, phone number verification codes ect...
            .AddDefaultTokenProviders();

        // Add JWT Authentication for API clients
        services.AddAuthentication().AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // Validate issuer
                ValidateIssuer = true,
                // Validate audience
                ValidateAudience = true,
                // Validate expiration
                ValidateLifetime = true,
                // Validate signature
                ValidateIssuerSigningKey = true,

                // Set issuer
                ValidIssuer = IoCContainer.Configuration["Jwt:Issuer"],
                // Set audience
                ValidAudience = IoCContainer.Configuration["Jwt:Audience"],

                // Set signing key
                IssuerSigningKey = new SymmetricSecurityKey(
                        // Get our secret key from configuration
                        Encoding.UTF8.GetBytes(IoCContainer.Configuration["Jwt:SecretKey"]))
            };
        });

        // Change password policy
        // TODO: revoke changes!!!
        services.Configure<IdentityOptions>(options =>
        {
            // Make really weak passwords possible
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 5;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        });

        // Alter application cookie info
        services.ConfigureApplicationCookie(options =>
        {
            // Redirect to /login
            options.LoginPath = "/login";

            // Change cookie timeout to expire in 15 seconds
            options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
        });

        services.AddMvc(options =>
        {
            options.EnableEndpointRouting = false;
            options.InputFormatters.Add(new XmlSerializerInputFormatter(null)); // TODO: check what settings should we pass as an argument
            options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
    {
        // Store instance of the DI service provider so our application can access it anywhere
        IoCContainer.Provider = serviceProvider as ServiceProvider;

        // Setup Identity
        app.UseAuthentication();

        if (env.IsDevelopment())
            // Show any exceptions in browser when they crash
            app.UseDeveloperExceptionPage();
        else
            // Show generic error page
            app.UseExceptionHandler("/Home/Error");

        // Serve static files
        app.UseStaticFiles();

        // Setup MVC routes
        app.UseMvc(routes =>
        {
            // Default route of /controller/action/info
            routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{moreInfo?}");

            // Set explicit about me page route
            routes.MapRoute(
            name: "aboutPage",
            template: "more",
            defaults: new { controller = "About", action = "TellMeMore" });
        });
    }
}
