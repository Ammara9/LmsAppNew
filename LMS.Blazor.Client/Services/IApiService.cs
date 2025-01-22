using LMS.Blazor.Client.Models;
using LMS.Shared.DTOs;

namespace LMS.Blazor.Client.Services;

public interface IApiService
{
    Task<TResponse?> GetAsync<TResponse>(string endpoint);
    Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest dto);

    Task<TResponse?> GetAsyncById<TRequest, TResponse>(string endpoint, TRequest id);
    Task<TResponse?> PutAsyncById<TRequest, TResponse>(string endpoint, TRequest dto);
    Task<TResponse?> DeleteAsync<TResponse>(string endpoint, int id);
    Task<HttpResponseMessage> PostMultipartFormDataAsync(
        string uri,
        MultipartFormDataContent content
    );
}
