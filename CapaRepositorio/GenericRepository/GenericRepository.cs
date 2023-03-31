using CapaDatos.Context;
using CapaRepositorio.Interface;
using Microsoft.EntityFrameworkCore;

namespace CapaRepositorio.GenericRepository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly string _msgEntityNotFound = "Request entity not found";

    private readonly CONTROLDECALIDADContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(CONTROLDECALIDADContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    //Get
    public virtual T GetById(object id)
    {
        var item = _dbSet.Find(id);

        if (item == null)
            throw new ApplicationException(_msgEntityNotFound);

        return item;
    }
    public virtual IEnumerable<T> GetAll() => _dbSet.ToList();
    public virtual IEnumerable<T> GetAllIdNoTracking() => _dbSet.AsNoTracking().ToList();
    //GetAsync
    public async virtual Task<T> GetByIdAsync(object id)
    {
        var item = await _dbSet.FindAsync(id);

        if (item == null)
            throw new ApplicationException(_msgEntityNotFound);

        return item;
    }
    public async virtual Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
    public async virtual Task<IEnumerable<T>> GetAllAsyncIdNoTracking() => await _dbSet.AsNoTracking().ToListAsync();

    //Create
    public virtual void Create(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _dbSet.Add(entity);
        _context.SaveChanges();
    }
    public T CreateEntity(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _dbSet.Add(entity);
        _context.SaveChanges();

        return entity;
    }
    public virtual void CreateRange(IEnumerable<T> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        _dbSet.AddRange(entities);
        _context.SaveChanges();
    }
    public IEnumerable<T> CreateEntityRange(IEnumerable<T> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        _dbSet.AddRange(entities);
        _context.SaveChanges();

        return entities;
    }
    //CreateAsync
    public async virtual Task CreateAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();

    }
    public async Task<T> CreateEntityAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await _dbSet.AddAsync(entity);

        return entity;
    }
    public async virtual Task CreateRangeAsync(IEnumerable<T> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        await _dbSet.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<T>> CreateRangeEntityAsync(IEnumerable<T> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        await _dbSet.AddRangeAsync(entities);
        await _context.SaveChangesAsync();

        return (List<T>)entities;
    }

    //Update
    public virtual void Update(T entity)
    {
        if (entity == null)
            throw new ArgumentException($"Entity not found {nameof(entity)}");

        _dbSet.Update(entity);
    }
    public virtual void UpdateRange(IEnumerable<T> entities)
    {
        if (entities == null)
            throw new ArgumentException($"Entity not found {nameof(entities)}");

        _dbSet.UpdateRange(entities);
    }
    //UpdateAsync
    public async virtual Task UpdateAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentException($"Entity not found {nameof(entity)}");

        _context.Entry(entity).State = EntityState.Modified;

        _dbSet.Update(entity);
    }
    public async virtual Task UpdateRangeAsync(IEnumerable<T> entities)
    {
        if (entities == null)
            throw new ArgumentException($"Entity not found {nameof(entities)}");

        _dbSet.UpdateRange(entities);
    }


    //Delete
    public virtual void Delete(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        if (_context.Entry(entity).State == EntityState.Detached)
            _dbSet.Attach(entity);

        _dbSet.Remove(entity);
    }
    public virtual void DeleteRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }
    //DeleteAsync
    public async virtual Task DeleteAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        if (_context.Entry(entity).State == EntityState.Detached)
            _dbSet.Attach(entity);

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
    public async virtual Task DeleteRangeAsync(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
        await _context.SaveChangesAsync();
    }

    //Exists
    public bool Exists(object id)
    {
        if (id == null)
            throw new NotImplementedException(_msgEntityNotFound);

        return _dbSet.Find(id) != null;
    }
}
