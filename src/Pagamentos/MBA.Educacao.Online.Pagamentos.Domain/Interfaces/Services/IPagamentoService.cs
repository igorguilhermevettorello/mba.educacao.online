using MBA.Educacao.Online.Core.Domain.DTOs;
using MBA.Educacao.Online.Pagamentos.Domain.Entities;

namespace MBA.Educacao.Online.Pagamentos.Domain.Interfaces.Services
{
    public interface IPagamentoService
    {
        Task<Transacao> RealizarPagamentoPedido(PagamentoPedido pagamentoPedido);
    }
}