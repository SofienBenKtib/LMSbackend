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

        var fileId = Guid.NewGuid();
        var extension = Path.GetExtension(request.File.FileName);
        var fileName = fileId + extension;

        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        var filePath = Path.Combine(uploadPath, fileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream, cancellationToken);
        }

        Console.WriteLine($"File {fileName} uploaded to {uploadPath}");

        return fileId;
    }
}