using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stage.Domain.Entities;
using Stage.Domain.UnitsOfWork;
using Stage.Infrastructure.Seeders;

namespace Stage.Application.Startup.Seeders
{
    public class InitialDataSeed : BackgroundService
    {
        private readonly ILogger<InitialDataSeed> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public InitialDataSeed(ILogger<InitialDataSeed> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Verificando necessidade de alimentação da base de dados");
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                if (await unitOfWork.UsuarioRepository.GetEntity().FirstOrDefaultAsync(stoppingToken) == null)
                {
                    _logger.LogInformation("Alimentando base de dados.");

                    var ferramentaSeeders = await CreateFerramentas(unitOfWork, stoppingToken);
                    var usuarioSeeders = await CreateUsuarios(unitOfWork, stoppingToken);
                    var processoSeeders = await CreateProcessos(unitOfWork, stoppingToken);
                    var areasSeeders = await CreateAreas(unitOfWork, stoppingToken);

                    SetUsuariosAreas(usuarioSeeders, areasSeeders);
                    SetAreasProcessos(areasSeeders, processoSeeders);
                    SetProcessosFerramentas(processoSeeders, ferramentaSeeders);

                    await unitOfWork.CommitAsync(stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "A inicialização da aplicação falhou.");
            }
        }

        public static async Task<IEnumerable<Ferramenta>> CreateFerramentas(IUnitOfWork unitOfWork, CancellationToken cancellationToken)
        {
            var ferramentaSeeders = Seed.Ferramentas();
            foreach (var entity in ferramentaSeeders)
            {
                await unitOfWork.FerramentaRepository.CreateAsync(entity, cancellationToken: cancellationToken);
            }

            return ferramentaSeeders;
        }

        public static async Task<IEnumerable<Usuario>> CreateUsuarios(IUnitOfWork unitOfWork, CancellationToken cancellationToken)
        {
            var usuarioSeeders = Seed.Usuarios();
            foreach (var entity in usuarioSeeders)
            {
                await unitOfWork.UsuarioRepository.CreateAsync(entity, cancellationToken: cancellationToken);
            }

            return usuarioSeeders;
        }

        public static async Task<IEnumerable<Processo>> CreateProcessos(IUnitOfWork unitOfWork, CancellationToken cancellationToken)
        {
            var processoSeeders = Seed.Processos();
            foreach (var entity in processoSeeders)
            {
                await unitOfWork.ProcessoRepository.CreateAsync(entity, cancellationToken: cancellationToken);
            }

            return processoSeeders;
        }

        public static async Task<IEnumerable<Area>> CreateAreas(IUnitOfWork unitOfWork, CancellationToken cancellationToken)
        {
            var areasSeeders = Seed.Areas();
            foreach (var entity in areasSeeders)
            {
                await unitOfWork.AreaRepository.CreateAsync(entity, cancellationToken: cancellationToken);
            }

            return areasSeeders;
        }

        public static void SetUsuariosAreas(IEnumerable<Usuario> usuarios, IEnumerable<Area> areas)
        {
            var ceo = usuarios.First(u => u.Name.Equals("CEO"));
            ceo.Areas = areas.Where(a => a.Name.Equals("Recursos Humanos")).ToList();

            var cto = usuarios.First(u => u.Name.Equals("CTO"));
            cto.Areas = areas.Where(a => a.Name.Equals("Tecnologia")).ToList();
        }

        public static void SetAreasProcessos(IEnumerable<Area> areas, IEnumerable<Processo> processos)
        {
            var tecnologia = areas.First(u => u.Name.Equals("Tecnologia"));
            tecnologia.Processos = processos.ToList();
        }

        public static void SetProcessosFerramentas(IEnumerable<Processo> processos, IEnumerable<Ferramenta> ferramentas)
        {
            var manual = ferramentas.First(f => f.Name.Equals("Manual"));
            var office = ferramentas.First(f => f.Name.Equals("Microsoft Office"));

            var orcamento = processos.First(p => p.Name.Equals("Orçamento"));
            orcamento.Ferramentas.Add(manual);
            orcamento.Ferramentas.Add(office);

            var analise = processos.First(p => p.Name.Equals("Análise de Requisitos"));
            analise.Ferramentas.Add(manual);
            analise.Ferramentas.Add(office);
        }
    }
}
