using Stage.Domain.DTOs;

namespace Stage.Application.Services.Usuarios.Commands.EditUsuario
{
    public class UpdateUsuarioCommandResponse : UsuarioDto
    {
        public UpdateUsuarioCommandResponse()
        {
            Areas = new HashSet<AreaDto>();
        }

        public ICollection<AreaDto> Areas { get; set; }
    }
}
