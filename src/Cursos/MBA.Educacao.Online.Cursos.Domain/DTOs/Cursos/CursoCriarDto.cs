using System.ComponentModel.DataAnnotations;
using MBA.Educacao.Online.Cursos.Domain.DTOs.Aulas;
using MBA.Educacao.Online.Cursos.Domain.DTOs.ConteudoProgramatico;
using MBA.Educacao.Online.Cursos.Domain.Enums;

namespace MBA.Educacao.Online.Cursos.Domain.DTOs.Cursos
{
    public class CursoCriarDto
    {
        [Required]
        public Guid Id { get; init; }

        [Required, StringLength(200, MinimumLength = 2)]
        public string Titulo { get; init; } = default!;

        [Required, StringLength(1000, MinimumLength = 2)]
        public string Descricao { get; init; } = default!;

        [Required, StringLength(150, MinimumLength = 2)]
        public string Instrutor { get; init; } = default!;

        // Se preferir manter como enum em vez de string, troque o tipo e use [EnumDataType(typeof(NivelCurso))]
        [EnumDataType(typeof(NivelCurso))]
        public string Nivel { get; init; }

        [Required, Range(0.0, 9999999999.99)]
        public decimal Valor { get; init; }
        
        [Required]
        public bool Ativo { get; init; }

        // Owned (opcional): não é Required porque pode não existir em todos os cursos
        public ConteudoProgramaticoDto? ConteudoProgramatico { get; init; }

        // Lista nunca nula
        [Required]
        public IReadOnlyList<AulaDto> Aulas { get; init; } = Array.Empty<AulaDto>();
    }    
}

