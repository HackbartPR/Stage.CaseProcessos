using MediatR;
using Stage.Domain.Config;
using System.ComponentModel.DataAnnotations;

namespace Stage.Application.Services.Usuarios.Commands.AddUsuario
{
    public class AddUsuarioCommand : IRequest<BaseResponse<AddUsuarioCommandResponse>>
    {
        public AddUsuarioCommand()
        {
            IdsAreas = new HashSet<int>();
        }

        [Required]
        public string Name { get; set; } = string.Empty;

        public ICollection<int> IdsAreas { get; set; }
    }
}
