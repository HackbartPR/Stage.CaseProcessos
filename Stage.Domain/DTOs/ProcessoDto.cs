namespace Stage.Domain.DTOs
{
    public class ProcessoDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int? IdParentProccess { get; set; }
    }
}
