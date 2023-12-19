using Stage.Domain.Entities;

namespace Stage.Domain.Repositories
{
    public interface IFerramentaRepository : IGenericRepository<Ferramenta>
    {
        IQueryable<Ferramenta> GetById(int id);

        IQueryable<Ferramenta> GetByName(string name);

        IQueryable<Ferramenta> GetByProcessos(IEnumerable<int> idsProcessos);

        IQueryable<Ferramenta> GetAll();
    }
}
