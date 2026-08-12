using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    protected readonly WebApplicationFactory<Program> _fixture;
    protected ApplicationDbContext context;
    public IntegrationTestBase(WebApplicationFactory<Program> fixture)
    {
        _fixture = fixture;
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseSqlite(connection)
        .Options;

        context = new ApplicationDbContext(options);
    }

    public void Dispose()
    {
        context.Dispose();
    }
}