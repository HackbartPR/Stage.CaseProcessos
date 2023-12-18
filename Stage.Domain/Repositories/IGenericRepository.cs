namespace Stage.Domain.Repositories
{
    public interface IGenericRepository<Entity> where Entity : class
    {
        IQueryable<Entity> GetEntity();

        Task<Entity> CreateAsync(Entity entity, bool active = true, CancellationToken cancellationToken = default);

        Entity Update(Entity entity);

        Entity Delete(Entity entity);

        bool DeleteRange(ICollection<Entity> entities);
    }
}
