using System;
using System.IO;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Infrastructure.Identity;
using CleanArchitecture.Razor.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Filters;
using System.Linq;
using Serilog.Events;
using Microsoft.AspNetCore.Builder;
using SmartAdmin.WebUI.Models;
using System.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using CleanArchitecture.Razor.Infrastructure;
using CleanArchitecture.Razor.Application;
using SmartAdmin.WebUI.Filters;
using FluentValidation.AspNetCore;
using CleanArchitecture.Razor.Application.Hubs;
using CleanArchitecture.Razor.Application.Hubs.Constants;
using CleanArchitecture.Razor.Infrastructure.Localization;
using Microsoft.Extensions.FileProviders;

string[] filters = new string[] { "Microsoft.EntityFrameworkCore.Model.Validation", "WorkflowCore.Services.WorkflowHost", "WorkflowCore.Services.BackgroundTasks.RunnablePoller", "Microsoft.Hosting.Lifetime", "Serilog.AspNetCore.RequestLoggingMiddleware" };

Log.Logger = new LoggerConfiguration()
          .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
          .Enrich.FromLogContext()
          .Enrich.WithClientIp()
          .Enrich.WithClientAgent()
          .Filter.ByExcluding(
                        Matching.WithProperty<string>("SourceContext", p => filters.Contains(p))
            )
          .WriteTo.Console()
          .CreateLogger();
var builder = WebApplication.CreateBuilder(args);
 
 

builder.WebHost.UseSerilog();
builder.Services.Configure<SmartSettings>(builder.Configuration.GetSection(SmartSettings.SectionName));
builder.Services.AddSingleton(s => s.GetRequiredService<IOptions<SmartSettings>>().Value);

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddInfrastructure(builder.Configuration)
        .AddApplication()
        .AddWorkflow(builder.Configuration);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllers();

builder.Services
     .AddRazorPages(options =>
     {
         options.Conventions.AddPageRoute("/AspNetCore/Welcome", "");
     })
     .AddMvcOptions(options =>
     {
         options.Filters.Add<ApiExceptionFilterAttribute>();
     })
    .AddFluentValidation(fv =>
    {
        fv.DisableDataAnnotationsValidation = true;
        fv.ImplicitlyValidateChildProperties = true;
        fv.ImplicitlyValidateRootCollectionElements = true;
    })
    .AddViewLocalization()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;

    })
    .AddRazorRuntimeCompilation();


builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});


builder.Services.AddSignalR();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        if (context.Database.IsSqlServer())
        {
            context.Database.Migrate();
        }

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

        await ApplicationDbContextSeed.SeedDefaultUserAsync(userManager, roleManager);
        await ApplicationDbContextSeed.SeedSampleDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        logger.LogError(ex, "An error occurred while migrating or seeding the database.");

        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Files")),
    RequestPath = new PathString("/Files")
});

app.UseRequestLocalization();
app.UseRequestLocalizationCookies();
app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) => {
        diagnosticContext.Set("UserName", httpContext.User?.Identity?.Name ?? string.Empty);
    };
});
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseWorkflow();
app.MapControllers();
app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();
    endpoints.MapHub<SignalRHub>(SignalR.HubUrl);
});

app.Run();
