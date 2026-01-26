using Application.DTOs.PropertyAd;

namespace Application.Abstracts.Services;

public interface IPropertyAdService
{
    List<GetAllPropertyAdResponse> GetAll();
    GetByIdPropertyAdResponse GetById(int id);
    void Create(CreatePropertyAdRequest request);
    void Update(int id, CreatePropertyAdRequest request);
    void Delete(int id);
}
