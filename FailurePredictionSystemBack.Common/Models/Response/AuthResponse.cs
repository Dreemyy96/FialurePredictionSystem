using System;
using FailurePredictionSystemBack.Core.Enums;
using FailurePredictionSystemBack.Core.Models;

namespace FailurePredictionSystemBack.Common.Models.Response;

public class AuthResponse
{
    public Guid UserId { get; }
    public string Email { get; }
    public string FullName { get; }
    public UserRole Role { get; }
    public int RoleCode => (int)Role;
    public string RoleName => Role.ToString();
    public string Token { get; }

    private AuthResponse(User user, string token)
    {
        UserId = user.Id;
        Email = user.Email;
        FullName = user.FullName;
        Role = user.Role;
        Token = token;
    }

    public static AuthResponse Create(User user, string token)
    {
        return new AuthResponse(user, token);
    }
}