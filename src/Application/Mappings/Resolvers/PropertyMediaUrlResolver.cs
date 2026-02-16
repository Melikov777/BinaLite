using Application.Abstracts.Services;
using Application.DTOs.PropertyAd;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings.Resolvers;

public class PropertyMediaUrlResolver : IValueResolver<PropertyAd, object, List<string>>
{
    private readonly IFileStorageService _fileStorageService;

    public PropertyMediaUrlResolver(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    public List<string> Resolve(PropertyAd source, object destination, List<string> destMember, ResolutionContext context)
    {
        if (source.MediaItems == null || !source.MediaItems.Any())
            return new List<string>();

        return source.MediaItems
            .Select(pm => _fileStorageService.GetFileUrl(pm.ObjectKey))
            .ToList();
    }
}