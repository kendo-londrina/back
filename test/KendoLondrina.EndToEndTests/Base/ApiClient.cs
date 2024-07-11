using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KenLo.EndToEndTests.Base;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Post<TOutput>(
        string route,
        object payload
    ) {
        var response = await _httpClient.PostAsync(
            route,
            new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            )
        );
        var responseContent = await response.Content.ReadAsStringAsync();
        var output = JsonSerializer.Deserialize<TOutput>(
            responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        return (response, output);
    }
    
}