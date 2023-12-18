using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stage.Application.Extensions;
using Stage.Application.Services.Areas.Queries.GetArea;
using Stage.Domain.Config;
using Stage.Domain.Notifications;

namespace Stage.WebAPI.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly INotificationContext _notificationContext;

        public AreaController(INotificationContext notificationContext, IMediator mediator)
        {
            _notificationContext = notificationContext ?? throw new ArgumentNullException(nameof(notificationContext));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("GetAreas")]
        public async Task<IActionResult> GetAreas([FromBody] GetAreaQuery request)
        {
            try
            {
                var result = await _mediator.Send(request);
                return MontarResponse<PagedBaseResponse<ICollection<GetAreaQueryResponse>>>.OkPaged(result);
            }
            catch (Exception ex)
            {
                return MontarResponse<Exception>.Failure(ex.Message, _notificationContext);
            }
        }
    }
}
