using Stage.Domain.DTOs;

namespace Stage.Application.Services.Processos.Queries.GetProcesso
{
    public class GetProcessoQueryResponse
    {
        public GetProcessoQueryResponse()
        {
            Areas = new HashSet<AreaDto>();
            SubProcessos = new HashSet<ProcessoDto>();
            Ferramentas = new HashSet<FerramentaDto>();
        }

        public ICollection<AreaDto> Areas { get; set; }

        public ProcessoDto? ParentProcess { get; set; } = null!;

        public ICollection<ProcessoDto> SubProcessos { get; set; } = new HashSet<ProcessoDto>();

        public ICollection<FerramentaDto> Ferramentas { get; set; } = new HashSet<FerramentaDto>();
    }
}
