using System;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.Models.Request;
using FailurePredictionSystemBack.Common.Models.Response;
using FailurePredictionSystemBack.Core.Models;
using FailurePredictionSystemBack.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FailurePredictionSystemBack.ServiceLayer.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly FailureSystemDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
        FailureSystemDbContext dbContext,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var exists = await _dbContext.Users
            .AnyAsync(x => x.Email == email, cancellationToken);

        if (exists)
            throw new InvalidOperationException("Пользователь с таким email уже существует.");

        var passwordHash = _passwordHasher.Hash(request.Password);

        var user = new User(
            email: email,
            passwordHash: passwordHash,
            fullName: request.FullName,
            role: request.Role);

        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var token = _jwtTokenGenerator.Generate(user);

        return AuthResponse.Create(user, token);
    }

    public async Task<AuthResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        if (user is null)
            throw new InvalidOperationException("Неверный email или пароль.");

        if (!user.IsActive)
            throw new InvalidOperationException("Пользователь отключён.");

        var isPasswordValid = _passwordHasher.Verify(
            request.Password,
            user.PasswordHash);

        if (!isPasswordValid)
            throw new InvalidOperationException("Неверный email или пароль.");

        var token = _jwtTokenGenerator.Generate(user);

        return AuthResponse.Create(user, token);
    }
}