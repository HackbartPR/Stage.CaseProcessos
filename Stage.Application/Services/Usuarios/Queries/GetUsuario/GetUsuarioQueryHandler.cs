using MediatR;
using Stage.Domain.UnitsOfWork;
using Stage.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Stage.Domain.Config;
using Stage.Domain.Notifications;
using Microsoft.IdentityModel.Tokens;
using Stage.Domain.DTOs;

namespace Stage.Application.Services.Usuarios.Queries.GetUsuario
{
    public class GetUsuarioQueryHandler : IRequestHandler<GetUsuarioQuery, PagedBaseResponse<ICollection<GetUsuarioQueryResponse>>>
    {
        private readonly INotificationContext _notification;
        private readonly IUnitOfWork _unitOfWork;

        public GetUsuarioQueryHandler(IUnitOfWork unitOfWork, INotificationContext notification)
        {
            _unitOfWork = unitOfWork;
            _notification = notification;
        }

        public async Task<PagedBaseResponse<ICollection<GetUsuarioQueryResponse>>> Handle(GetUsuarioQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Usuario> query = QueryUsuario(request);

            int totalItems = await query.CountAsync(cancellationToken);
            ICollection<Usuario> resultPaged = await ToListAsync(query, request, cancellationToken);

            return new PagedBaseResponse<ICollection<GetUsuarioQueryResponse>>()
            {
                Page = request.Page,
                TotalItems = totalItems,
                Notifications = _notification.Notifications(),
                Result = CreateResponse(resultPaged),
                Success = true,
                TotalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize)
            };
        }

        public IQueryable<Usuario> QueryUsuario(GetUsuarioQuery request)
        {
            if (request.Id != null && request.Id != 0)
                return GetById(request);

            if (!request.Name.IsNullOrEmpty())
                return GetByName(request);

            if (request.IdsAreas.Any(id => id != 0))
                return GetByAreas(request);

            return GetAll();
        }

        public IQueryable<Usuario> GetById(GetUsuarioQuery request)
        {
            return _unitOfWork.UsuarioRepository
                    .GetEntity()
                    .Include(u => u.Areas.Where(a => a.Active))
                    .Where(u => u.Active && u.Id == request.Id);
        }

        public IQueryable<Usuario> GetByAreas(GetUsuarioQuery request)
        {
            return _unitOfWork.UsuarioRepository
                .GetEntity()
                .Include(u => u.Areas.Where(a => a.Active))
                .Where(u => u.Active && request.IdsAreas.Contains(u.Id));
        }

        public IQueryable<Usuario> GetByName(GetUsuarioQuery request)
        {
            return _unitOfWork.UsuarioRepository
                .GetEntity()
                .Include(u => u.Areas.Where(a => a.Active))
                .Where(u => u.Active && u.Name.Contains(request.Name!));
        }

        public IQueryable<Usuario> GetAll()
        {
            return _unitOfWork.UsuarioRepository
                .GetEntity()
                .Include(u => u.Areas.Where(a => a.Active))
                .Where(u => u.Active);
        }

        public static async Task<ICollection<Usuario>> ToListAsync(IQueryable<Usuario> query, GetUsuarioQuery request, CancellationToken cancellationToken)
        {
            request.ValidatePageRequest();

            return await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
        }

        public static ICollection<GetUsuarioQueryResponse> CreateResponse(ICollection<Usuario> usuarios)
        {
            return usuarios.Select(u => new GetUsuarioQueryResponse()
            {
                Id = u.Id,
                Name = u.Name,
                Areas = u.Areas.Select(a => new AreaDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    IdResponsible = a.IdResponsible
                }).ToList()
            }).ToList();
        }
    }
}
