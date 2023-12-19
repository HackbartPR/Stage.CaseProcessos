using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stage.Domain.Config;
using Stage.Domain.DTOs;
using Stage.Domain.Entities;
using Stage.Domain.Notifications;
using Stage.Domain.UnitsOfWork;

namespace Stage.Application.Services.Processos.Queries.GetProcesso
{
    public class GetProcessoQueryHandler : IRequestHandler<GetProcessoQuery, PagedBaseResponse<ICollection<GetProcessoQueryResponse>>>
    {
        private readonly INotificationContext _notification;
        private readonly IUnitOfWork _unitOfWork;

        public GetProcessoQueryHandler(IUnitOfWork unitOfWork, INotificationContext notification)
        {
            _unitOfWork = unitOfWork;
            _notification = notification;
        }

        public async Task<PagedBaseResponse<ICollection<GetProcessoQueryResponse>>> Handle(GetProcessoQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Processo> query = QueryProcesso(request);

            int totalItems = await query.CountAsync(cancellationToken);
            ICollection<Processo> resultPaged = await ToListAsync(query, request, cancellationToken);

            return new PagedBaseResponse<ICollection<GetProcessoQueryResponse>>()
            {
                Page = request.Page,
                TotalItems = totalItems,
                Notifications = _notification.Notifications(),
                Result = CreateResponse(resultPaged),
                Success = true,
                TotalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize)
            };
        }

        public IQueryable<Processo> QueryProcesso(GetProcessoQuery request)
        {
            if (request.Id != null && request.Id > 0)
                return _unitOfWork.ProcessoRepository.GetById((int) request.Id);

            if (!request.Name.IsNullOrEmpty())
                return _unitOfWork.ProcessoRepository.GetByName(request.Name!);

            if (request.IdParentProccess != null && request.IdParentProccess > 0)
                return _unitOfWork.ProcessoRepository.GetByParent((int) request.IdParentProccess);

            if (!request.IdsAreas.Any(id => id <= 0))
                return _unitOfWork.ProcessoRepository.GetByAreas(request.IdsAreas);

            return _unitOfWork.ProcessoRepository.GetAll();
        }

        public async Task<ICollection<Processo>> ToListAsync(IQueryable<Processo> query, GetProcessoQuery request, CancellationToken cancellationToken)
        {
            request.ValidatePageRequest();
            return await _unitOfWork.ProcessoRepository.ToPagedListAsync(query, request, cancellationToken);
        }

        public static ICollection<GetProcessoQueryResponse> CreateResponse(ICollection<Processo> processos)
        {
            return processos.Select(p => new GetProcessoQueryResponse()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                IdParentProccess = p.IdParentProccess,
                ParentProcess = p.IdParentProccess != null ? new ProcessoDto 
                {
                    Id = p.ParentProcess.Id,
                    Name = p.ParentProcess.Name,
                    Description = p.ParentProcess.Description,
                    IdParentProccess = p.ParentProcess.IdParentProccess
                } : null,
                SubProcessos = CreateResponse(p.SubProcessos),
                Areas = p.Areas.Select(a => new AreaDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    IdResponsible = a.IdResponsible,
                }).ToList(),
                Ferramentas = p.Ferramentas.Select(f => new FerramentaDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Description = f.Description,
                }).ToList()
            }).ToList();
        }
    }
}
