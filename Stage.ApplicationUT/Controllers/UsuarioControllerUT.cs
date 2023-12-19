using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Stage.Application.Extensions;
using Stage.Application.Services.Areas.Queries.GetArea;
using Stage.ApplicationUT.Fixtures.Controller;
using Stage.Domain.Config;
using Stage.Domain.Notifications;
using Stage.WebAPI.Controllers.V1;

namespace Stage.ApplicationUT.Controllers
{
    public class UsuarioControllerUT
    {
        private readonly AreaController _areaController; 

        private readonly Mock<IMediator> _mediator;
        private readonly Mock<INotificationContext> _notificationContext;

        public UsuarioControllerUT()
        {
            _mediator = new Mock<IMediator>();
            _notificationContext = new Mock<INotificationContext>();

            _areaController = new AreaController(_notificationContext.Object, _mediator.Object);
        }

        [Fact]
        public async Task GetAreas_OnSuccess_Should_Call_MediatR()
        {
            // Arrange
            var payload = GetAreas.GetAreaQueryPayload();
            var expected = GetAreas.GetAreaQueryResponsePaged();

            _mediator.Setup(m => m.Send(It.IsAny<GetAreaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);
            
            // Act
            await _areaController.GetAreas(payload);

            // Assert
            _mediator.Verify(x => x.Send(It.IsAny<GetAreaQuery>(), default), Times.Once());
        }

        [Fact]
        public async Task GetAreas_OnSuccess_Should_Return_Ok()
        {
            // Arrange
            var payload = GetAreas.GetAreaQueryPayload();

            var response = GetAreas.GetAreaQueryResponse();
            response.Id = (int)payload.Id!;
            response.IdResponsible = payload.IdResponsible;

            var responsePaged = GetAreas.GetAreaQueryResponsePaged();
            responsePaged.Result.Add(response);

            var expected = (ObjectResult) MontarResponse<PagedBaseResponse<ICollection<GetAreaQueryResponse>>>.OkPaged(responsePaged);

            _mediator.Setup(m => m.Send(It.IsAny<GetAreaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(responsePaged);

            // Act
            var result = (ObjectResult) await _areaController.GetAreas(payload);

            // Assert
            result.Value.Should().BeEquivalentTo(expected.Value);
            result.StatusCode.Should().Be(expected.StatusCode);
        }

        [Fact]
        public async Task GetAreas_OnFailed_Should_Return_FailureResponse()
        {
            // Arrange
            var payload = GetAreas.GetAreaQueryPayload();
            var exception = new Exception("teste");
            var expected = (ObjectResult) MontarResponse<Exception>.Failure(exception.Message, _notificationContext.Object);

            _mediator.Setup(m => m.Send(It.IsAny<GetAreaQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            // Act
            var result = (ObjectResult) await _areaController.GetAreas(payload);

            // Assert
            result.Value.Should().BeEquivalentTo(expected.Value);
            result.StatusCode.Should().Be(expected.StatusCode);
        }
    }
}
