using Stage.Application.Services.Areas.Queries.GetArea;
using Stage.Domain.Config;
using Stage.Domain.DTOs;

namespace Stage.ApplicationUT.Fixtures.Controller
{
    public static class GetAreas
    {
        public static GetAreaQuery GetAreaQueryPayload()
        {
            return new GetAreaQuery
            {
                Id = 1,
                Name = "Tecnologia",
                IdResponsible = 1,
                IdsProcessos = new List<int> { 1, 2},
                Page = 1,
                PageSize = 10,
            };
        }

        public static GetAreaQueryResponse GetAreaQueryResponse()
        {
            return new GetAreaQueryResponse
            {
                Id = 1,
                Name = "Teste Name",
                Description = "Teste Description",
                IdResponsible = 1,
                Processos = new List<ProcessoDto> { },
                Responsible = new UsuarioDto()
            };
        }

        public static PagedBaseResponse<ICollection<GetAreaQueryResponse>> GetAreaQueryResponsePaged()
        {
            return new PagedBaseResponse<ICollection<GetAreaQueryResponse>>
            {
                Notifications = new Dictionary<string, string>(),
                Result = new List<GetAreaQueryResponse>(),
                Success = true,
                TotalItems = 1,
                Page = 1,
                TotalPages = 1,
            };
        }
    }
}
