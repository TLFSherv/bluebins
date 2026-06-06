using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            policy.WithOrigins("http://localhost:5173");
        });
});


builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);
app.MapIdentityApi<ApplicationUser>();

app.MapGet("/", () => "Hello World!");

app.Run();
