using FailurePredictionSystemBack.Core.Enums;

namespace FailurePredictionSystemBack.Common.Models.Request;

public class RegisterRequest
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string FullName { get; init; }
    public UserRole Role { get; init; }
}