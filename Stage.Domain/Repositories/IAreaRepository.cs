using Stage.Domain.Entities;

namespace Stage.Domain.Repositories
{
    public interface IAreaRepository : IGenericRepository<Area>
    {
        IQueryable<Area> GetById(int id);

        IQueryable<Area> GetByName(string name);

        IQueryable<Area> GetByResponsible(int idResponsible);

        IQueryable<Area> GetByProcessos(IEnumerable<int> idsProcessos);

        IQueryable<Area> GetAll();
    }
}
