using System.Net.Http;
using System.Net.Http.Headers;
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

    public async Task<TResponse?> GetAsync<TResponse>(string endpoint)
    {
        return await CallApiAsync<object?, TResponse>(endpoint, HttpMethod.Get, null);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest dto)
    {
        return await CallApiAsync<TRequest, TResponse>(endpoint, HttpMethod.Post, dto);
    }

    public async Task<TResponse?> GetAsyncById<TRequest, TResponse>(string endpoint, TRequest id)
    {
        return await CallApiAsync<object?, TResponse>(endpoint, HttpMethod.Get, id);
    }

    public async Task<TResponse?> PutAsyncById<TRequest, TResponse>(string endpoint, TRequest dto)
    {
        return await CallApiAsync<TRequest, TResponse>(endpoint, HttpMethod.Put, dto);
    }

    public async Task<HttpResponseMessage?> DeleteAsync<TRequest, HttpResponseMessage>(string endpoint, TRequest id)
    {
        return await CallApiAsync<object?, HttpResponseMessage>(endpoint, HttpMethod.Delete, id);
    }

    public async Task<HttpResponseMessage> PostMultipartFormDataAsync(
        string uri,
        MultipartFormDataContent content
    )
    {
        var response = await httpClient.PostAsync(uri, content);
        return response;
    }

    private async Task<TResponse?> CallApiAsync<TRequest, TResponse>(
        string endpoint,
        HttpMethod httpMethod,
        TRequest? dto
    )
    {
        var request = new HttpRequestMessage(httpMethod, $"proxy-endpoint/{endpoint}");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (httpMethod != HttpMethod.Get && dto is not null)
        {
            var serialized = JsonSerializer.Serialize(dto);
            request.Content = new StringContent(serialized);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        var response = await httpClient.SendAsync(request);

        if (
            response.StatusCode == System.Net.HttpStatusCode.Forbidden
            || response.StatusCode == System.Net.HttpStatusCode.Unauthorized
        )
        {
            navigationManager.NavigateTo("AccessDenied");
        }

        response.EnsureSuccessStatusCode();

        var res = await JsonSerializer.DeserializeAsync<TResponse>(
            await response.Content.ReadAsStreamAsync(),
            _jsonSerializerOptions,
            CancellationToken.None
        );
        return res;
    }
}
