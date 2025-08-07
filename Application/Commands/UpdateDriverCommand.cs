namespace Application.Commands;
using MediatR;

public class UpdateDriverCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
    
}
