using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPWrecover.Services;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>( // register DbContext
    options => options.UseNpgsql(connString) // specify provider
);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// specify the allowed origins for CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
        {
            policy.WithOrigins("https://localhost:5173")
             .AllowAnyHeader()
             .AllowAnyMethod()
             .AllowCredentials();
        });
});

builder.Services.AddProblemDetails();
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<ApplicationUser>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Path = "/";

    // This tells the backend: "Only require HTTPS if the request came from HTTPS, 
    // but don't force the cookie to be strictly HTTPS-only in the browser."
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

    // Alternatively, to completely force it off for local dev testing:
    // options.Cookie.SecurePolicy = CookieSecurePolicy.None;

    // CRITICAL: When Secure is None/SameAsRequest, SameSite CANNOT be 'None'.
    // It must be Lax or Strict.
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddAuthentication()
.AddOpenIdConnect("Google", options =>
{
    options.Authority = "https://accounts.google.com";
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.CallbackPath = "/signin-google";
    options.ResponseType = "code";
    options.SaveTokens = true;
    options.SignInScheme = IdentityConstants.ExternalScheme;

    // --- CRUCIAL COOKIE OVERRIDES FOR DECOUPLED DEV ---
    options.CorrelationCookie.HttpOnly = true;
    options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
    options.CorrelationCookie.SameSite = SameSiteMode.None;

    options.NonceCookie.HttpOnly = true;
    options.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;
    options.NonceCookie.SameSite = SameSiteMode.None;
});

// add auto mapper
builder.Services.AddAutoMapper(cfg => { }, typeof(BookingProfile));
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddSingleton<IBookingHelpers, BookingHelpers>();
builder.Services.AddValidatorsFromAssemblyContaining<AddBookingValidator>(ServiceLifetime.Singleton);

var app = builder.Build();
// add custom middleware for catching errors 
app.UseMiddleware<ErrorHandlerMiddleware>();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseStatusCodePages();

app.MapGet("/", () => "Hello World!");

// add api endpoints
app.AddAccountRoutes();
app.AddBookingRoutes();

app.Run();

public partial class Program { }