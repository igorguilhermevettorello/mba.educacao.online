using MBA.Educacao.Online.API.Controllers.Base;
using MBA.Educacao.Online.API.DTOs;
using MBA.Educacao.Online.Core.Application.Models;
using MBA.Educacao.Online.Core.Domain.Enums;
using MBA.Educacao.Online.Core.Domain.Interfaces.Identity;
using MBA.Educacao.Online.Core.Domain.Interfaces.Mediator;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Cursos.Application.Commands.Cursos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MBA.Educacao.Online.API.Controllers
{
    [Authorize(Roles = nameof(TipoUsuario.Administrador))]
    // [Authorize]
    [ApiController]
    [Route("api/cursos")]
    public class CursosController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public CursosController(IMediatorHandler mediatorHandler, INotificador notificador, IUser appUser) 
            : base(notificador, appUser)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Criar([FromBody] CriarCursoDto criarCursoDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var command = new CriarCursoCommand(
                criarCursoDto.Titulo,
                criarCursoDto.Descricao,
                criarCursoDto.Instrutor,
                criarCursoDto.Nivel,
                criarCursoDto.Valor
            );

            var resultado = await _mediatorHandler.EnviarComando(command);

            if (!resultado)
                return CustomResponse();

            var response = Result.Ok(command.AggregateId, "Curso criado com sucesso");
            return CreatedAtAction(nameof(ObterPorId), new { id = command.AggregateId }, response);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Atualizar(Guid id, [FromBody] AtualizarCursoDto atualizarCursoDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var command = new AtualizarCursoCommand
            {
                Id = id,
                Titulo = atualizarCursoDto.Titulo,
                Descricao = atualizarCursoDto.Descricao,
                Nivel = atualizarCursoDto.Nivel
            };

            var resultado = await _mediatorHandler.EnviarComando(command);

            if (!resultado)
                return CustomResponse();

            var response = Result.Ok("Curso atualizado com sucesso");
            return CustomResponse(response);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Result<CursoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ObterPorId(Guid id)
        {
            var command = new ObterCursoPorIdCommand(id);
            var curso = await _mediatorHandler.EnviarComando(command);

            if (curso == null)
            {
                NotificarErro("Curso", "Curso não encontrado");
                return CustomResponse();
            }

            var cursoDto = new CursoDto
            {
                Id = curso.Id,
                Titulo = curso.Titulo,
                Descricao = curso.Descricao,
                Instrutor = curso.Instrutor,
                Nivel = curso.Nivel,
                Valor = curso.Valor,
                DataCriacao = curso.DataCriacao,
                Ativo = curso.Ativo
            };

            var response = Result.Ok(cursoDto, "Curso obtido com sucesso");
            return CustomResponse(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Result<IEnumerable<CursoDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Listar([FromQuery] bool apenasAtivos = false)
        {
            var command = new ListarCursosCommand(apenasAtivos);
            var cursos = await _mediatorHandler.EnviarComando(command);

            var cursosDto = cursos.Select(c => new CursoDto
            {
                Id = c.Id,
                Titulo = c.Titulo,
                Descricao = c.Descricao,
                Instrutor = c.Instrutor,
                Nivel = c.Nivel,
                Valor = c.Valor,
                DataCriacao = c.DataCriacao,
                Ativo = c.Ativo
            }).ToList();

            var response = Result.Ok<IEnumerable<CursoDto>>(cursosDto, "Cursos obtidos com sucesso");
            return CustomResponse(response);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Deletar(Guid id)
        {
            var command = new DeletarCursoCommand(id);
            var resultado = await _mediatorHandler.EnviarComando(command);

            if (!resultado)
                return CustomResponse();

            var response = Result.Ok("Curso deletado com sucesso");
            return CustomResponse(response);
        }

        [HttpPut("{id:guid}/ativar")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Ativar(Guid id)
        {
            var command = new AtivarCursoCommand { CursoId = id };
            var resultado = await _mediatorHandler.EnviarComando(command);

            if (!resultado)
                return CustomResponse();

            var response = Result.Ok("Curso ativado com sucesso");
            return CustomResponse(response);
        }

        [HttpPut("{id:guid}/inativar")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Inativar(Guid id)
        {
            var command = new InativarCursoCommand(id);
            var resultado = await _mediatorHandler.EnviarComando(command);

            if (!resultado)
                return CustomResponse();

            var response = Result.Ok("Curso inativado com sucesso");
            return CustomResponse(response);
        }
    }
}

