using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stage.Domain.Config;
using Stage.Domain.DTOs;
using Stage.Domain.Entities;
using Stage.Domain.Notifications;
using Stage.Domain.UnitsOfWork;

namespace Stage.Application.Services.Areas.Queries.GetArea
{
    public class GetAreaQueryHandler : IRequestHandler<GetAreaQuery, PagedBaseResponse<ICollection<GetAreaQueryResponse>>>
    {
        private readonly INotificationContext _notification;
        private readonly IUnitOfWork _unitOfWork;

        public GetAreaQueryHandler(IUnitOfWork unitOfWork, INotificationContext notification)
        {
            _unitOfWork = unitOfWork;
            _notification = notification;
        }

        public async Task<PagedBaseResponse<ICollection<GetAreaQueryResponse>>> Handle(GetAreaQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Area> query = QueryArea(request);

            int totalItems = await query.CountAsync(cancellationToken);
            ICollection<Area> resultPaged = await ToListAsync(query, request, cancellationToken);

            return new PagedBaseResponse<ICollection<GetAreaQueryResponse>>()
            {
                Page = request.Page,
                TotalItems = totalItems,
                Notifications = _notification.Notifications(),
                Result = CreateResponse(resultPaged),
                Success = true,
                TotalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize)
            };
        }

        public IQueryable<Area> QueryArea(GetAreaQuery request)
        {
            if (request.Id != null && request.Id > 0)
                return _unitOfWork.AreaRepository.GetById((int) request.Id);

            if (!request.Name.IsNullOrEmpty())
                return _unitOfWork.AreaRepository.GetByName(request.Name!);

            if (request.IdResponsible != null &&  request.IdResponsible > 0)
                return _unitOfWork.AreaRepository.GetByResponsible((int) request.IdResponsible);

            if (!request.IdsProcessos.Any(id => id <= 0))
                return _unitOfWork.AreaRepository.GetByProcessos(request.IdsProcessos);

            return _unitOfWork.AreaRepository.GetAll();
        }

        public async Task<ICollection<Area>> ToListAsync(IQueryable<Area> query, GetAreaQuery request, CancellationToken cancellationToken)
        {
            request.ValidatePageRequest();
            return await _unitOfWork.AreaRepository.ToPagedListAsync(query, request, cancellationToken);
        }

        public static ICollection<GetAreaQueryResponse> CreateResponse(ICollection<Area> usuarios)
        {
            return usuarios.Select(a => new GetAreaQueryResponse()
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                IdResponsible = a.IdResponsible,
                Responsible = a.IdResponsible != null ? new UsuarioDto
                {
                    Id = a.Responsible.Id,
                    Name = a.Responsible.Name,
                } : null,
                Processos = a.Processos.Select(p => new ProcessoDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    IdParentProccess = p.IdParentProccess,
                }).ToList()
            }).ToList();
        }
    }
}
