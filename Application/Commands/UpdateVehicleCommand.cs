namespace Application.Commands;

using MediatR;
using System.Text.Json.Serialization;

public class UpdateVehicleCommand : IRequest<bool>
    {
        [JsonIgnore] 
        public Guid Id { get; set; }

        public string PlateNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    
}
