using Stage.Domain.Repositories;

namespace Stage.Domain.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAreaRepository AreaRepository { get; }

        IFerramentaRepository FerramentaRepository { get; }

        IProcessoRepository ProcessoRepository { get; }

        IUsuarioRepository UsuarioRepository { get; }

        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
