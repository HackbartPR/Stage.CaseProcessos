namespace Stage.Domain.DTOs
{
    public class AreaDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int? IdResponsible { get; set; }
    }
}
