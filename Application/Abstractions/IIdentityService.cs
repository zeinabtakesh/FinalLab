namespace Application.Abstractions;

public interface IIdentityService
{
    Task<string> CreateUserAsync(string username, string tempPw, string role);
    Task DisableUserAsync(string sub);
}