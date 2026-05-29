using FailurePredictionSystemBack.Core.Models;

namespace FailurePredictionSystemBack.ServiceLayer.Services.AuthService;

public interface IJwtTokenGenerator
{
    string Generate(User user);
}