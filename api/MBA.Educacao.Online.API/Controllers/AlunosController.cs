using AutoMapper;
using MBA.Educacao.Online.Alunos.Application.Commands;
using MBA.Educacao.Online.Alunos.Application.Queries;
using MBA.Educacao.Online.Alunos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Alunos.Domain.Interfaces.Services;
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
        private readonly ICertificadoRepository _certificadoRepository;
        private readonly IAlunoRepository _alunoRepository;
        private readonly ICertificadoPdfService _certificadoPdfService;
        private readonly IMapper _mapper;

        public AlunosController(
            IMediatorHandler mediatorHandler,
            INotificador notificador,
            IUser appUser,
            ICursoRepository cursoRepository,
            ICertificadoRepository certificadoRepository,
            IAlunoRepository alunoRepository,
            ICertificadoPdfService certificadoPdfService,
            IMapper mapper)
            : base(notificador, appUser)
        {
            _mediatorHandler = mediatorHandler;
            _cursoRepository = cursoRepository;
            _certificadoRepository = certificadoRepository;
            _alunoRepository = alunoRepository;
            _certificadoPdfService = certificadoPdfService;
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

        [HttpPost("aulas/solicitacao-certificado")]
        [ProducesResponseType(typeof(ResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> SolicitarCertificado([FromBody] SolicitarCertificadoDto certificadoDto)
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

            var command = new SolicitarCertificadoCommand(
                alunoId,
                certificadoDto.MatriculaId,
                certificadoDto.CursoId
            );

            var resultado = await _mediatorHandler.EnviarComando(command);

            if (!resultado)
                return CustomResponse();

            var response = ResultDto.Ok("Certificado gerado com sucesso! Parabéns pela conclusão do curso.");
            return CustomResponse(response);
        }

        [HttpGet("certificados")]
        [ProducesResponseType(typeof(ResultDto<List<CertificadoDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ListarCertificados()
        {
            if (!UsuarioAutenticado)
            {
                NotificarErro("Usuario", "Usuário não autenticado");
                return Unauthorized();
            }

            // Obtém o ID do aluno do JWT
            var alunoId = UsuarioId;

            // Busca os certificados do aluno
            var certificados = _certificadoRepository.BuscarPorAluno(alunoId);

            if (certificados == null || !certificados.Any())
            {
                var responseVazio = ResultDto.Ok(new List<CertificadoDto>(), "Nenhum certificado encontrado.");
                return CustomResponse(responseVazio);
            }

            // Mapeia para DTO e busca informações dos cursos
            var certificadosDto = new List<CertificadoDto>();

            foreach (var certificado in certificados)
            {
                var curso = await _cursoRepository.BuscarPorIdAsync(certificado.CursoId);

                certificadosDto.Add(new CertificadoDto
                {
                    Id = certificado.Id,
                    CursoId = certificado.CursoId,
                    CursoNome = curso?.Titulo ?? "Curso não encontrado",
                    DataEmissao = certificado.DataEmissao,
                    Codigo = certificado.Codigo,
                    Ativo = certificado.Ativo
                });
            }

            var response = ResultDto.Ok(certificadosDto, "Certificados obtidos com sucesso.");
            return CustomResponse(response);
        }

        [HttpGet("certificados/visualizar-certificado/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> VisualizarCertificado(Guid id)
        {
            if (!UsuarioAutenticado)
            {
                NotificarErro("Usuario", "Usuário não autenticado");
                return Unauthorized();
            }

            // Obtém o ID do aluno do JWT
            var alunoId = UsuarioId;

            // Busca o certificado
            var certificado = _certificadoRepository.BuscarPorId(id);
            if (certificado == null)
            {
                NotificarErro("CertificadoId", $"Certificado com ID {id} não encontrado");
                return CustomResponse();
            }

            // Verifica se o certificado pertence ao aluno autenticado
            var aluno = _alunoRepository.BuscarPorId(alunoId);
            if (aluno == null || !aluno.Certificados.Any(c => c.Id == id))
            {
                NotificarErro("Certificado", "Você não tem permissão para visualizar este certificado");
                return CustomResponse();
            }

            // Busca informações do curso
            var curso = await _cursoRepository.BuscarPorIdAsync(certificado.CursoId);
            if (curso == null)
            {
                NotificarErro("Curso", "Curso não encontrado");
                return CustomResponse();
            }

            // Gera o PDF do certificado
            try
            {
                var pdfBytes = await _certificadoPdfService.GerarCertificadoPdfAsync(
                    aluno.Nome,
                    curso.Titulo,
                    certificado.DataEmissao,
                    certificado.Codigo
                );

                // Retorna o PDF como arquivo para download
                return File(pdfBytes, "application/pdf", $"Certificado_{certificado.Codigo}.pdf");
            }
            catch (Exception ex)
            {
                NotificarErro("Certificado", $"Erro ao gerar PDF do certificado: {ex.Message}");
                return CustomResponse();
            }
        }
    }
}

