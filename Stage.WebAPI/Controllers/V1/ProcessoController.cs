using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stage.Application.Extensions;
using Stage.Application.Services.Ferramentas.Queries;
using Stage.Application.Services.Processos.Queries.GetProcesso;
using Stage.Domain.Config;
using Stage.Domain.Notifications;

namespace Stage.WebAPI.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessoController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly INotificationContext _notificationContext;

        public ProcessoController(INotificationContext notificationContext, IMediator mediator)
        {
            _notificationContext = notificationContext ?? throw new ArgumentNullException(nameof(notificationContext));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("GetProcessos")]
        public async Task<IActionResult> GetProcessos([FromBody] GetProcessoQuery request)
        {
            try
            {
                var result = await _mediator.Send(request);
                return MontarResponse<PagedBaseResponse<ICollection<GetProcessoQueryResponse>>>.OkPaged(result);
            }
            catch (Exception ex)
            {
                return MontarResponse<Exception>.Failure(ex.Message, _notificationContext);
            }
        }
    }
}
