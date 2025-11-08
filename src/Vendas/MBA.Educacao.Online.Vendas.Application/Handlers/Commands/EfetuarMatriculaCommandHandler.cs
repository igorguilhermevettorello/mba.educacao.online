using MBA.Educacao.Online.Core.Domain.DTOs;
using MBA.Educacao.Online.Core.Domain.Interfaces.Mediator;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Core.Domain.Messages.CommonMessages.IntegrationEvents;
using MBA.Educacao.Online.Core.Domain.Notifications;
using MBA.Educacao.Online.Vendas.Application.Commands;
using MBA.Educacao.Online.Vendas.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Vendas.Application.Handlers.Commands
{
    public class EfetuarMatriculaCommandHandler : IRequestHandler<EfetuarMatriculaCommand, bool>
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly INotificador _notificador;
        private readonly IPedidoRepository _pedidoRepository;

        public EfetuarMatriculaCommandHandler(
            IMediatorHandler mediatorHandler,
            INotificador notificador,
            IPedidoRepository pedidoRepository)
        {
            _mediatorHandler = mediatorHandler;
            _notificador = notificador;
            _pedidoRepository = pedidoRepository;
        }

        public async Task<bool> Handle(EfetuarMatriculaCommand request, CancellationToken cancellationToken)
        {
            if (!ValidarComando(request)) return false;

            // Busca o pedido pago do aluno
            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorAlunoId(request.AlunoId);

            if (pedido == null)
            {
                Notificar("Pedido", "Pedido não encontrado para este aluno");
                return false;
            }

            if (!pedido.PossuiItens())
            {
                Notificar("Pedido", "Pedido não possui itens para matrícula");
                return false;
            }

            // Prepara a lista de cursos para matrícula
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
                request.AlunoId,
                listaCursos);

            await _mediatorHandler.PublicarEvento(efetuarMatriculaEvent);

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

        private void Notificar(string campo, string mensage)
        {
            _notificador.Handle(new Notificacao
            {
                Campo = campo,
                Mensagem = mensage
            });
        }
    }
}