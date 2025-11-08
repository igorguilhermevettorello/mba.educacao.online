using MBA.Educacao.Online.Alunos.Application.DTOs;
using MBA.Educacao.Online.Alunos.Application.Interfaces;
using MBA.Educacao.Online.Alunos.Domain.Interfaces.Repositories;

namespace MBA.Educacao.Online.Alunos.Application.Services
{
    public class MatriculaService : IMatriculaService
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly IAlunoRepository _alunoRepository;

        public MatriculaService(
            IMatriculaRepository matriculaRepository,
            IAlunoRepository alunoRepository)
        {
            _matriculaRepository = matriculaRepository;
            _alunoRepository = alunoRepository;
        }

        public async Task<List<MatriculaVerificationDto>> VerificarMatriculasAluno(Guid alunoId, List<Guid> cursoIds)
        {
            // Valida se o aluno existe
            var aluno = _alunoRepository.BuscarPorIdNoTracking(alunoId);
            if (aluno == null)
            {
                return cursoIds.Select(cursoId =>
                    new MatriculaVerificationDto(cursoId, false, false)).ToList();
            }

            var resultados = new List<MatriculaVerificationDto>();

            foreach (var cursoId in cursoIds)
            {
                var matricula = _matriculaRepository.BuscarPorAlunoECurso(alunoId, cursoId);

                if (matricula != null)
                {
                    resultados.Add(new MatriculaVerificationDto(
                        cursoId,
                        estaMatriculado: true,
                        matriculaAtiva: matricula.Ativo && !matricula.EstaVencida(),
                        dataMatricula: matricula.DataMatricula
                    ));
                }
                else
                {
                    resultados.Add(new MatriculaVerificationDto(
                        cursoId,
                        estaMatriculado: false,
                        matriculaAtiva: false
                    ));
                }
            }

            return await Task.FromResult(resultados);
        }

        public async Task<bool> PossuiMatriculaAtivaEmAlgumCurso(Guid alunoId, List<Guid> cursoIds)
        {
            var verificacoes = await VerificarMatriculasAluno(alunoId, cursoIds);
            return verificacoes.Any(v => v.EstaMatriculado && v.MatriculaAtiva);
        }

        public async Task<VerificacaoPedidoMatriculaDto> VerificarMatriculasParaPagamento(Guid alunoId, List<Guid> cursoIds)
        {
            // Verifica se o aluno possui matrícula ativa em algum dos cursos
            var possuiMatriculaAtiva = await PossuiMatriculaAtivaEmAlgumCurso(alunoId, cursoIds);

            if (!possuiMatriculaAtiva)
            {
                return VerificacaoPedidoMatriculaDto.Sucesso();
            }

            // Busca quais cursos já possuem matrícula ativa
            var verificacoes = await VerificarMatriculasAluno(alunoId, cursoIds);

            var cursosComMatricula = verificacoes
                .Where(v => v.EstaMatriculado && v.MatriculaAtiva)
                .Select(v => v.CursoId.ToString())
                .ToList();

            var mensagem = $"O aluno já possui matrícula ativa em {cursosComMatricula.Count} curso(s). " +
                           "Não é possível processar o pagamento para cursos já matriculados.";

            return VerificacaoPedidoMatriculaDto.Falha(mensagem, cursosComMatricula);
        }
    }
}

