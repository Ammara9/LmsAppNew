using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using LMS.Blazor.Client.Models;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Services;

public class ClientApiService(
    IHttpClientFactory httpClientFactory,
    NavigationManager navigationManager
) : IApiService
{
    private readonly HttpClient httpClient = httpClientFactory.CreateClient("BffClient");

    private readonly JsonSerializerOptions _jsonSerializerOptions =
        new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    //ToDo: Make generic
    public async Task<TResponse?> CallApiAsync<TResponse>(string endpoint)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"proxy-endpoint/{endpoint}");
        var response = await httpClient.SendAsync(requestMessage);

        if (
            response.StatusCode == System.Net.HttpStatusCode.Forbidden
            || response.StatusCode == System.Net.HttpStatusCode.Unauthorized
        )
        {
            navigationManager.NavigateTo("AccessDenied");
        }

        response.EnsureSuccessStatusCode();

        var demoDtos = await JsonSerializer.DeserializeAsync<TResponse>(
            await response.Content.ReadAsStreamAsync(),
            _jsonSerializerOptions,
            CancellationToken.None
        );
        return demoDtos;
    }

    public async Task<TResponse?> PostApiAsync<TRequest, TResponse>(
        string endpoint,
        TRequest content
    )
    {
        // Create an HttpRequestMessage for a POST request
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"proxy-endpoint/{endpoint}")
        {
            Content = JsonContent.Create(content, options: _jsonSerializerOptions),
        };

        // Send the request using HttpClient
        var response = await httpClient.SendAsync(requestMessage);

        // Handle unauthorized or forbidden responses
        if (
            response.StatusCode == System.Net.HttpStatusCode.Forbidden
            || response.StatusCode == System.Net.HttpStatusCode.Unauthorized
        )
        {
            navigationManager.NavigateTo("AccessDenied");
            return default;
        }

        // Ensure the response was successful
        response.EnsureSuccessStatusCode();

        // Deserialize and return the response content
        var result = await JsonSerializer.DeserializeAsync<TResponse>(
            await response.Content.ReadAsStreamAsync(),
            _jsonSerializerOptions,
            CancellationToken.None
        );

        return result;
    }
}
