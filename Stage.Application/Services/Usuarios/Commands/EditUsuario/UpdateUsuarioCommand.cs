using MediatR;
using Stage.Domain.Config;

namespace Stage.Application.Services.Usuarios.Commands.EditUsuario
{
    public class UpdateUsuarioCommand : IRequest<BaseResponse<UpdateUsuarioCommandResponse>>
    {
        public UpdateUsuarioCommand()
        {
            IdsAreas = new HashSet<int>();
        }

        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool Active { get; set; } = true;

        public ICollection<int> IdsAreas { get; set; }
    }
}
