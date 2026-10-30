using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EventPlus.WebAPI.Utils;
using Microsoft.Extensions.Options;

namespace EventPlus.WebAPI.Services;

public sealed class CloudinaryImageStorageService : IImageStorageService
{
    private const string ImageFolder = "eventplus/eventos";
    private const long MaxImageBytes = 10 * 1024 * 1024;
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp", ".gif"
    };

    private readonly Cloudinary _cloudinary;
    private readonly string _cloudName;

    public CloudinaryImageStorageService(IOptions<CloudinarySettings> options)
    {
        var settings = options.Value;
        _cloudName = settings.CloudName;
        _cloudinary = new Cloudinary(new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret));
        _cloudinary.Api.Secure = true;
    }

    public async Task<string> UploadImageAsync(IFormFile arquivo)
    {
        ArgumentNullException.ThrowIfNull(arquivo);

        if (arquivo.Length == 0 || arquivo.Length > MaxImageBytes)
            throw new ArgumentException("A imagem deve ter entre 1 byte e 10 MB.", nameof(arquivo));

        var extension = Path.GetExtension(arquivo.FileName);
        if (!AllowedExtensions.Contains(extension))
            throw new ArgumentException("Formato de imagem inválido. Use JPG, PNG, WEBP ou GIF.", nameof(arquivo));

        await using var stream = arquivo.OpenReadStream();
        var upload = await _cloudinary.UploadAsync(new ImageUploadParams
        {
            File = new FileDescription($"{Guid.NewGuid():N}{extension.ToLowerInvariant()}", stream),
            Folder = ImageFolder
        });

        if (upload.Error is not null || upload.SecureUrl is null)
            throw new InvalidOperationException($"Falha ao enviar imagem ao Cloudinary: {upload.Error?.Message ?? "URL segura ausente"}.");

        return upload.SecureUrl.AbsoluteUri;
    }

    public async Task<bool> DeleteImageAsync(string imageUrl)
    {
        if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri) ||
            uri.Scheme != Uri.UriSchemeHttps ||
            !uri.Host.Equals("res.cloudinary.com", StringComparison.OrdinalIgnoreCase))
            return false;

        var prefix = $"/{_cloudName}/image/upload/";
        if (!uri.AbsolutePath.StartsWith(prefix, StringComparison.Ordinal))
            return false;

        var path = uri.AbsolutePath[prefix.Length..];
        var slash = path.IndexOf('/');
        if (slash < 0 || path[0] != 'v' || !long.TryParse(path.AsSpan(1, slash - 1), out _))
            return false;

        path = Uri.UnescapeDataString(path[(slash + 1)..]);
        var extension = Path.GetExtension(path);
        var publicId = path[..^extension.Length];
        if (!AllowedExtensions.Contains(extension) ||
            !publicId.StartsWith(ImageFolder + "/", StringComparison.Ordinal) ||
            publicId.Contains("..", StringComparison.Ordinal))
            return false;

        var result = await _cloudinary.DestroyAsync(new DeletionParams(publicId));
        return result.Result == "ok";
    }
}
