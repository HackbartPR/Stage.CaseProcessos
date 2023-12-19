using Stage.Domain.Entities;

namespace Stage.Domain.Repositories
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        IQueryable<Usuario> GetById(int Id);

        IQueryable<Usuario> GetByAreas(IEnumerable<int> IdsAreas);

        IQueryable<Usuario> GetByName(string name);

        IQueryable<Usuario> GetAll();
    }
}
