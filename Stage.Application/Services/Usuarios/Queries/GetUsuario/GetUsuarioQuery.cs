using MediatR;
using Stage.Domain.Config;

namespace Stage.Application.Services.Usuarios.Queries.GetUsuario
{
    public class GetUsuarioQuery : PagedBaseRequest, IRequest<PagedBaseResponse<ICollection<GetUsuarioQueryResponse>>>
    {
        public GetUsuarioQuery()
        {
            IdsAreas = new HashSet<int>();
        }

        public int? Id { get; set; }

        public string? Name { get; set; }

        public IEnumerable<int> IdsAreas { get; set; }
    }
}
