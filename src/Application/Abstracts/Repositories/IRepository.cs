using Domain;

namespace Application.Abstracts.Repositories;

public interface IRepository<Tentity,Tkey> where Tentity : BaseEntity<Tkey>
{
    Task<List<Tentity>> GetAllAsync(CancellationToken ct=default);
    Task<Tentity?> GetByIdAsync(Tkey id, CancellationToken ct=default);
    Task AddAsync(Tentity entity, CancellationToken ct=default);
    void Update(Tentity entity);
    void Delete(Tentity entity);

    Task SaveChangesAsync(CancellationToken ct=default);
}