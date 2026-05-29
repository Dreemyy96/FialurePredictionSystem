using System;

namespace FailurePredictionSystemBack.ServiceLayer.Services.CurrentUserService;

public interface ICurrentUserService
{
    Guid? UserId { get; }

    string? Email { get; }

    string? Role { get; }
}