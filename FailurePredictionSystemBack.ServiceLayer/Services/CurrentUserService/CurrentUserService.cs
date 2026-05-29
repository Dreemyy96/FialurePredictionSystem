using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace FailurePredictionSystemBack.ServiceLayer.Services.CurrentUserService;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(value, out var userId)
                ? userId
                : null;
        }
    }

    public string Email =>
        _httpContextAccessor.HttpContext
            .User
            .FindFirst(ClaimTypes.Email)?.Value;

    public string Role =>
        _httpContextAccessor.HttpContext
            .User
            .FindFirst(ClaimTypes.Role)?.Value;
}