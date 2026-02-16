using Application.Abstracts.Repositories;
using Application.Abstracts.Services;
using Application.DTOs.PropertyAd;
using AutoMapper;
using Domain.Entities;

namespace Persistence.Services;

public class PropertyAdService : IPropertyAdService
{
    private readonly IPropertyAdRepository _propertyAdRepository;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileStorageService;
    private readonly IPropertyMediaRepository _propertyMediaRepository;

    public PropertyAdService(IPropertyAdRepository propertyAdRepository, IMapper mapper, IFileStorageService fileStorageService, IPropertyMediaRepository propertyMediaRepository)
    {
        _propertyAdRepository = propertyAdRepository;
        _mapper = mapper;
        _fileStorageService = fileStorageService;
        _propertyMediaRepository = propertyMediaRepository;
    }

    public async Task CreatePropertyAdAsync(CreatePropertyAdRequest request, string userId, CancellationToken ct = default)
    {
        var propertyAd = _mapper.Map<PropertyAd>(request);
        propertyAd.UserId = userId;

        await _propertyAdRepository.AddAsync(propertyAd, ct);
        await _propertyAdRepository.SaveChangesAsync(ct);

        if (request.Images != null && request.Images.Any())
        {
            propertyAd.MediaItems = new List<PropertyMedia>();
            int order = 0;
            foreach (var image in request.Images)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                
                var objectKey = await _fileStorageService.SaveAsync(image.OpenReadStream(), fileName, image.ContentType, propertyAd.Id, ct);

                propertyAd.MediaItems.Add(new PropertyMedia
                {
                    ObjectKey = objectKey,
                    Order = order++,
                    PropertyAdId = propertyAd.Id
                });
            }
            
            _propertyAdRepository.Update(propertyAd);
            await _propertyAdRepository.SaveChangesAsync(ct);
        }
    }

    public async Task<List<GetAllPropertyAdResponse>> GetAllPropertyAdsAsync(CancellationToken ct = default)
    {
        var propertyAds = await _propertyAdRepository.GetAllAsync(ct);
        return _mapper.Map<List<GetAllPropertyAdResponse>>(propertyAds);
    }

    public async Task<GetByIdPropertyAdResponse> GetByIdPropertyAdAsync(int id, CancellationToken ct = default)
    {
        var propertyAd = await _propertyAdRepository.GetByIdAsync(id, ct);
        if (propertyAd == null) throw new Exception($"PropertyAd with id {id} not found");
        return _mapper.Map<GetByIdPropertyAdResponse>(propertyAd);
    }

    public async Task UpdatePropertyAdAsync(UpdatePropertyAdRequest request, string userId, CancellationToken ct = default)
    {
        var propertyAd = await _propertyAdRepository.GetByIdAsync(request.Id, ct);
        if (propertyAd == null) throw new Exception($"PropertyAd with id {request.Id} not found");
        
        // Ownership check
        if (propertyAd.UserId != userId)
            throw new UnauthorizedAccessException("You can only edit your own property ads");

        _mapper.Map(request, propertyAd);

        // 1. Delete requested media
        if (request.MediaIdsToDelete != null && request.MediaIdsToDelete.Any())
        {
            var allMedia = await _propertyMediaRepository.GetByPropertyAdIdAsync(propertyAd.Id, ct);
            var mediasToDelete = allMedia.Where(m => request.MediaIdsToDelete.Contains(m.Id)).ToList();

            foreach (var media in mediasToDelete)
            {
                await _fileStorageService.DeleteFileAsync(media.ObjectKey, ct);
                _propertyMediaRepository.Delete(media);
            }
            await _propertyMediaRepository.SaveChangesAsync(ct);
        }

        // 2. Add new media
        if (request.NewImages != null && request.NewImages.Any())
        {
             // Get current max order
            var currentMedia = await _propertyMediaRepository.GetByPropertyAdIdAsync(propertyAd.Id, ct);
            int nextOrder = currentMedia.Any() ? currentMedia.Max(m => m.Order) + 1 : 0;

            foreach (var image in request.NewImages)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                var objectKey = await _fileStorageService.SaveAsync(image.OpenReadStream(), fileName, image.ContentType, propertyAd.Id, ct);

                var newMedia = new PropertyMedia
                {
                    ObjectKey = objectKey,
                    Order = nextOrder++,
                    PropertyAdId = propertyAd.Id
                };
                await _propertyMediaRepository.AddAsync(newMedia, ct);
            }
             await _propertyMediaRepository.SaveChangesAsync(ct);
        }

        _propertyAdRepository.Update(propertyAd);
        await _propertyAdRepository.SaveChangesAsync(ct);
    }

    public async Task DeletePropertyAdAsync(int id, string userId, CancellationToken ct = default)
    {
        var propertyAd = await _propertyAdRepository.GetByIdAsync(id, ct);
        if (propertyAd == null) throw new Exception($"PropertyAd with id {id} not found");
        
        // Ownership check
        if (propertyAd.UserId != userId)
            throw new UnauthorizedAccessException("You can only delete your own property ads");

        // 1. Get all media and delete from MinIO
        var mediaItems = await _propertyMediaRepository.GetByPropertyAdIdAsync(id, ct);
        foreach (var media in mediaItems)
        {
            await _fileStorageService.DeleteFileAsync(media.ObjectKey, ct);
        }

        // 2. Delete from DB (Cascade delete will handle media records, but we explicitly cleared files)
        _propertyAdRepository.Delete(propertyAd);
        await _propertyAdRepository.SaveChangesAsync(ct);
    }

    public async Task<string> UploadMediaAsync(int propertyAdId, Microsoft.AspNetCore.Http.IFormFile file, CancellationToken ct = default)
    {
        var propertyAd = await _propertyAdRepository.GetByIdAsync(propertyAdId, ct);
        if (propertyAd == null) throw new Exception($"PropertyAd with id {propertyAdId} not found");

        var currentMedia = await _propertyMediaRepository.GetByPropertyAdIdAsync(propertyAdId, ct);
        if (currentMedia.Count >= 5)
        {
            throw new Exception("Maximum 5 media items allowed.");
        }

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var objectKey = await _fileStorageService.SaveAsync(file.OpenReadStream(), fileName, file.ContentType, propertyAdId, ct);

        int nextOrder = currentMedia.Any() ? currentMedia.Max(m => m.Order) + 1 : 0;

        var newMedia = new PropertyMedia
        {
            ObjectKey = objectKey,
            Order = nextOrder,
            PropertyAdId = propertyAdId
        };

        await _propertyMediaRepository.AddAsync(newMedia, ct);
        await _propertyMediaRepository.SaveChangesAsync(ct);

        return objectKey;
    }

    public async Task DeleteMediaAsync(int mediaId, CancellationToken ct = default)
    {
        var media = await _propertyMediaRepository.GetByIdAsync(mediaId, ct);
        if (media == null) throw new Exception($"Media with id {mediaId} not found");

        await _fileStorageService.DeleteFileAsync(media.ObjectKey, ct);
        _propertyMediaRepository.Delete(media);
        await _propertyMediaRepository.SaveChangesAsync(ct);
    }

    public async Task<List<Application.DTOs.PropertyMedia.PropertyMediaItemDto>> GetMediaForPropertyAdAsync(int propertyAdId, CancellationToken ct = default)
    {
        var mediaItems = await _propertyMediaRepository.GetByPropertyAdIdAsync(propertyAdId, ct);
        return _mapper.Map<List<Application.DTOs.PropertyMedia.PropertyMediaItemDto>>(mediaItems);
    }
}
