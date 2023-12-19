using Microsoft.EntityFrameworkCore;
using Stage.Domain.Entities;
using Stage.Domain.Repositories;
using Stage.Infrastructure.Persistence;

namespace Stage.Application.Repositories
{
    public class FerramentaRepository : GenericRepository<SqlContext, Ferramenta>, IFerramentaRepository
    {
        private readonly SqlContext _context;

        public FerramentaRepository(SqlContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Ferramenta> GetById(int id)
        {
            return _context.Set<Ferramenta>()
                    .Include(a => a.Processos.Where(p => p.Active))
                    .Where(a => a.Active && a.Id == id);
        }

        public IQueryable<Ferramenta> GetByName(string name)
        {
            return _context.Set<Ferramenta>()
                .Include(a => a.Processos.Where(p => p.Active))
                .Where(u => u.Active && u.Name.Contains(name!));
        }

        public IQueryable<Ferramenta> GetByProcessos(IEnumerable<int> idsProcessos)
        {
            return _context.Set<Ferramenta>()
                .Include(a => a.Processos.Where(p => p.Active))
                .Where(a => a.Active && idsProcessos.Contains(a.Id));
        }

        public IQueryable<Ferramenta> GetAll()
        {
            return _context.Set<Ferramenta>()
                .Include(a => a.Processos.Where(p => p.Active))
                .Where(a => a.Active);
        }
    }
}
