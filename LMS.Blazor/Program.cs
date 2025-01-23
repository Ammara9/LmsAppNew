using Domain.Models.Entities;
using LMS.Blazor;
using LMS.Blazor.Client.Services;
using LMS.Blazor.Components;
using LMS.Blazor.Components.Account;
using LMS.Blazor.Data;
using LMS.Blazor.Services;
using LMS.Infrastructure.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<IdentityUserAccessor>();

builder.Services.AddScoped<IdentityRedirectManager>();

builder.Services.AddScoped<AuthenticationStateProvider,
    PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddScoped<IApiService, ClientApiService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<LmsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LmsContext") ?? throw new InvalidOperationException("Connection string 'CompaniesContext' not found.")));

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<IdentityRole>()
    //.AddEntityFrameworkStores<ApplicationDbContext>()
    .AddEntityFrameworkStores<LmsContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddHttpClient("LmsAPIClient", cfg =>
     {
         cfg.BaseAddress = new Uri(
            builder.Configuration["LmsAPIBaseAddress"] ??
                throw new Exception("LmsAPIBaseAddress is missing."));
         cfg.Timeout = TimeSpan.FromMinutes(10);
     });

builder.Services.Configure<PasswordHasherOptions>(options => options.IterationCount = 10000);

builder.Services.AddSingleton<ITokenStorage, TokenStorageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(LMS.Blazor.Client._Imports).Assembly);

app.MapControllers();

app.MapAdditionalIdentityEndpoints(); ;

app.Run();
