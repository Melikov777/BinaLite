using Domain.Entities;

namespace Application.Abstracts.Repositories;

public interface IPropertyAdRepository:IRepository<PropertyAd,int>
{
    List<PropertyAd> GetAllWithMedia();
    PropertyAd GetByIdWithMedia(int id);
}
