using eduflowbackend.Core.Abstractions;
using eduflowbackend.Core.Resource;
using Mediator;

namespace eduflowbackend.Application.Resources.Upload;

public class UploadResourceHandler : IRequestHandler<UploadResourceCommand, Guid>
{
    private readonly IRepository<Resource> _repository;

    public UploadResourceHandler(IRepository<Resource> repository)
    {
        _repository = repository;
    }

    public async ValueTask<Guid> Handle(UploadResourceCommand request, CancellationToken cancellationToken)
    {
        if (request.File == null || request.File.Length == 0)
            throw new ArgumentException("File is empty");
        /*if(request.File.Length > 10*1024*1024)
            throw new ArgumentException("The max size should be 10MB");*/

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.File.FileName);
        var fileId = Guid.NewGuid(); //  Generate a unique ID for the file

        //  Setting the upload path
        var rootFolder = Directory.GetCurrentDirectory();
        var uploadsFolder = Path.Combine(rootFolder, "wwwroot", "uploads");

        //  Creating the directory in case it doesn't exist
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var fullPath = Path.Combine(uploadsFolder, fileName);
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream, cancellationToken);
        }

        return fileId;
    }
}