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
            if (request.Id != null && request.Id != 0)
                return GetById(request);

            if (!request.Name.IsNullOrEmpty())
                return GetByName(request);

            if (request.IdResponsible != null &&  request.IdResponsible != 0)
                return GetByResponsible(request);

            if (request.IdsProcessos.Any(id => id != 0))
                return GetByProcessos(request);

            return GetAll();
        }

        public IQueryable<Area> GetById(GetAreaQuery request)
        {
            return _unitOfWork.AreaRepository
                    .GetEntity()
                    .Include(a => a.Responsible)
                    .Include(a => a.Processos.Where(p => p.Active))
                    .Where(a => a.Active && a.Id == request.Id);
        }

        public IQueryable<Area> GetByName(GetAreaQuery request)
        {
            return _unitOfWork.AreaRepository
                .GetEntity()
                .Include(a => a.Responsible)
                .Include(a => a.Processos.Where(p => p.Active))
                .Where(u => u.Active && u.Name.Contains(request.Name!));
        }

        public IQueryable<Area> GetByResponsible(GetAreaQuery request)
        {
            return _unitOfWork.AreaRepository
                .GetEntity()
                .Include(a => a.Responsible)
                .Include(a => a.Processos.Where(p => p.Active))
                .Where(u => u.Active && u.IdResponsible != null && u.IdResponsible == request.IdResponsible);
        }

        public IQueryable<Area> GetByProcessos(GetAreaQuery request)
        {
            return _unitOfWork.AreaRepository
                .GetEntity()
                .Include(a => a.Responsible)
                .Include(a => a.Processos.Where(p => p.Active))
                .Where(a => a.Active && request.IdsProcessos.Contains(a.Id));
        }

        public IQueryable<Area> GetAll()
        {
            return _unitOfWork.AreaRepository
                .GetEntity()
                .Include(a => a.Responsible)
                .Include(a => a.Processos.Where(p => p.Active))
                .Where(a => a.Active);
        }

        public static async Task<ICollection<Area>> ToListAsync(IQueryable<Area> query, GetAreaQuery request, CancellationToken cancellationToken)
        {
            request.ValidatePageRequest();

            return await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
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
