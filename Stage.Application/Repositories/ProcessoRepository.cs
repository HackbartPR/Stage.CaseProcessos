using Stage.Domain.Entities;
using Stage.Domain.Repositories;
using Stage.Infrastructure.Persistence;

namespace Stage.Application.Repositories
{
    public class ProcessoRepository : GenericRepository<SqlContext, Processo>, IProcessoRepository
    {
        public ProcessoRepository(SqlContext context) : base(context)
        {
        }
    }
}
