using System.ComponentModel.DataAnnotations;

namespace MBA.Educacao.Online.API.DTOs
{
    public class AtualizarAulaDto
    {
        [Required(ErrorMessage = "O título da aula é obrigatório")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 200 caracteres")]
        public string Titulo { get; set; } = default!;

        [Required(ErrorMessage = "A descrição da aula é obrigatória")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "A descrição deve ter entre 10 e 1000 caracteres")]
        public string Descricao { get; set; } = default!;

        [Required(ErrorMessage = "A duração em minutos é obrigatória")]
        [Range(1, int.MaxValue, ErrorMessage = "A duração deve ser maior que zero")]
        public int DuracaoMinutos { get; set; }

        [Required(ErrorMessage = "A ordem da aula é obrigatória")]
        [Range(1, int.MaxValue, ErrorMessage = "A ordem deve ser maior que zero")]
        public int Ordem { get; set; }
    }
}

