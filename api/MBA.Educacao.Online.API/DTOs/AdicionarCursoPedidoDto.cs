using System.ComponentModel.DataAnnotations;

namespace MBA.Educacao.Online.API.DTOs
{
    public class AdicionarCursoPedidoDto
    {
        [Required(ErrorMessage = "O ID do curso é obrigatório")]
        public Guid CursoId { get; set; }
    }
}

