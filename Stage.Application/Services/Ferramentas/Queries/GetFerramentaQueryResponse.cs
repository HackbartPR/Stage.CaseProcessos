using Stage.Domain.DTOs;

namespace Stage.Application.Services.Ferramentas.Queries
{
    public class GetFerramentaQueryResponse : FerramentaDto
    {
        public GetFerramentaQueryResponse()
        {
            Processos = new HashSet<ProcessoDto>();
        }

        public ICollection<ProcessoDto> Processos { get; set; }
    }
}
