using MediatR;
using Stage.Domain.Config;
using System.ComponentModel.DataAnnotations;

namespace Stage.Application.Services.Processos.Command.AddProcesso
{
    public class AddProcessoCommand : IRequest<BaseResponse<AddProcessoCommandResponse>>
    {
        public AddProcessoCommand()
        {
            SubProcessos = new HashSet<AddProcessoCommand>();
            IdsFerramentas = new HashSet<int>();
            IdsAreas = new HashSet<int>();
        }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int? IdParentProccess { get; set; }

        public ICollection<int> IdsFerramentas { get; set; }

        public ICollection<int> IdsAreas { get; set; }

        public ICollection<AddProcessoCommand> SubProcessos { get; set; }
    }
}
