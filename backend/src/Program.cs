using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>( // register DbContext
    options => options.UseNpgsql(connString) // specify provider
);
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();
app.MapIdentityApi<ApplicationUser>();

app.MapGet("/", () => "Hello World!")
.RequireAuthorization();

app.Run();
