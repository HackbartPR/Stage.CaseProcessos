using System.ComponentModel.DataAnnotations;

namespace Stage.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<Area> Areas { get; set; } = new HashSet<Area>();
    }
}
