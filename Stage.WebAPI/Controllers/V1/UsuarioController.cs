﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stage.Application.Extensions;
using Stage.Application.Services.Usuarios.Queries.GetUsuario;
using Stage.Domain.Config;
using Stage.Domain.Notifications;

namespace Stage.WebAPI.V1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly INotificationContext _notificationContext;

        public UsuarioController(IMediator mediator, INotificationContext notificationContext)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _notificationContext = notificationContext ?? throw new ArgumentNullException(nameof(notificationContext));
        }

        [HttpPost("GetUsuarios")]
        public async Task<IActionResult> GetUsers ([FromBody] GetUsuarioQuery request)
        {
            try
            {
                var result = await _mediator.Send(request);
                return MontarResponse<PagedBaseResponse<ICollection<GetUsuarioQueryResponse>>>.OkPaged(result);
            }
            catch (Exception ex)
            {
                return MontarResponse<Exception>.Failure(ex.Message, _notificationContext);
            }
        }
    }
}
