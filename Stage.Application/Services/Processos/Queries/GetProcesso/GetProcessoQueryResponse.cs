using Stage.Domain.DTOs;

namespace Stage.Application.Services.Processos.Queries.GetProcesso
{
    public class GetProcessoQueryResponse : ProcessoDto
    {
        public GetProcessoQueryResponse()
        {
            Areas = new HashSet<AreaDto>();
            SubProcessos = new HashSet<GetProcessoQueryResponse>();
            Ferramentas = new HashSet<FerramentaDto>();
        }

        public ICollection<AreaDto> Areas { get; set; }

        public ProcessoDto? ParentProcess { get; set; } = null!;

        public ICollection<GetProcessoQueryResponse> SubProcessos { get; set; }

        public ICollection<FerramentaDto> Ferramentas { get; set; }
    }
}
