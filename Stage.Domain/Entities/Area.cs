using System.ComponentModel.DataAnnotations;

namespace Stage.Domain.Entities
{
    public class Area : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int? IdResponsible { get; set; }

        public virtual Usuario Responsible { get; set; } = null!;

        public virtual ICollection<Processo> Processos { get; set; } = null!;
    }
}
