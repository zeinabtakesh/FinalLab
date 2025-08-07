namespace Application.Commands;
using MediatR;
public class DeleteDriverCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DeleteDriverCommand(Guid id) => Id = id;
    
}
