using FluentResults;
using Mediator;

namespace eduflowbackend.Application.Resources.Update;

public class UpdateResourceCommand:IRequest<Result>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}