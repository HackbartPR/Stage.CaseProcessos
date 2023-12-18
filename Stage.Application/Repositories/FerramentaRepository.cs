using Stage.Domain.Entities;
using Stage.Domain.Repositories;
using Stage.Infrastructure.Persistence;

namespace Stage.Application.Repositories
{
    public class FerramentaRepository : GenericRepository<SqlContext, Ferramenta>, IFerramentaRepository
    {
        public FerramentaRepository(SqlContext context) : base(context)
        {
        }
    }
}
