using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace KenLo.EndToEndTests.Base;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _defaultSerializeOptions;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _defaultSerializeOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Post<TOutput>(
        string route,
        object payload
    ) {
        var payloadJson = JsonSerializer.Serialize(
            payload,
            _defaultSerializeOptions
        );
        var response = await _httpClient.PostAsync(
            route,
            new StringContent(
                payloadJson,
                Encoding.UTF8,
                "application/json"
            )
        );
        var responseContent = await response.Content.ReadAsStringAsync();
        TOutput? output = default;
        if (!string.IsNullOrEmpty(responseContent)) {
            output = JsonSerializer.Deserialize<TOutput>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
        }

        return (response, output);
    }

    private async Task<TOutput?> GetOutput<TOutput>(HttpResponseMessage response)
        where TOutput : class
    {
        var outputString = await response.Content.ReadAsStringAsync();
        TOutput? output = null;
        if (!string.IsNullOrWhiteSpace(outputString))
            output = JsonSerializer.Deserialize<TOutput>(
                outputString,
                _defaultSerializeOptions
            );
        return output;
    }

    private string PrepareGetRoute(
        string route, 
        object? queryStringParametersObject
    )
    {
        if(queryStringParametersObject is null)
            return route;
        var parametersJson = JsonSerializer.Serialize(
            queryStringParametersObject,
            _defaultSerializeOptions
        );
        var parametersDictionary = Newtonsoft.Json.JsonConvert
            .DeserializeObject<Dictionary<string, string>>(parametersJson);
        return QueryHelpers.AddQueryString(route, parametersDictionary!);
    }    

    public async Task<(HttpResponseMessage?, TOutput?)> Get<TOutput>(
        string route,
        object? queryStringParametersObject = null 
    ) where TOutput : class
    {
        var url = PrepareGetRoute(route, queryStringParametersObject);
        var response = await _httpClient.GetAsync(url);
        var output = await GetOutput<TOutput>(response);
        return (response, output);
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Delete<TOutput>(
        string route
    ) where TOutput : class
    {
        var response = await _httpClient.DeleteAsync(route);
        var output = await GetOutput<TOutput>(response);
        return (response, output);
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Put<TOutput>(
        string route,
        object payload
    ) {
        var payloadJson = JsonSerializer.Serialize(
            payload,
            _defaultSerializeOptions
        );
        var response = await _httpClient.PutAsync(
            route,
            new StringContent(
                payloadJson,
                Encoding.UTF8,
                "application/json"
            )
        );
        var responseContent = await response.Content.ReadAsStringAsync();
        TOutput? output = default;
        if (!string.IsNullOrEmpty(responseContent)) {
            output = JsonSerializer.Deserialize<TOutput>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
        }

        return (response, output);
    }

}
