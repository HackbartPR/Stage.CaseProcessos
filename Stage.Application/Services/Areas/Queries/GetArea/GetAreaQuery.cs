using MediatR;
using Stage.Domain.Config;

namespace Stage.Application.Services.Areas.Queries.GetArea
{
    public class GetAreaQuery : PagedBaseRequest, IRequest<PagedBaseResponse<ICollection<GetAreaQueryResponse>>>
    {
        public GetAreaQuery()
        {
            IdsProcessos = new HashSet<int>();
        }

        public int? Id { get; set; }

        public string? Name { get; set; }

        public int? IdResponsible { get; set; }

        public IEnumerable<int> IdsProcessos { get; set; }
    }
}
