using Microsoft.EntityFrameworkCore;
using Stage.Domain.Entities;
using Stage.Domain.Repositories;
using Stage.Infrastructure.Persistence;

namespace Stage.Application.Repositories
{
    public class AreaRepository : GenericRepository<SqlContext, Area>, IAreaRepository
    {
        private readonly SqlContext _context;

        public AreaRepository(SqlContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Area> GetById(int id)
        {
            return _context.Set<Area>()
                .Include(a => a.Responsible)
                .Include(a => a.Processos.Where(p => p.Active))
                .Where(a => a.Active && a.Id == id);
        }

        public IQueryable<Area> GetByName(string name)
        {
            return _context.Set<Area>()
                .Include(a => a.Responsible)
                .Include(a => a.Processos.Where(p => p.Active))
                .Where(u => u.Active && u.Name.Contains(name!));
        }

        public IQueryable<Area> GetByResponsible(int idResponsible)
        {
            return _context.Set<Area>()
                .Include(a => a.Responsible)
                .Include(a => a.Processos.Where(p => p.Active))
                .Where(u => u.Active && u.IdResponsible != null && u.IdResponsible == idResponsible);
        }

        public IQueryable<Area> GetByProcessos(IEnumerable<int> idsProcessos)
        {
            return _context.Set<Area>()
                .Include(a => a.Responsible)
                .Include(a => a.Processos.Where(p => p.Active))
                .Where(a => a.Active && idsProcessos.Contains(a.Id));
        }

        public IQueryable<Area> GetAll()
        {
            return _context.Set<Area>()
                .Include(a => a.Responsible)
                .Include(a => a.Processos.Where(p => p.Active))
                .Where(a => a.Active);
        }
    }
}
