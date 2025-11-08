using MBA.Educacao.Online.API.Controllers.Base;
using MBA.Educacao.Online.API.DTOs;
using MBA.Educacao.Online.Core.Application.DTOs;
using MBA.Educacao.Online.Core.Domain.Enums;
using MBA.Educacao.Online.Core.Domain.Interfaces.Identity;
using MBA.Educacao.Online.Core.Domain.Interfaces.Mediator;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Vendas.Application.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MBA.Educacao.Online.API.Controllers
{
    [Authorize(Roles = nameof(TipoUsuario.Administrador) + "," + nameof(TipoUsuario.Aluno))]
    [ApiController]
    [Route("api/pedidos")]
    public class PedidosController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public PedidosController(IMediatorHandler mediatorHandler, INotificador notificador, IUser appUser)
            : base(notificador, appUser)
        {
            _mediatorHandler = mediatorHandler;
        }

        [Authorize(Roles = nameof(TipoUsuario.Aluno))]
        [HttpPost("adicionar-curso")]
        [ProducesResponseType(typeof(ResultDto<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> AdicionarCursoAoPedido([FromBody] AdicionarCursoPedidoDto adicionarCursoDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (!UsuarioAutenticado)
            {
                NotificarErro("Usuario", "Usuário não autenticado");
                return Unauthorized();
            }

            // Obtém o ID do aluno do JWT
            var alunoId = UsuarioId;

            var command = new AdicionarCursoAoPedidoCommand(
                alunoId,
                adicionarCursoDto.CursoId
            );

            var resultado = await _mediatorHandler.EnviarComando(command);

            if (!resultado)
                return CustomResponse();

            var response = ResultDto.Ok(command.PedidoId, "Curso adicionado ao pedido com sucesso.");
            return CustomResponse(response);
        }

        [Authorize(Roles = nameof(TipoUsuario.Aluno))]
        [HttpPost("pagamento")]
        [ProducesResponseType(typeof(ResultDto<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ProcessarPagamentoPedido([FromBody] PagamentoPedidoDto pagamentoPedidoDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (!UsuarioAutenticado)
            {
                NotificarErro("Usuario", "Usuário não autenticado");
                return Unauthorized();
            }

            // Obtém o ID do aluno do JWT
            var alunoId = UsuarioId;

            var command = new ProcessarPagamentoPedidoCommand(
                alunoId,
                pagamentoPedidoDto.NomeCartao,
                pagamentoPedidoDto.NumeroCartao,
                pagamentoPedidoDto.ExpiracaoCartao,
                pagamentoPedidoDto.CvvCartao
            );

            var resultado = await _mediatorHandler.EnviarComando(command);

            if (!resultado)
                return CustomResponse();

            var response = ResultDto.Ok(command.PedidoId, "Pagamento processado com sucesso. Aguarde a confirmação da matrícula.");
            return CustomResponse(response);
        }
    }
}

