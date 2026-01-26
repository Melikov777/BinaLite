using Domain;

namespace Application.Abstracts.Repositories;

public interface IRepository<Tentity,Tkey> where Tentity : BaseEntity<Tkey>
{
    List<Tentity> GetAll();
    void Add(Tentity entity);
    void Update(Tentity entity);
    void Delete(Tentity entity);
    Tentity GetById(Tkey id);
    void SaveChanges();
}
