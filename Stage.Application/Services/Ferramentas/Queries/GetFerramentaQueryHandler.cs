using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stage.Domain.Config;
using Stage.Domain.DTOs;
using Stage.Domain.Entities;
using Stage.Domain.Notifications;
using Stage.Domain.UnitsOfWork;

namespace Stage.Application.Services.Ferramentas.Queries
{
    public class GetFerramentaQueryHandler : IRequestHandler<GetFerramentaQuery, PagedBaseResponse<ICollection<GetFerramentaQueryResponse>>>
    {
        private readonly INotificationContext _notification;
        private readonly IUnitOfWork _unitOfWork;

        public GetFerramentaQueryHandler(IUnitOfWork unitOfWork, INotificationContext notification)
        {
            _unitOfWork = unitOfWork;
            _notification = notification;
        }

        public async Task<PagedBaseResponse<ICollection<GetFerramentaQueryResponse>>> Handle(GetFerramentaQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Ferramenta> query = QueryFerramenta(request);

            int totalItems = await query.CountAsync(cancellationToken);
            ICollection<Ferramenta> resultPaged = await ToListAsync(query, request, cancellationToken);

            return new PagedBaseResponse<ICollection<GetFerramentaQueryResponse>>()
            {
                Page = request.Page,
                TotalItems = totalItems,
                Notifications = _notification.Notifications(),
                Result = CreateResponse(resultPaged),
                Success = true,
                TotalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize)
            };
        }

        public IQueryable<Ferramenta> QueryFerramenta(GetFerramentaQuery request)
        {
            if (request.Id != null && request.Id != 0)
                return GetById(request);

            if (!request.Name.IsNullOrEmpty())
                return GetByName(request);

            if (request.IdsProcessos.Any(id => id != 0))
                return GetByProcessos(request);

            return GetAll();
        }

        public IQueryable<Ferramenta> GetById(GetFerramentaQuery request)
        {
            return _unitOfWork.FerramentaRepository
                    .GetEntity()
                    .Include(a => a.Processos.Where(p => p.Active))
                    .Where(a => a.Active && a.Id == request.Id);
        }

        public IQueryable<Ferramenta> GetByName(GetFerramentaQuery request)
        {
            return _unitOfWork.FerramentaRepository
                .GetEntity()
                .Include(a => a.Processos.Where(p => p.Active))
                .Where(u => u.Active && u.Name.Contains(request.Name!));
        }

        public IQueryable<Ferramenta> GetByProcessos(GetFerramentaQuery request)
        {
            return _unitOfWork.FerramentaRepository
                .GetEntity()
                .Include(a => a.Processos.Where(p => p.Active))
                .Where(a => a.Active && request.IdsProcessos.Contains(a.Id));
        }

        public IQueryable<Ferramenta> GetAll()
        {
            return _unitOfWork.FerramentaRepository
                .GetEntity()
                .Include(a => a.Processos.Where(p => p.Active))
                .Where(a => a.Active);
        }

        public static async Task<ICollection<Ferramenta>> ToListAsync(IQueryable<Ferramenta> query, GetFerramentaQuery request, CancellationToken cancellationToken)
        {
            request.ValidatePageRequest();

            return await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
        }

        public static ICollection<GetFerramentaQueryResponse> CreateResponse(ICollection<Ferramenta> usuarios)
        {
            return usuarios.Select(a => new GetFerramentaQueryResponse()
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
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
