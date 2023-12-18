using Stage.Domain.DTOs;

namespace Stage.Application.Services.Areas.Queries.GetArea
{
    public class GetAreaQueryResponse : AreaDto
    {
        public GetAreaQueryResponse()
        {
            Processos = new HashSet<ProcessoDto>();
        }

        public UsuarioDto? Responsible { get; set; }

        public ICollection<ProcessoDto> Processos { get; set; }
    }
}
