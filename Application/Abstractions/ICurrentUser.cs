namespace Application.Abstractions;


public interface ICurrentUser
{ string? UserName { get; }

    string? Email { get; }

    int? DriverId { get; }
    
    bool IsInRole(string role);
}
