using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Vendas.Application.Commands;
using MBA.Educacao.Online.Vendas.Domain.Entities;
using MBA.Educacao.Online.Vendas.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Vendas.Application.Handlers
{
    public class AdicionarCursoAoPedidoCommandHandler : IRequestHandler<AdicionarCursoAoPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ICursoRepository _cursoRepository;
        private readonly INotificador _notificador;

        public AdicionarCursoAoPedidoCommandHandler(
            IPedidoRepository pedidoRepository,
            ICursoRepository cursoRepository,
            INotificador notificador)
        {
            _pedidoRepository = pedidoRepository;
            _cursoRepository = cursoRepository;
            _notificador = notificador;
        }

        public async Task<bool> Handle(AdicionarCursoAoPedidoCommand request, CancellationToken cancellationToken)
        {
            // Valida o comando
            if (!ValidarComando(request)) return false;

            // Busca ou cria um pedido em rascunho para o aluno
            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorAlunoId(request.AlunoId);
            var pedidoExistente = pedido != null;

            if (pedido == null)
            {
                pedido = Pedido.PedidoFactory.CriarPedido(request.AlunoId);
                pedido.DefinirDataCadastro(DateTime.UtcNow);
                _pedidoRepository.Adicionar(pedido);
            }

            // Busca o curso
            var curso = await _cursoRepository.BuscarPorIdAsync(request.CursoId);
            if (curso == null)
            {
                _notificador.Handle(new Core.Domain.Notifications.Notificacao
                {
                    Campo = "CursoId",
                    Mensagem = $"Curso com ID {request.CursoId} não encontrado"
                });
                return false;
            }

            if (!curso.Ativo)
            {
                _notificador.Handle(new Core.Domain.Notifications.Notificacao
                {
                    Campo = "CursoId",
                    Mensagem = $"Curso '{curso.Titulo}' não está ativo"
                });
                return false;
            }

            // Cria o item do pedido
            var pedidoItem = new PedidoItem(
                curso.Id,
                curso.Titulo,
                curso.Valor
            );

            // Tenta adicionar o item ao pedido (inclui verificação de curso duplicado)
            try
            {
                pedido.AdicionarItem(pedidoItem);
                
                // IMPORTANTE: Quando o pedido já existe, precisamos informar ao EF
                // que ele foi modificado e adicionar o novo item ao contexto
                if (pedidoExistente)
                {
                    _pedidoRepository.AdicionarItem(pedidoItem);
                    _pedidoRepository.Alterar(pedido);
                }
            }
            catch (InvalidOperationException ex)
            {
                _notificador.Handle(new Core.Domain.Notifications.Notificacao
                {
                    Campo = "CursoId",
                    Mensagem = ex.Message
                });
                return false;
            }

            // Salva as alterações
            try
            {
                var resultado = await _pedidoRepository.UnitOfWork.Commit();

                if (!resultado)
                {
                    _notificador.Handle(new Core.Domain.Notifications.Notificacao
                    {
                        Campo = "Pedido",
                        Mensagem = "Erro ao adicionar curso ao pedido"
                    });
                    return false;
                }
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Core.Domain.Notifications.Notificacao
                {
                    Campo = "Pedido",
                    Mensagem = $"Erro ao salvar: {ex.Message}"
                });
                return false;
            }

            // Atualiza o PedidoId no comando para retornar ao controller
            request.PedidoId = pedido.Id;

            return true;
        }

        private bool ValidarComando(Command request)
        {
            if (request.IsValid()) return true;

            foreach (var error in request.ValidationResult.Errors)
            {
                _notificador.Handle(new Core.Domain.Notifications.Notificacao
                {
                    Campo = error.PropertyName,
                    Mensagem = error.ErrorMessage
                });
            }

            return false;
        }
    }
}

