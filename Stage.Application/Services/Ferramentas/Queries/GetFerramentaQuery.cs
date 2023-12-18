using MediatR;
using Stage.Domain.Config;

namespace Stage.Application.Services.Ferramentas.Queries
{
    public class GetFerramentaQuery : PagedBaseRequest, IRequest<PagedBaseResponse<ICollection<GetFerramentaQueryResponse>>>
    {
        public GetFerramentaQuery()
        {
            IdsProcessos = new HashSet<int>();
        }

        public int? Id { get; set; }

        public string? Name { get; set; }

        public IEnumerable<int> IdsProcessos { get; set; }
    }
}
