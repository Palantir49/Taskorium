using System.Net;
using System.Text;
using FileStorageService.Infrastructure.MinIO;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.DataModel.Response;
using Minio.Exceptions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace FileStorageService.Tests;

public class MinioServiceTests
{
    private const string BucketName = "test-bucket";
    private const string ObjectName = "test-object.txt";
    private readonly IMinioClient _minioClient;
    private readonly MinioService _sut;

    public MinioServiceTests()
    {
        _minioClient = Substitute.For<IMinioClient>();
        var logger = Substitute.For<ILogger<MinioService>>();
        _sut = new MinioService(_minioClient, logger);
    }

    #region UploadFileAsync (ReadOnlyMemory overload)

    [Fact]
    public async Task UploadFileAsync_Memory_EnsuresBucketAndUploads()
    {
        _minioClient.BucketExistsAsync(Arg.Any<BucketExistsArgs>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var data = new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes("memory content"));

        await _sut.UploadFileAsync(data, ObjectName, BucketName);

        await _minioClient.Received(1)
            .MakeBucketAsync(Arg.Any<MakeBucketArgs>(), Arg.Any<CancellationToken>());
        await _minioClient.Received(1)
            .PutObjectAsync(Arg.Any<PutObjectArgs>(), Arg.Any<CancellationToken>());
    }

    #endregion

    #region DownloadFileAsync

    [Fact]
    public async Task DownloadFileAsync_WhenClientThrows_LogsAndRethrows()
    {
        _minioClient.GetObjectAsync(Arg.Any<GetObjectArgs>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new Exception("not found"));

        var act = async () => await _sut.DownloadFileAsync(BucketName, ObjectName);

        await act.Should().ThrowAsync<Exception>().WithMessage("not found");
    }

    #endregion

    #region EnsureBucketExistsAsync

    [Fact]
    public async Task EnsureBucketExistsAsync_WhenBucketDoesNotExist_CreatesBucket()
    {
        _minioClient.BucketExistsAsync(Arg.Any<BucketExistsArgs>(), Arg.Any<CancellationToken>())
            .Returns(false);

        await _sut.EnsureBucketExistsAsync(BucketName);

        await _minioClient.Received(1)
            .MakeBucketAsync(Arg.Any<MakeBucketArgs>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task EnsureBucketExistsAsync_WhenBucketExists_DoesNotCreateBucket()
    {
        _minioClient.BucketExistsAsync(Arg.Any<BucketExistsArgs>(), Arg.Any<CancellationToken>())
            .Returns(true);

        await _sut.EnsureBucketExistsAsync(BucketName);

        await _minioClient.DidNotReceive()
            .MakeBucketAsync(Arg.Any<MakeBucketArgs>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task EnsureBucketExistsAsync_WhenClientThrows_LogsAndRethrows()
    {
        var exception = new Exception("connection error");
        _minioClient.BucketExistsAsync(Arg.Any<BucketExistsArgs>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(exception);

        var act = async () => await _sut.EnsureBucketExistsAsync(BucketName);

        await act.Should().ThrowAsync<Exception>().WithMessage("connection error");
    }

    #endregion

    #region UploadFileAsync (Stream overload)

    [Fact]
    public async Task UploadFileAsync_Stream_UploadsAndReturnsObjectName_AndDisposesStream()
    {
        _minioClient.BucketExistsAsync(Arg.Any<BucketExistsArgs>(), Arg.Any<CancellationToken>())
            .Returns(true);

        _minioClient.PutObjectAsync(Arg.Any<PutObjectArgs>(), Arg.Any<CancellationToken>())
            .Returns(new PutObjectResponse(HttpStatusCode.OK, ObjectName,
                new Dictionary<string, string>(), 0, ObjectName));

        var stream = new MemoryStream(Encoding.UTF8.GetBytes("file content"));

        var result = await _sut.UploadFileAsync(stream, BucketName, ObjectName, "text/plain");

        result.Should().Be(ObjectName);
        await _minioClient.Received(1)
            .PutObjectAsync(Arg.Any<PutObjectArgs>(), Arg.Any<CancellationToken>());

        // Stream disposed in finally — further use should throw
        var act = stream.ReadByte;
        act.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public async Task UploadFileAsync_Stream_EnsuresBucketExistsBeforeUpload()
    {
        _minioClient.BucketExistsAsync(Arg.Any<BucketExistsArgs>(), Arg.Any<CancellationToken>())
            .Returns(false);

        _minioClient.PutObjectAsync(Arg.Any<PutObjectArgs>(), Arg.Any<CancellationToken>())
            .Returns(new PutObjectResponse(HttpStatusCode.OK, ObjectName,
                new Dictionary<string, string>(), 0, ObjectName));

        var stream = new MemoryStream("data"u8.ToArray());

        await _sut.UploadFileAsync(stream, BucketName, ObjectName, "text/plain");

        await _minioClient.Received(1)
            .MakeBucketAsync(Arg.Any<MakeBucketArgs>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UploadFileAsync_Stream_WhenPutObjectThrows_DisposesStreamAndRethrows()
    {
        _minioClient.BucketExistsAsync(Arg.Any<BucketExistsArgs>(), Arg.Any<CancellationToken>())
            .Returns(true);

        _minioClient.PutObjectAsync(Arg.Any<PutObjectArgs>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new Exception("upload failed"));

        var stream = new MemoryStream(Encoding.UTF8.GetBytes("data"));

        var act = async () => await _sut.UploadFileAsync(stream, BucketName, ObjectName, "text/plain");

        await act.Should().ThrowAsync<Exception>().WithMessage("upload failed");

        var disposedCheck = stream.ReadByte;
        disposedCheck.Should().Throw<ObjectDisposedException>();
    }

    #endregion

    #region DeleteFileAsync

    [Fact]
    public async Task DeleteFileAsync_WhenObjectExists_RemovesObject()
    {
        _minioClient.StatObjectAsync(Arg.Any<StatObjectArgs>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<ObjectStat>(null!));

        await _sut.DeleteFileAsync(BucketName, ObjectName);

        await _minioClient.Received(1)
            .RemoveObjectAsync(Arg.Any<RemoveObjectArgs>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteFileAsync_WhenObjectNotFound_ThrowsFileNotFoundException()
    {
        _minioClient.StatObjectAsync(Arg.Any<StatObjectArgs>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ObjectNotFoundException("object not found"));

        var act = async () => await _sut.DeleteFileAsync(BucketName, ObjectName);

        await act.Should().ThrowAsync<FileNotFoundException>()
            .WithMessage($"*{ObjectName}*{BucketName}*");

        await _minioClient.DidNotReceive()
            .RemoveObjectAsync(Arg.Any<RemoveObjectArgs>(), Arg.Any<CancellationToken>());
    }

    #endregion

    #region GetPresignedUrlAsync

    [Fact]
    public async Task GetPresignedUrlAsync_ReturnsUrlFromClient()
    {
        const string expectedUrl = "https://minio.example.com/test-bucket/test-object.txt?presigned=true";

        _minioClient.PresignedGetObjectAsync(Arg.Any<PresignedGetObjectArgs>())
            .Returns(expectedUrl);

        var result = await _sut.GetPresignedUrlAsync(BucketName, ObjectName, 3600);

        result.Should().Be(expectedUrl);
    }

    [Fact]
    public async Task GetPresignedUrlAsync_WhenClientThrows_LogsAndRethrows()
    {
        _minioClient.PresignedGetObjectAsync(Arg.Any<PresignedGetObjectArgs>())
            .ThrowsAsync(new Exception("presign error"));

        var act = async () => await _sut.GetPresignedUrlAsync(BucketName, ObjectName, 3600);

        await act.Should().ThrowAsync<Exception>().WithMessage("presign error");
    }

    #endregion
}
