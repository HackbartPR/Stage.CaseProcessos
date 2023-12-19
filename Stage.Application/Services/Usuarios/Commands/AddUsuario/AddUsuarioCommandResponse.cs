using Stage.Domain.DTOs;

namespace Stage.Application.Services.Usuarios.Commands.AddUsuario
{
    public class AddUsuarioCommandResponse : UsuarioDto
    {
        public AddUsuarioCommandResponse()
        {
            Areas = new HashSet<AreaDto>();
        }

        public ICollection<AreaDto> Areas { get; set; }
    }
}
