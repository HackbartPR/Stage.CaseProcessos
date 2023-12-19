using Microsoft.EntityFrameworkCore;
using Stage.Domain.Entities;
using Stage.Domain.Repositories;
using Stage.Infrastructure.Persistence;

namespace Stage.Application.Repositories
{
    public class UsuarioRepository : GenericRepository<SqlContext, Usuario>, IUsuarioRepository
    {
        private readonly SqlContext _context;

        public UsuarioRepository(SqlContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Usuario> GetById(int Id)
        {
            return _context.Set<Usuario>()
                    .Include(u => u.Areas.Where(a => a.Active))
                    .Where(u => u.Active && u.Id == Id);
        }

        public IQueryable<Usuario> GetByAreas(IEnumerable<int> IdsAreas)
        {
            return _context.Set<Usuario>()
                .Include(u => u.Areas.Where(a => a.Active))
                .Where(u => u.Active && IdsAreas.Contains(u.Id));
        }

        public IQueryable<Usuario> GetByName(string name)
        {
            return _context.Set<Usuario>()
                .Include(u => u.Areas.Where(a => a.Active))
                .Where(u => u.Active && u.Name.Contains(name!));
        }

        public IQueryable<Usuario> GetAll()
        {
            return _context.Set<Usuario>()
                .Include(u => u.Areas.Where(a => a.Active))
                .Where(u => u.Active);
        }
    }
}
