using MediatR;
using Microsoft.EntityFrameworkCore;
using Stage.Domain.Config;
using Stage.Domain.DTOs;
using Stage.Domain.Entities;
using Stage.Domain.Notifications;
using Stage.Domain.UnitsOfWork;
using static Stage.Domain.Config.Constants;

namespace Stage.Application.Services.Usuarios.Commands.AddUsuario
{
    public class AddUsuarioCommandHandler : IRequestHandler<AddUsuarioCommand, BaseResponse<AddUsuarioCommandResponse>>
    {
        private readonly INotificationContext _notification;
        private readonly IUnitOfWork _unitOfWork;

        public AddUsuarioCommandHandler(IUnitOfWork unitOfWork, INotificationContext notification)
        {
            _unitOfWork = unitOfWork;
            _notification = notification;
        }

        public async Task<BaseResponse<AddUsuarioCommandResponse>> Handle(AddUsuarioCommand request, CancellationToken cancellationToken)
        {
            await ValidateBusinessRules(request, cancellationToken);
            
            Usuario usuario = await CreateUsuarioAsync(request, cancellationToken);

            AddUsuarioCommandResponse response = CreateResponse(usuario);

            return new BaseResponse<AddUsuarioCommandResponse>(response, _notification.Notifications(), true);
        }

        public async Task ValidateBusinessRules(AddUsuarioCommand request, CancellationToken cancellationToken)
        {
            foreach (int IdArea in request.IdsAreas)
            {
                if ( await _unitOfWork.AreaRepository.GetById(IdArea).FirstOrDefaultAsync(cancellationToken: cancellationToken) == null){
                    _notification.AddNotification(ErrorsKeys.AreaNotFound, ErrorsMessages.AreaNotFound);
                    throw new InvalidOperationException();
                }
            }
        }

        public async Task<ICollection<Area>> GetAreasAsync(AddUsuarioCommand request, CancellationToken cancellationToken)
        {
            ICollection<Area> areas = new List<Area>();

            foreach (int IdArea in request.IdsAreas)
                areas.Add(await _unitOfWork.AreaRepository.GetById(IdArea).FirstAsync(cancellationToken));

            return areas;
        }

        public async Task<Usuario> CreateUsuarioAsync(AddUsuarioCommand request, CancellationToken cancellationToken)
        {
            Usuario usuario = new()
            {
                Name = request.Name,
                Areas = await GetAreasAsync(request, cancellationToken)
            };

            await _unitOfWork.UsuarioRepository.CreateAsync(usuario, cancellationToken: cancellationToken);

            if (await _unitOfWork.AreaRepository.Commit())
                return usuario;

            _notification.AddNotification(ErrorsKeys.AreaNotSaved, ErrorsMessages.AreaNotSaved);
            throw new InvalidOperationException("Entity had a problem to save!");
        }

        public static AddUsuarioCommandResponse CreateResponse(Usuario usuario)
        {
            return new AddUsuarioCommandResponse()
            {
                Id = usuario.Id,
                Name = usuario.Name,
                Areas = usuario.Areas.Select(a => new AreaDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    IdResponsible = a.IdResponsible,
                }).ToList(),
            };
        }
    }
}
