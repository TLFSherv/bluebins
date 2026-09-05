using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;

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
        var userIdService = new Mock<IUserIdService>();
        userIdService.Setup(x => x.GetUserId()).Returns("123456");
        context = new ApplicationDbContext(options, userIdService.Object);
    }

    public void Dispose()
    {
        context.Dispose();
    }
}