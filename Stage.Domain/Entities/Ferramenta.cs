using System.ComponentModel.DataAnnotations;

namespace Stage.Domain.Entities
{
    public class Ferramenta : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public virtual ICollection<Processo> Processos { get; set; } = new HashSet<Processo>();
    }
}
