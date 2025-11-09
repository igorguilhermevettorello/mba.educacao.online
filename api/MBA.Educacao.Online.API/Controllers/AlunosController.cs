using AutoMapper;
using MBA.Educacao.Online.Alunos.Application.Commands;
using MBA.Educacao.Online.Alunos.Application.Queries;
using MBA.Educacao.Online.API.Controllers.Base;
using MBA.Educacao.Online.API.DTOs.Alunos;
using MBA.Educacao.Online.Core.Application.DTOs;
using MBA.Educacao.Online.Core.Domain.Enums;
using MBA.Educacao.Online.Core.Domain.Interfaces.Identity;
using MBA.Educacao.Online.Core.Domain.Interfaces.Mediator;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Cursos.Application.Queries;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MBA.Educacao.Online.API.Controllers
{
    [Authorize(Roles = nameof(TipoUsuario.Aluno))]
    [ApiController]
    [Route("api/alunos")]
    public class AlunosController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly ICursoRepository _cursoRepository;
        private readonly IMapper _mapper;

        public AlunosController(
            IMediatorHandler mediatorHandler,
            INotificador notificador,
            IUser appUser,
            ICursoRepository cursoRepository,
            IMapper mapper)
            : base(notificador, appUser)
        {
            _mediatorHandler = mediatorHandler;
            _cursoRepository = cursoRepository;
            _mapper = mapper;
        }

        [HttpGet("meus-cursos")]
        [ProducesResponseType(typeof(ResultDto<List<CursoMatriculadoDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ObterMeusCursos()
        {
            if (!UsuarioAutenticado)
            {
                NotificarErro("Usuario", "Usuário não autenticado");
                return Unauthorized();
            }

            // Obtém o ID do aluno do JWT
            var alunoId = UsuarioId;

            var query = new ObterCursosMatriculadosQuery(alunoId);
            var matriculas = await _mediatorHandler.EnviarComando(query);

            if (matriculas == null || !matriculas.Any())
            {
                var responseVazio = ResultDto.Ok(new List<CursoMatriculadoDto>(), "Nenhum curso matriculado encontrado.");
                return CustomResponse(responseVazio);
            }

            // Busca informações dos cursos
            var cursosMatriculados = new List<CursoMatriculadoDto>();

            foreach (var matricula in matriculas)
            {
                var curso = await _cursoRepository.BuscarPorIdAsync(matricula.CursoId);
                if (curso != null)
                {
                    cursosMatriculados.Add(new CursoMatriculadoDto
                    {
                        MatriculaId = matricula.Id,
                        CursoId = matricula.CursoId,
                        CursoNome = curso.Titulo,
                        DataMatricula = matricula.DataMatricula,
                        DataValidade = matricula.DataValidade,
                        Ativo = matricula.Ativo,
                        EstaVencida = matricula.EstaVencida(),
                        DiasRestantes = matricula.ObterDiasRestantes(),
                        ProgressoPercentual = matricula.ObterProgressoGeral()
                    });
                }
            }

            var response = ResultDto.Ok(cursosMatriculados, "Cursos matriculados obtidos com sucesso.");
            return CustomResponse(response);
        }

        [HttpGet("cursos/{cursoId:guid}/aulas")]
        [ProducesResponseType(typeof(ResultDto<List<AulaCursoDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ObterAulasPorCurso(Guid cursoId)
        {
            if (!UsuarioAutenticado)
            {
                NotificarErro("Usuario", "Usuário não autenticado");
                return Unauthorized();
            }

            // Verifica se o curso existe
            var curso = await _cursoRepository.BuscarPorIdAsync(cursoId);
            if (curso == null)
            {
                NotificarErro("CursoId", $"Curso com ID {cursoId} não encontrado");
                return CustomResponse();
            }

            var query = new ObterAulasPorCursoQuery(cursoId);
            var aulas = await _mediatorHandler.EnviarComando(query);

            if (aulas == null || !aulas.Any())
            {
                var responseVazio = ResultDto.Ok(new List<AulaCursoDto>(), "Nenhuma aula encontrada para este curso.");
                return CustomResponse(responseVazio);
            }

            // Mapeia as aulas para o DTO
            var aulasDto = _mapper.Map<List<AulaCursoDto>>(aulas);

            var response = ResultDto.Ok(aulasDto, "Aulas obtidas com sucesso.");
            return CustomResponse(response);
        }

        [HttpPost("aulas/acessar-estudo")]
        [ProducesResponseType(typeof(ResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> AcessarEstudoAula([FromBody] AcessarEstudoDto estudoDto)
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

            var command = new AcessarEstudoAulaCommand(
                alunoId,
                estudoDto.MatriculaId,
                estudoDto.CursoId,
                estudoDto.AulaId
            );

            var resultado = await _mediatorHandler.EnviarComando(command);

            if (!resultado)
                return CustomResponse();

            var response = ResultDto.Ok("Aula acessada com sucesso. Você pode iniciar o estudo.");
            return CustomResponse(response);
        }

        [HttpPost("aulas/iniciar-estudo")]
        [ProducesResponseType(typeof(ResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> IniciarEstudoAula([FromBody] IniciarEstudoDto estudoDto)
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

            var command = new IniciarEstudoAulaCommand(
                alunoId,
                estudoDto.MatriculaId,
                estudoDto.CursoId,
                estudoDto.AulaId
            );

            var resultado = await _mediatorHandler.EnviarComando(command);

            if (!resultado)
                return CustomResponse();

            var response = ResultDto.Ok("Estudo da aula iniciado com sucesso.");
            return CustomResponse(response);
        }

        [HttpPost("aulas/finalizar-estudo")]
        [ProducesResponseType(typeof(ResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> FinalizarEstudoAula([FromBody] FinalizarEstudoDto estudoDto)
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

            var command = new FinalizarEstudoAulaCommand(
                alunoId,
                estudoDto.MatriculaId,
                estudoDto.CursoId,
                estudoDto.AulaId
            );

            var resultado = await _mediatorHandler.EnviarComando(command);

            if (!resultado)
                return CustomResponse();

            var response = ResultDto.Ok("Estudo da aula finalizado com sucesso. Progresso atualizado.");
            return CustomResponse(response);
        }
    }
}

