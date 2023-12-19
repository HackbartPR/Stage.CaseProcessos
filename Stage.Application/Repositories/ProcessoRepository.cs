using Microsoft.EntityFrameworkCore;
using Stage.Domain.Entities;
using Stage.Domain.Repositories;
using Stage.Infrastructure.Persistence;

namespace Stage.Application.Repositories
{
    public class ProcessoRepository : GenericRepository<SqlContext, Processo>, IProcessoRepository
    {
        private readonly SqlContext _context;

        public ProcessoRepository(SqlContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Processo> GetById(int id)
        {
            return _context.Set<Processo>()
                .Include(p => p.ParentProcess)
                .Include(p => p.Ferramentas.Where(f => f.Active))
                .Include(p => p.SubProcessos.Where(s => s.Active))
                .Where(p => p.Active && p.Id == id);
        }

        public IQueryable<Processo> GetByName(string name)
        {
            return _context.Set<Processo>()
                .Include(p => p.ParentProcess)
                .Include(p => p.Ferramentas.Where(f => f.Active))
                .Include(p => p.SubProcessos.Where(s => s.Active))
                .Where(u => u.Active && u.Name.Contains(name!));
        }

        public IQueryable<Processo> GetByParent(int idParent)
        {
            return _context.Set<Processo>()
                .Include(p => p.ParentProcess)
                .Include(p => p.Ferramentas.Where(f => f.Active))
                .Include(p => p.SubProcessos.Where(s => s.Active))
                .Where(u => u.Active && u.IdParentProccess != null && u.IdParentProccess == idParent);
        }

        public IQueryable<Processo> GetByAreas(IEnumerable<int> idsAreas)
        {
            return _context.Set<Processo>()
                .Include(p => p.ParentProcess)
                .Include(p => p.Ferramentas.Where(f => f.Active))
                .Include(p => p.SubProcessos.Where(s => s.Active))
                .Where(a => a.Active && idsAreas.Contains(a.Id));
        }

        public IQueryable<Processo> GetAll()
        {
            return _context.Set<Processo>()
                .Include(p => p.ParentProcess)
                .Include(p => p.Ferramentas.Where(f => f.Active))
                .Include(p => p.SubProcessos.Where(s => s.Active))
                .Where(a => a.Active);
        }
    }
}
