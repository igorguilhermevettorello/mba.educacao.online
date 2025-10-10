using System.ComponentModel.DataAnnotations;

namespace MBA.Educacao.Online.Cursos.Domain.DTOs.Aulas
{
    public class AulaDto
    {
        [Required]
        public Guid Id { get; init; }

        [Required, StringLength(200, MinimumLength = 2)]
        public string Titulo { get; init; } = default!;

        [Required, StringLength(1000, MinimumLength = 2)]
        public string Descricao { get; init; } = default!;

        [Required, Range(1, int.MaxValue, ErrorMessage = "Duração deve ser >= 1 minuto.")]
        public int DuracaoMinutos { get; init; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "Ordem deve ser >= 1.")]
        public int Ordem { get; init; }

        [Required]
        public DateTime DataCriacao { get; init; }

        [Required]
        public bool Ativa { get; init; }
    }
}