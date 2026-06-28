using Microsoft.AspNetCore.Mvc.Testing;

public class IntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _fixture;
    public IntegrationTest(WebApplicationFactory<Program> fixture)
    {
        _fixture = fixture;
    }
}