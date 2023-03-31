namespace CapaRepositorio.Interface;

public interface IGenericRepository<T>
{
    //Get
    T GetById(object id);
    IEnumerable<T> GetAll();
    IEnumerable<T> GetAllIdNoTracking();
    //GetAsync
    Task<IEnumerable<T>> GetAllAsyncIdNoTracking();
    Task<T> GetByIdAsync(object id);
    Task<IEnumerable<T>> GetAllAsync();

    //Create
    void Create(T entity);
    void CreateRange(IEnumerable<T> entities);
    T CreateEntity(T entity);
    IEnumerable<T> CreateEntityRange(IEnumerable<T> entities);
    //CreateAsync
    Task CreateAsync(T entity);
    Task CreateRangeAsync(IEnumerable<T> entities);
    Task<T> CreateEntityAsync(T entity);
    Task<IEnumerable<T>> CreateRangeEntityAsync(IEnumerable<T> entities);

    //Update
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    //UpdateAsync
    Task UpdateAsync(T entity);
    Task UpdateRangeAsync(IEnumerable<T> entities);

    //Delete
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
    //DeleteAsync
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(IEnumerable<T> entities);

    //Exist
    bool Exists(object id);
}