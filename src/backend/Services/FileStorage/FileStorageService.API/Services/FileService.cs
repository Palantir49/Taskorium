using FileStorageService.Application.Interfaces;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Taskorium.FileStorage.V1;

namespace FileStorageService.Api.Services;

public class FileService(IFileStorageService fileStorageService, ILogger<FileService> logger)
    : Taskorium.FileStorage.V1.FileStorageService.FileStorageServiceBase
{
    public override async Task<GetFileResponse> DownloadFile(GetFileRequest request, ServerCallContext context)
    {
        logger.LogInformation("Начало получения вложения {ObjectName} из хранилища.", request.Name);
        var file = await fileStorageService.DownloadFileAsync(request.Name);
        var byteString = await ByteString.FromStreamAsync(file.FileStream);
        return new GetFileResponse { Body = byteString };
    }

    public override async Task<Empty> UploadFile(UploadFileRequest request, ServerCallContext context)
    {
        logger.LogInformation("Начало загрузки вложения {ObjectName} в хранилище.", request.Name);
        var stream = new MemoryStream();
        request.Body.WriteTo(stream);
        stream.Position = 0;
        var name = request.Name;
        var contentType = request.ContentType;
        await fileStorageService.UploadFileAsync(stream, name, contentType);
        return new Empty();
    }

    public override async Task<Empty> DeleteFile(DeleteFileRequest request, ServerCallContext context)
    {
        logger.LogInformation("Начало удаления вложения {ObjectName} из хранилища.", request.Name);
        await fileStorageService.DeleteFileAsync(request.Name);
        return new Empty();
    }
}
