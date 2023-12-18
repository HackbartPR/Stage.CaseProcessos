using Stage.Domain.Entities;
using Stage.Domain.Repositories;
using Stage.Infrastructure.Persistence;

namespace Stage.Application.Repositories
{
    public class AreaRepository : GenericRepository<SqlContext, Area>, IAreaRepository
    {
        public AreaRepository(SqlContext context) : base(context)
        {
        }
    }
}
