using MBA.Educacao.Online.Cursos.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MBA.Educacao.Online.API.DTOs
{
    public class AtualizarCursoDto
    {
        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 200 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "A descrição deve ter entre 10 e 1000 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O nível do curso é obrigatório")]
        public NivelCurso Nivel { get; set; }

        [Required(ErrorMessage = "A chave Conteúdo Programático é obrigatória")]
        public ConteudoProgramaticoDto ConteudoProgramatico { get; set; }
    }
}

