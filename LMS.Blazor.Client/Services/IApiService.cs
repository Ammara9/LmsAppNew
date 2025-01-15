using LMS.Blazor.Client.Models;
using LMS.Shared.DTOs;

namespace LMS.Blazor.Client.Services;

public interface IApiService
{
    Task<TResponse?> CallApiAsync<TResponse>(string endpoint);
    Task<TResponse?> PostApiAsync<TRequest, TResponse>(string endpoint, TRequest content);
}
