using System.Linq.Expressions;

namespace GreenConnectPlatform.Data.Repositories.Base;

public interface IBaseRepository<TEntity, TKey> where TEntity : class
{
    // 1. GET (Chỉ trả về Data, không trả IQueryable)
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    // Hàm check tồn tại nhanh gọn
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

    // 2. CREATE
    Task<TEntity> AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    // 3. UPDATE
    Task UpdateAsync(TEntity entity);

    // 4. DELETE
    Task DeleteAsync(TEntity entity);

    // 5. SAVE (Tùy chọn, nếu bạn muốn control việc save change từ Service)
    Task<bool> SaveChangesAsync();
}