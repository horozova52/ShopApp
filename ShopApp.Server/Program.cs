using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using ShopApp.Core;
using ShopApp.Infrastructure;
using ShopApp.Server.Components;
using ShopApp.Server.Components.Account;
using ShopApp.UseCases.Services.Book;
using ShopApp.UseCases.Services.Category;
using ShopApp.UseCases.Services.Email;

var builder = WebApplication.CreateBuilder(args);

var keyRingPath = Path.Combine(builder.Environment.ContentRootPath, "KeyRing");

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keyRingPath))   
    .SetApplicationName("ShopApp");


// --- Identity & Authentication ---
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddSignInManager()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();




builder.Services.ConfigureApplicationCookie(o =>
{
    o.Cookie.SameSite = SameSiteMode.None;
    o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    o.Cookie.HttpOnly = true;
    // dacă vrei 401 pe API în loc de redirect la /Account/Login
    o.Events.OnRedirectToLogin = ctx =>
    {
        if (ctx.Request.Path.StartsWithSegments("/api"))
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        }
        ctx.Response.Redirect(ctx.RedirectUri);
        return Task.CompletedTask;
    };
});


builder.Services.AddAuthorization();

// --- Blazor & Razor Components ---
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();

// --- DbContext ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// --- API / Controllers ---
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

//// --- CORS ---
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowBlazorClient", policy =>
//    {
//        policy.WithOrigins("https://localhost:5001", "http://localhost:5000")
//              .AllowAnyHeader()
//              .AllowAnyMethod()
//              .AllowCredentials();
//    });
//});

// --- Services ---
builder.Services.AddScoped<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBookService, BookService>();

// --- MudBlazor ---
builder.Services.AddMudServices();

// --- HTTP Client (for Blazor WebAssembly) ---
builder.Services.AddHttpClient("ShopAppApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7027/api/");
})
.ConfigurePrimaryHttpMessageHandler(() =>
    new HttpClientHandler
    {
        UseCookies = true,
        AllowAutoRedirect = false
    });

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("ShopAppApi"));

// --- Compression for WASM ---
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
});

// --- Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- Middleware pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
        options.RoutePrefix = "swagger";
    });

    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
//app.UseCors("AllowBlazorClient");

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(ShopApp.Client._Imports).Assembly);

app.MapAdditionalIdentityEndpoints();
app.MapControllers();

app.Run();
