using Stage.Domain.Entities;

namespace Stage.Infrastructure.Seeders
{
    public static class Seed
    {
        public static IEnumerable<Usuario> Usuarios()
        {
            return new List<Usuario>
            {
                new Usuario
                {
                    //Id = 1,
                    Name = "CEO",
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                },
                new Usuario
                {
                    //Id = 2,
                    Name = "CTO",
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                },
                new Usuario
                {
                    //Id = 3,
                    Name = "Administrador Sistema",
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                },
            };
        }

        public static IEnumerable<Area> Areas() 
        {
            return new List<Area>
            {
                new Area
                {
                    //Id = 1,
                    Name = "Recursos Humanos",
                    Description = "Responsável por cuidar da contratação, integração e satisfação do profissional dentro do ambiente de trabalho.",
                    //IdResponsible = 1,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                },
                new Area
                {
                    //Id = 2,
                    Name = "Tecnologia",
                    Description = "Responsável pelo desenvolvimento do produto digital da empresa.",
                    //IdResponsible = 2,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                },
            };
        }

        public static IEnumerable<Ferramenta> Ferramentas()
        {
            return new List<Ferramenta>
            {
                new Ferramenta
                {
                    //Id = 1,
                    Name = "Manual",
                    Description = "Processo realizado manualmente.",
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                },
                new Ferramenta
                {
                    //Id = 2,
                    Name = "Microsoft Office",
                    Description = "Pacote de ferramentas utilizadas para a criação de manuais, normas entre outros processos.",
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                }
            };
        }
        
        public static IEnumerable<Processo> Processos()
        {
            return new List<Processo>
            {
                new Processo
                {
                    //Id = 1,
                    Name = "Orçamento",
                    Description = "",
                    IdParentProccess = null,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                },
                new Processo
                {
                    //Id = 2,
                    Name = "Análise de Requisitos",
                    Description = "",
                    IdParentProccess = null,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                },
                new Processo
                {
                    //Id = 3,
                    Name = "Desenvolvimento",
                    Description = "",
                    IdParentProccess = null,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    SubProcessos = new List<Processo>
                    {
                        new Processo
                        {
                            //Id = 4,
                            Name = "Arquitetura",
                            Description = "",
                            //IdParentProccess = 3,
                            Active = true,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = null,
                            SubProcessos = new List<Processo>
                            {
                                new Processo
                                {
                                    //Id = 5,
                                    Name = "Estudo da Arquitetura",
                                    Description = "",
                                    //IdParentProccess = 4,
                                    Active = true,
                                    CreatedAt = DateTime.UtcNow,
                                    UpdatedAt = null,
                                },
                                new Processo
                                {
                                    //Id = 6,
                                    Name = "Desenvolvimento da Arquitetura",
                                    Description = "",
                                    //IdParentProccess = 4,
                                    Active = true,
                                    CreatedAt = DateTime.UtcNow,
                                    UpdatedAt = null,
                                }
                            }
                        },
                        new Processo
                        {
                            //Id = 7,
                            Name = "Teste Automatizados",
                            Description = "",
                            //IdParentProccess = 3,
                            Active = true,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = null,
                        }
                    }
                }
            };
        }
    }
}
