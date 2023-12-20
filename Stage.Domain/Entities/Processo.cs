using System.ComponentModel.DataAnnotations;

namespace Stage.Domain.Entities
{
    public class Processo : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int? IdParentProccess { get; set; }

        public virtual Processo? ParentProcess { get; set; }

        public virtual ICollection<Processo> SubProcessos { get; set; } = new HashSet<Processo>();

        public virtual ICollection<Ferramenta> Ferramentas { get; set; } = new HashSet<Ferramenta>();

        public virtual ICollection<Area> Areas { get; set; } = new HashSet<Area>();
    }
}
