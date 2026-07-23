using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Task_Managment.BLL.DTOS.Auth;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Task_Management.IntegrationTests;

public class IntegrationTestBase : IClassFixture<ApiFactory>
{
    protected readonly HttpClient Client;

    public IntegrationTestBase(ApiFactory factory)
    {
        Client = factory.CreateClient();
    }

    protected static readonly JsonSerializerOptions JsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };
    protected async Task RegisterAndLoginAsync()
    {
        var email = $"test{Guid.NewGuid():N}@test.com";

        var register = new RegisterDTO
        {
            UserName = $"user{Guid.NewGuid():N}",
            Email = email,
            Password = "Test@123"
        };

        var registerResponse = await Client.PostAsJsonAsync("/api/auth/register", register);
        if (!registerResponse.IsSuccessStatusCode)
        {
            var body = await registerResponse.Content.ReadAsStringAsync();
            throw new Exception($"REGISTER FAILED: {registerResponse.StatusCode} - {body}");
        }

        var login = new LoginDTO
        {
            Email = email,
            Password = "Test@123"
        };

        var loginResponse = await Client.PostAsJsonAsync("/api/auth/login", login);
        if (!loginResponse.IsSuccessStatusCode)
        {
            var body = await loginResponse.Content.ReadAsStringAsync();
            throw new Exception($"LOGIN FAILED: {loginResponse.StatusCode} - {body}");
        }

        var auth = await loginResponse.Content.ReadFromJsonAsync<AuthResponseDto>();

        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", auth!.Token);
    }
}