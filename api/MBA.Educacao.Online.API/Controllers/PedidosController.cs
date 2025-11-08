using AutoMapper;
using MBA.Educacao.Online.Alunos.Application.Interfaces;
using MBA.Educacao.Online.API.Controllers.Base;
using MBA.Educacao.Online.API.DTOs.Alunos;
using MBA.Educacao.Online.API.DTOs.Pedido;
using MBA.Educacao.Online.Core.Application.DTOs;
using MBA.Educacao.Online.Core.Domain.Enums;
using MBA.Educacao.Online.Core.Domain.Interfaces.Identity;
using MBA.Educacao.Online.Core.Domain.Interfaces.Mediator;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Vendas.Application.Commands;
using MBA.Educacao.Online.Vendas.Application.Queries;
using MBA.Educacao.Online.Vendas.Domain.Interfaces.Repositories;
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
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMatriculaService _matriculaService;
        private readonly IMapper _mapper;

        public PedidosController(
            IMediatorHandler mediatorHandler,
            INotificador notificador,
            IUser appUser,
            IPedidoRepository pedidoRepository,
            IMatriculaService matriculaService,
            IMapper mapper)
            : base(notificador, appUser)
        {
            _mediatorHandler = mediatorHandler;
            _pedidoRepository = pedidoRepository;
            _matriculaService = matriculaService;
            _mapper = mapper;
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

            // Busca o pedido rascunho do aluno para verificar os cursos
            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorAlunoId(alunoId);
            if (pedido == null)
            {
                NotificarErro("Pedido", "Pedido não encontrado para este aluno");
                return CustomResponse();
            }

            if (!pedido.PossuiItens())
            {
                NotificarErro("Pedido", "Pedido não possui itens para pagamento");
                return CustomResponse();
            }

            // Verifica se o aluno já está matriculado em algum dos cursos do pedido
            var cursoIds = pedido.PedidoItens.Select(pi => pi.CursoId).ToList();
            var verificacao = await _matriculaService.VerificarMatriculasParaPagamento(alunoId, cursoIds);

            if (!verificacao.PodeProsseguir)
            {
                NotificarErro("Matricula", verificacao.MensagemErro);
                return CustomResponse();
            }

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

        [Authorize(Roles = nameof(TipoUsuario.Aluno))]
        [HttpGet("meu-pedido")]
        [ProducesResponseType(typeof(ResultDto<PedidoRascunhoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ObterMeuPedidoRascunho()
        {
            if (!UsuarioAutenticado)
            {
                NotificarErro("Usuario", "Usuário não autenticado");
                return Unauthorized();
            }

            // Obtém o ID do aluno do JWT
            var alunoId = UsuarioId;

            var query = new ObterPedidoRascunhoQuery(alunoId);
            var pedido = await _mediatorHandler.EnviarComando(query);

            if (pedido == null)
            {
                NotificarErro("Pedido", "Nenhum pedido rascunho encontrado para este aluno");
                return CustomResponse();
            }

            // Usa AutoMapper para mapear Pedido -> PedidoRascunhoDto
            var pedidoDto = _mapper.Map<PedidoRascunhoDto>(pedido);

            var response = ResultDto.Ok(pedidoDto, "Pedido obtido com sucesso.");
            return CustomResponse(response);
        }

        [Authorize(Roles = nameof(TipoUsuario.Aluno))]
        [HttpDelete("remover-curso/{cursoId:guid}")]
        [ProducesResponseType(typeof(ResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemoverCursoDoPedido(Guid cursoId)
        {
            if (!UsuarioAutenticado)
            {
                NotificarErro("Usuario", "Usuário não autenticado");
                return Unauthorized();
            }

            // Obtém o ID do aluno do JWT
            var alunoId = UsuarioId;

            var command = new RemoverCursoDoPedidoCommand(alunoId, cursoId);

            var resultado = await _mediatorHandler.EnviarComando(command);

            if (!resultado)
                return CustomResponse();

            var response = ResultDto.Ok("Curso removido do pedido com sucesso.");
            return CustomResponse(response);
        }
    }
}

