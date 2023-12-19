using Stage.Domain.Entities;

namespace Stage.Domain.Repositories
{
    public interface IProcessoRepository : IGenericRepository<Processo>
    {
        IQueryable<Processo> GetById(int id);

        IQueryable<Processo> GetByName(string name);

        IQueryable<Processo> GetByParent(int idParent);

        IQueryable<Processo> GetByAreas(IEnumerable<int> idsAreas);

        IQueryable<Processo> GetAll();
    }
}
