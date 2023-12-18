using MediatR;
using Stage.Domain.Config;

namespace Stage.Application.Services.Processos.Queries.GetProcesso
{
    public class GetProcessoQuery : PagedBaseRequest, IRequest<PagedBaseResponse<ICollection<GetProcessoQueryResponse>>>
    {
        public GetProcessoQuery()
        {
            IdsAreas = new HashSet<int>();
        }

        public int? Id { get; set; }

        public string? Name { get; set; }

        public int? IdParentProccess { get; set; }

        public IEnumerable<int> IdsAreas { get; set; }
    }
}
