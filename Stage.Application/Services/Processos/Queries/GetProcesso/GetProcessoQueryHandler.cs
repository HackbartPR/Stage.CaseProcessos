using MediatR;
using Microsoft.EntityFrameworkCore;
using Stage.Domain.Config;
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

            //return new PagedBaseResponse<ICollection<GetProcessoQueryResponse>>()
            //{
            //    Page = request.Page,
            //    TotalItems = totalItems,
            //    Notifications = _notification.Notifications(),
            //    Result = CreateResponse(resultPaged),
            //    Success = true,
            //    TotalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize)
            //};

            throw new Exception();
        }

        public IQueryable<Processo> QueryProcesso(GetProcessoQuery request)
        {
            if (request.Id != null && request.Id != 0)
                return GetById(request);

            //if (!request.Name.IsNullOrEmpty())
            //    return GetByName(request);

            //if (request.IdsAreas.Any(id => id != 0))
            //    return GetByAreas(request);

            //return GetAll();
            return GetById(request);
        }

        public IQueryable<Processo> GetById(GetProcessoQuery request)
        {
            return _unitOfWork.ProcessoRepository
                .GetEntity()
                .Include(p => p.ParentProcess)
                .Include(p => p.Ferramentas.Where(f => f.Active))
                .Include(p => p.SubProcessos.Where(s => s.Active))
                .Where(p => p.Active && p.Id == request.Id);
        }

        public IQueryable<Processo> GetSubProccessByRecursion(Processo processo)
        {
            return _unitOfWork.ProcessoRepository
                .GetEntity()
                .Include(p => p.Ferramentas.Where(f => f.Active))
                .Include(p => p.SubProcessos.Where(s => s.ParentProcess == processo))
                .Where(p => p.ParentProcess == processo);
        }

        public static async Task<ICollection<Processo>> ToListAsync(IQueryable<Processo> query, GetProcessoQuery request, CancellationToken cancellationToken)
        {
            request.ValidatePageRequest();

            return await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
