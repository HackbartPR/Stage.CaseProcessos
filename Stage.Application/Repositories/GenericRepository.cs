using Microsoft.EntityFrameworkCore;
using Stage.Domain.Repositories;
using static Stage.Domain.Config.Constants;

namespace Stage.Application.Repositories
{
    public class GenericRepository<Context, Entity> : IGenericRepository<Entity> where Entity : class where Context : DbContext
    {
        private readonly Context _context;

        public GenericRepository(Context context)
        {
            _context = context;
        }

        public async Task<Entity> CreateAsync(Entity entity, bool active = true, CancellationToken cancellationToken = default)
        {
            object entityAux = (object) entity;
            
            entityAux.GetType().GetProperty(GenericProperties.Active)!.SetValue(entityAux, true);
            entityAux.GetType().GetProperty(GenericProperties.CreatedAt)!.SetValue(entityAux, DateTime.UtcNow);

            entity = (Entity) entityAux;

            await _context.Set<Entity>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public Entity Delete(Entity entity)
        {
            _context.Set<Entity>().Remove(entity);
            return entity;
        }

        public bool DeleteRange(ICollection<Entity> entities)
        {
            _context.Set<Entity>().RemoveRange(entities);
            return true;
        }

        public IQueryable<Entity> GetEntity()
        {
            return _context.Set<Entity>().AsQueryable();
        }

        public Entity Update(Entity entity)
        {
            object entityAux = (object)entity;
            entityAux.GetType().GetProperty(GenericProperties.UpdatedAt)!.SetValue(entityAux, DateTime.UtcNow);
            entity = (Entity)entityAux;

            _context.Set<Entity>().Update(entity);
            return entity;
        }
    }
}
