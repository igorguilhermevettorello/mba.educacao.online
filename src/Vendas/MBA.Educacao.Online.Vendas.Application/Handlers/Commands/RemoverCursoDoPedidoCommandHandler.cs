using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Core.Domain.Notifications;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Vendas.Application.Commands;
using MBA.Educacao.Online.Vendas.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Vendas.Application.Handlers.Commands
{
    public class RemoverCursoDoPedidoCommandHandler : IRequestHandler<RemoverCursoDoPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ICursoRepository _cursoRepository;
        private readonly INotificador _notificador;

        public RemoverCursoDoPedidoCommandHandler(
            IPedidoRepository pedidoRepository,
            ICursoRepository cursoRepository,
            INotificador notificador)
        {
            _pedidoRepository = pedidoRepository;
            _cursoRepository = cursoRepository;
            _notificador = notificador;
        }

        public async Task<bool> Handle(RemoverCursoDoPedidoCommand request, CancellationToken cancellationToken)
        {
            // Valida o comando
            if (!ValidarComando(request)) return false;

            // Verifica se o curso existe
            var curso = await _cursoRepository.BuscarPorIdAsync(request.CursoId);
            if (curso == null)
            {
                Notificar("CursoId", $"Curso com ID {request.CursoId} não encontrado");
                return false;
            }

            // Busca o pedido rascunho do aluno
            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorAlunoId(request.AlunoId);
            if (pedido == null)
            {
                Notificar("Pedido", "Pedido não encontrado para este aluno");
                return false;
            }

            if (!pedido.PossuiItens())
            {
                Notificar("Pedido", "Pedido não possui itens para remover");
                return false;
            }

            // Verifica se o curso está no pedido
            var pedidoItem = pedido.PedidoItens.FirstOrDefault(pi => pi.CursoId == request.CursoId);
            if (pedidoItem == null)
            {
                Notificar("CursoId", $"Curso '{curso.Titulo}' não está no pedido");
                return false;
            }

            // Remove o item do pedido
            try
            {
                pedido.RemoverItem(pedidoItem);
                _pedidoRepository.RemoverItem(pedidoItem);
                _pedidoRepository.Alterar(pedido);
            }
            catch (InvalidOperationException ex)
            {
                Notificar("Pedido", ex.Message);
                return false;
            }

            // Salva as alterações
            var resultado = await _pedidoRepository.UnitOfWork.Commit();
            if (!resultado)
            {
                Notificar("Pedido", "Erro ao remover curso do pedido");
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

