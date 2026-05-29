namespace FailurePredictionSystemBack.ServiceLayer.Services.AuthService;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string passwordHash);
}