using MBA.Educacao.Online.Core.Domain.DTOs;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Core.Domain.Messages.CommonMessages.IntegrationEvents;
using MBA.Educacao.Online.Core.Domain.Notifications;
using MBA.Educacao.Online.Vendas.Application.Commands;
using MBA.Educacao.Online.Vendas.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Vendas.Application.Handlers.Commands
{
    public class AlterarStatusPedidoPagoCommandHandler : IRequestHandler<AlterarStatusPedidoPagoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly INotificador _notificador;

        public AlterarStatusPedidoPagoCommandHandler(
            IPedidoRepository pedidoRepository,
            INotificador notificador)
        {
            _pedidoRepository = pedidoRepository;
            _notificador = notificador;
        }

        public async Task<bool> Handle(AlterarStatusPedidoPagoCommand request, CancellationToken cancellationToken)
        {
            // Valida o comando
            if (!ValidarComando(request)) return false;

            var pedido = await _pedidoRepository.ObterPorIdComTracking(request.PedidoId);
            if (pedido == null)
            {
                Notificar("PedidoId", $"Pedido com ID {request.PedidoId} não encontrado");
                return false;
            }

            try
            {
                pedido.AtualizarStatusPago();
            }
            catch (InvalidOperationException ex)
            {
                Notificar("Status", ex.Message);
                return false;
            }

            _pedidoRepository.Alterar(pedido);

            var listaCursos = new ListaCursosPedidoDto
            {
                PedidoId = pedido.Id,
                Itens = pedido.PedidoItens.Select(i => new Item
                {
                    Id = i.CursoId,
                    Descricao = i.CursoNome.GetHashCode(),
                    Valor = i.Valor
                }).ToList()
            };

            // Cria e publica o evento de matrícula
            var efetuarMatriculaEvent = new EfetuarMatriculaEvent(
                pedido.Id,
                pedido.AlunoId,
                listaCursos
            );

            pedido.AdicionarEvento(efetuarMatriculaEvent);
            var resultado = await _pedidoRepository.UnitOfWork.Commit();

            if (!resultado)
            {
                Notificar("Pedido", "Erro ao alterar status do pedido para rascunho");
                return false;
            }

            return true;
        }

        private bool ValidarComando(Command request)
        {
            if (request.IsValid()) return true;

            foreach (var error in request.ValidationResult.Errors)
            {
                Notificar(error.PropertyName, error.ErrorMessage);
            }

            return false;
        }

        private void Notificar(string campo, string mensagem)
        {
            _notificador.Handle(new Notificacao
            {
                Campo = campo,
                Mensagem = mensagem
            });
        }
    }
}
