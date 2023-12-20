using MediatR;
using Microsoft.EntityFrameworkCore;
using Stage.Domain.Config;
using Stage.Domain.DTOs;
using Stage.Domain.Entities;
using Stage.Domain.Notifications;
using Stage.Domain.UnitsOfWork;
using static Stage.Domain.Config.Constants;

namespace Stage.Application.Services.Usuarios.Commands.EditUsuario
{
    public class UpdateUsuarioCommandHandler : IRequestHandler<UpdateUsuarioCommand, BaseResponse<UpdateUsuarioCommandResponse>>
    {
        private readonly INotificationContext _notification;
        private readonly IUnitOfWork _unitOfWork;

        private Usuario? _usuario;

        public UpdateUsuarioCommandHandler(IUnitOfWork unitOfWork, INotificationContext notification)
        {
            _unitOfWork = unitOfWork;
            _notification = notification;
        }

        public async Task<BaseResponse<UpdateUsuarioCommandResponse>> Handle(UpdateUsuarioCommand request, CancellationToken cancellationToken)
        {
            await ValidateBusinessRules(request, cancellationToken);

            await UpdateUsuario(request, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            UpdateUsuarioCommandResponse response = CreateResponse();

            return new BaseResponse<UpdateUsuarioCommandResponse>(response, _notification.Notifications(), true);
        }

        public async Task ValidateBusinessRules(UpdateUsuarioCommand request, CancellationToken cancellationToken)
        {
            _usuario = await _unitOfWork.UsuarioRepository.GetById(request.Id).FirstOrDefaultAsync(cancellationToken);
            if (_usuario == null)
            {
                _notification.AddNotification(ErrorsKeys.EntityNotFound, ErrorsMessages.UsuarioNotFound);
                throw new InvalidOperationException();
            }
            
            foreach (int IdArea in request.IdsAreas)
            {
                if (await _unitOfWork.AreaRepository.GetById(IdArea).FirstOrDefaultAsync(cancellationToken) == null)
                {
                    _notification.AddNotification(ErrorsKeys.EntityNotFound, ErrorsMessages.AreaNotFound);
                    throw new InvalidOperationException();
                }
            }
        }

        public async Task UpdateUsuario(UpdateUsuarioCommand request, CancellationToken cancellationToken)
        {
            _usuario!.Name = request.Name;
            _usuario.Active = request.Active;

            RemoveUnselectedAreas(request.IdsAreas);
            await AddNewAreasAsync(request.IdsAreas, cancellationToken);

            _unitOfWork.UsuarioRepository.Update(_usuario);
        }

        public void RemoveUnselectedAreas(IEnumerable<int> newIdsArea)
        {
            foreach (var area in _usuario!.Areas)
            {
                if (!newIdsArea.Contains(area.Id))
                    _usuario.Areas.Remove(area);
            }
        }

        public async Task AddNewAreasAsync(IEnumerable<int> newIdsArea, CancellationToken cancellationToken)
        {
            foreach (var newId in newIdsArea)
            {
                var newArea = await _unitOfWork.AreaRepository.GetById(newId).FirstAsync(cancellationToken);
                _usuario!.Areas.Add(newArea);
            }
        }

        public UpdateUsuarioCommandResponse CreateResponse()
        {
            return new UpdateUsuarioCommandResponse()
            {
                Id = _usuario!.Id,
                Name = _usuario.Name,
                Areas = _usuario.Areas.Select(a => new AreaDto
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
