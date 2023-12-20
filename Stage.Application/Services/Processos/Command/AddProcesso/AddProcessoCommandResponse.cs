using Stage.Domain.DTOs;

namespace Stage.Application.Services.Processos.Command.AddProcesso
{
    public class AddProcessoCommandResponse : ProcessoDto
    {
        public AddProcessoCommandResponse()
        {
            SubProcessos = new HashSet<AddProcessoCommandResponse>();
            Ferramentas = new HashSet<FerramentaDto>();
            Areas = new HashSet<AreaDto>();
        }

        public ProcessoDto? ParentProcess { get; set; } = null!;

        public ICollection<AddProcessoCommandResponse> SubProcessos { get; set; }

        public ICollection<FerramentaDto> Ferramentas { get; set; }

        public ICollection<AreaDto> Areas { get; set; }
    }
}
