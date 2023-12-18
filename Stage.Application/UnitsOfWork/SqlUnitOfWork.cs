using Stage.Application.Repositories;
using Stage.Domain.Repositories;
using Stage.Domain.UnitsOfWork;
using Stage.Infrastructure.Persistence;

namespace Stage.Application.UnitsOfWork
{
    public class SqlUnitOfWork : IUnitOfWork
    {
        private readonly SqlContext _context;

        private readonly IAreaRepository _areaRepository = null!;
        private readonly IFerramentaRepository _ferramentaRepository = null!;
        private readonly IProcessoRepository _processoRepository = null!;
        private readonly IUsuarioRepository _usuarioRepository = null!;

        public SqlUnitOfWork(SqlContext context)
        {
            _context = context;
        }

        public IAreaRepository AreaRepository 
        {
            get
            {
                if (_areaRepository == null) return new AreaRepository(_context);
                return _areaRepository;
            }
        }

        public IFerramentaRepository FerramentaRepository 
        {
            get
            {
                if (_ferramentaRepository == null) return new FerramentaRepository(_context);
                return _ferramentaRepository;
            }
        }

        public IProcessoRepository ProcessoRepository 
        {
            get
            {
                if (_processoRepository == null) return new ProcessoRepository(_context);
                return _processoRepository;
            }
        }

        public IUsuarioRepository UsuarioRepository 
        {
            get
            {
                if (_usuarioRepository == null) return new UsuarioRepository(_context);
                return _usuarioRepository;
            }
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
