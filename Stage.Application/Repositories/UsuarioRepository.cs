using Stage.Domain.Entities;
using Stage.Domain.Repositories;
using Stage.Infrastructure.Persistence;

namespace Stage.Application.Repositories
{
    public class UsuarioRepository : GenericRepository<SqlContext, Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(SqlContext context) : base(context)
        {
        }
    }
}
