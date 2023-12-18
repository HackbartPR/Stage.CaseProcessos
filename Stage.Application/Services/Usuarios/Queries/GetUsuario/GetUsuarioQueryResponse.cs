using Stage.Domain.DTOs;

namespace Stage.Application.Services.Usuarios.Queries.GetUsuario
{
    public class GetUsuarioQueryResponse : UsuarioDto
    {
        public ICollection<AreaDto> Areas { get; set; } = new HashSet<AreaDto>();
    }
}
