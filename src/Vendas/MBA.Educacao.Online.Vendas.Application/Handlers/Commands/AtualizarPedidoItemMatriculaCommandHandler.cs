using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Core.Domain.Notifications;
using MBA.Educacao.Online.Vendas.Application.Commands;
using MBA.Educacao.Online.Vendas.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Vendas.Application.Handlers.Commands
{
    public class AtualizarPedidoItemMatriculaCommandHandler : IRequestHandler<AtualizarPedidoItemMatriculaCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly INotificador _notificador;

        public AtualizarPedidoItemMatriculaCommandHandler(
            IPedidoRepository pedidoRepository,
            INotificador notificador)
        {
            _pedidoRepository = pedidoRepository;
            _notificador = notificador;
        }

        public async Task<bool> Handle(AtualizarPedidoItemMatriculaCommand request, CancellationToken cancellationToken)
        {
            // Valida o comando
            if (!ValidarComando(request)) return false;

            var pedido = await _pedidoRepository.ObterPorIdComTracking(request.PedidoId);
            if (pedido == null)
            {
                Notificar("PedidoId", $"Pedido com ID {request.PedidoId} não encontrado");
                return false;
            }

            var itensAtualizados = 0;
            var itensComErro = 0;
            foreach (var matriculaItem in request.Matriculas)
            {
                try
                {
                    // Busca o item específico dentro do pedido pelo CursoId
                    var pedidoItem = pedido.PedidoItens.FirstOrDefault(pi => pi.CursoId == matriculaItem.CursoId);
                    if (pedidoItem == null)
                    {
                        Notificar("CursoId", $"Item do pedido com Curso ID {matriculaItem.CursoId} não encontrado");
                        itensComErro++;
                        continue;
                    }

                    // Atualiza o PedidoItem com a MatriculaId gerada
                    pedidoItem.AtribuirMatricula(matriculaItem.MatriculaId);
                    itensAtualizados++;
                }
                catch (InvalidOperationException ex)
                {
                    Notificar("MatriculaId", $"Erro ao atribuir matrícula {matriculaItem.MatriculaId} ao curso {matriculaItem.CursoId}: {ex.Message}");
                    itensComErro++;
                }
            }

            // Verifica se pelo menos um item foi atualizado
            if (itensAtualizados == 0)
            {
                Notificar("PedidoItem", "Nenhum item do pedido foi atualizado com sucesso");
                return false;
            }

            _pedidoRepository.Alterar(pedido);
            var resultado = await _pedidoRepository.UnitOfWork.Commit();
            if (!resultado)
            {
                Notificar("PedidoItem", "Erro ao atualizar os itens do pedido com as matrículas");
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
