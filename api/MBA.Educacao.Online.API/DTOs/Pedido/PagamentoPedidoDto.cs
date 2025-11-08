using System.ComponentModel.DataAnnotations;

namespace MBA.Educacao.Online.API.DTOs.Pedido
{
    public class PagamentoPedidoDto
    {
        [Required(ErrorMessage = "O nome no cartão é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome no cartão deve ter entre 3 e 100 caracteres")]
        public string NomeCartao { get; set; }

        [Required(ErrorMessage = "O número do cartão é obrigatório")]
        [StringLength(19, MinimumLength = 13, ErrorMessage = "O número do cartão deve ter entre 13 e 19 caracteres")]
        [RegularExpression(@"^\d{13,19}$", ErrorMessage = "O número do cartão deve conter apenas dígitos")]
        public string NumeroCartao { get; set; }

        [Required(ErrorMessage = "A data de expiração é obrigatória")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "A data de expiração deve estar no formato MM/YY")]
        public string ExpiracaoCartao { get; set; }

        [Required(ErrorMessage = "O CVV é obrigatório")]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "O CVV deve ter 3 ou 4 dígitos")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "O CVV deve conter apenas dígitos")]
        public string CvvCartao { get; set; }
    }
}

