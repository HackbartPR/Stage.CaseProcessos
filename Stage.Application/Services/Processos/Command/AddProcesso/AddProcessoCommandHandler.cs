using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Stage.Application.Services.Usuarios.Commands.AddUsuario;
using Stage.Domain.Config;
using Stage.Domain.DTOs;
using Stage.Domain.Entities;
using Stage.Domain.Notifications;
using Stage.Domain.UnitsOfWork;
using static Stage.Domain.Config.Constants;

namespace Stage.Application.Services.Processos.Command.AddProcesso
{
    public class AddProcessoCommandHandler : IRequestHandler<AddProcessoCommand, BaseResponse<AddProcessoCommandResponse>>
    {
        private readonly INotificationContext _notification;
        private readonly IUnitOfWork _unitOfWork;

        public AddProcessoCommandHandler(IUnitOfWork unitOfWork, INotificationContext notification)
        {
            _unitOfWork = unitOfWork;
            _notification = notification;
        }

        public async Task<BaseResponse<AddProcessoCommandResponse>> Handle(AddProcessoCommand request, CancellationToken cancellationToken)
        {
            Processo processo = await BuildProcesso(request, cancellationToken);

            await CreateProcessoAsync(processo, cancellationToken);

            AddProcessoCommandResponse response = CreateResponse(processo);

            return new BaseResponse<AddProcessoCommandResponse>(response, _notification.Notifications(), true);
        }

        public async Task<Processo> BuildProcesso(AddProcessoCommand request, CancellationToken cancellationToken)
        {
            Processo? parent = await GetParentProcesso(request.IdParentProccess, cancellationToken);

            ICollection<Ferramenta> ferramentas = await GetFerramentas(request.IdsFerramentas, cancellationToken);

            ICollection<Area> areas = await GetAreas(request.IdsAreas, cancellationToken);

            ICollection<Processo> subProcessos = new List<Processo>();
            foreach (var sub in request.SubProcessos)
            {
                subProcessos.Add(await BuildProcesso(sub, cancellationToken));
            }

            return new Processo
            {
                Areas = areas,
                Name = request.Name,
                ParentProcess = parent,
                Ferramentas = ferramentas,
                SubProcessos = subProcessos,
                Description = request.Description,
                IdParentProccess = request.IdParentProccess,
            };
        }

        public async Task<Processo?> GetParentProcesso(int? idParent, CancellationToken cancellationToken)
        {
            Processo? parent = null;
            if (idParent != null)
            {
                parent = await _unitOfWork.ProcessoRepository.GetById((int) idParent).FirstOrDefaultAsync(cancellationToken);
                if (parent == null)
                {
                    _notification.AddNotification(ErrorsKeys.EntityNotFound, ErrorsMessages.ProcessoNotFound);
                    throw new InvalidOperationException();
                }
            }

            return parent;
        }

        public async Task<List<Ferramenta>> GetFerramentas(ICollection<int> idsFerramentas, CancellationToken cancellationToken)
        {
            List<Ferramenta> ferramentas = new();
            foreach (int IdFerramenta in idsFerramentas)
            {
                Ferramenta? ferramenta = await _unitOfWork.FerramentaRepository.GetById(IdFerramenta).FirstOrDefaultAsync(cancellationToken);
                if (ferramenta == null)
                {
                    _notification.AddNotification(ErrorsKeys.EntityNotFound, ErrorsMessages.FerramentaNotFound);
                    throw new InvalidOperationException();
                }

                ferramentas.Add(ferramenta);
            }

            return ferramentas;
        }

        public async Task<List<Area>> GetAreas(ICollection<int> idsAreas, CancellationToken cancellationToken)
        {
            List<Area> areas = new();
            foreach (int IdArea in idsAreas)
            {
                Area? area = await _unitOfWork.AreaRepository.GetById(IdArea).FirstOrDefaultAsync(cancellationToken);
                if (area == null)
                {
                    _notification.AddNotification(ErrorsKeys.EntityNotFound, ErrorsMessages.AreaNotFound);
                    throw new InvalidOperationException();
                }

                areas.Add(area);
            }

            return areas;
        }

        public async Task CreateProcessoAsync(Processo processo, CancellationToken cancellationToken)
        {
            await CreateSubProcessoAsync(processo, cancellationToken);

            if (await _unitOfWork.ProcessoRepository.Commit())
                return;

            _notification.AddNotification(ErrorsKeys.EntityNotSaved, ErrorsMessages.ProcessoNotSaved);
            throw new InvalidOperationException("Entity had a problem to save!");
        }

        public async Task CreateSubProcessoAsync(Processo processo, CancellationToken cancellationToken)
        {
            await _unitOfWork.ProcessoRepository.CreateAsync(processo, cancellationToken: cancellationToken);

            foreach (var sub in processo.SubProcessos)
            {
                if (sub != null)
                    await CreateSubProcessoAsync(sub, cancellationToken);
            }
        }

        public static AddProcessoCommandResponse CreateResponse(Processo processo)
        {
            return new AddProcessoCommandResponse()
            {
                Id = processo.Id,
                Name = processo.Name,
                Description = processo.Description,
                IdParentProccess = processo.IdParentProccess,
                ParentProcess = processo.ParentProcess != null ? new ProcessoDto 
                {
                    Id = processo.ParentProcess.Id,
                    Name = processo.ParentProcess.Name,
                    Description = processo.ParentProcess.Description,
                    IdParentProccess = processo.ParentProcess.IdParentProccess
                } : null,
                Areas = processo.Areas.Select(a => new AreaDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    IdResponsible = a.IdResponsible 
                }).ToList(),
                Ferramentas = processo.Ferramentas.Select(f => new FerramentaDto
                {
                    Id = f.Id,
                    Name= f.Name,
                    Description = f.Description,
                }).ToList(),
                SubProcessos = processo.SubProcessos.Select(s => CreateResponse(s)).ToList(),
            };
        }
    }
}
