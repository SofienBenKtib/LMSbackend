using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.Resource;
using Mediator;

namespace eduflowbackend.Application.Resources.Update;

public class UpdateResourceHandler : IRequestHandler<UpdateResourceCommand, string>
{
    private readonly IRepository<Resource> _repository;

    public UpdateResourceHandler(IRepository<Resource> repository)
    {
        _repository = repository;
    }

    public async ValueTask<string> Handle(UpdateResourceCommand request, CancellationToken cancellationToken)
    {
        //  Retrieve the resource from the Db
        var resource = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (resource == null)
        {
            return $"Resource with Id {request.Id} does not exist";
        }

        resource.Title = request.Title;
        resource.Description = request.Description;

        await _repository.UpdateAsync(resource);
        await _repository.SaveChangesAsync(cancellationToken);

        return $"Resource with Id {request.Id} updated";
    }
}