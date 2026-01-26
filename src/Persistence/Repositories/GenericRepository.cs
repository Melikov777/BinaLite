using Application.Abstracts.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class GenericRepository<Tentity, Tkey> : IRepository<Tentity, Tkey> where Tentity : BaseEntity<Tkey>
{
    private readonly BinaLiteDbContext _context;
    private readonly DbSet<Tentity> _table;
    public GenericRepository(BinaLiteDbContext context)
    {
        _context = context;
        _table = _context.Set<Tentity>();
    }
    public void Add(Tentity entity)
    {  
        _table.Add(entity);
    }

    public void Delete(Tentity entity)
    {
        _table.Remove(entity);
    }

    public List<Tentity> GetAll()
    {
        return _table.ToList();
    }

    public Tentity GetById(Tkey id)
    {
        return _table.Find(id);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public void Update(Tentity entity)
    {
        _table.Update(entity);
    }
}
