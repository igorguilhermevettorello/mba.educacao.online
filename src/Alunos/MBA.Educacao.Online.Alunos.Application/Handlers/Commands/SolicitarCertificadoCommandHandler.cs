using MBA.Educacao.Online.Alunos.Application.Commands;
using MBA.Educacao.Online.Alunos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Core.Domain.Notifications;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Alunos.Application.Handlers.Commands
{
    public class SolicitarCertificadoCommandHandler : IRequestHandler<SolicitarCertificadoCommand, bool>
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly IAlunoRepository _alunoRepository;
        private readonly ICursoRepository _cursoRepository;
        private readonly ICertificadoRepository _certificadoRepository;
        private readonly INotificador _notificador;

        public SolicitarCertificadoCommandHandler(
            IMatriculaRepository matriculaRepository,
            IAlunoRepository alunoRepository,
            ICursoRepository cursoRepository,
            ICertificadoRepository certificadoRepository,
            INotificador notificador)
        {
            _matriculaRepository = matriculaRepository;
            _alunoRepository = alunoRepository;
            _cursoRepository = cursoRepository;
            _certificadoRepository = certificadoRepository;
            _notificador = notificador;
        }

        public async Task<bool> Handle(SolicitarCertificadoCommand request, CancellationToken cancellationToken)
        {
            // Valida o comando
            if (!ValidarComando(request)) return false;

            var matricula = _matriculaRepository.BuscarPorId(request.MatriculaId);
            if (matricula == null)
            {
                Notificar("MatriculaId", $"Matrícula com ID {request.MatriculaId} não encontrada");
                return false;
            }

            if (matricula.AlunoId != request.AlunoId)
            {
                Notificar("Matricula", "Matrícula não pertence ao aluno informado");
                return false;
            }

            if (matricula.CursoId != request.CursoId)
            {
                Notificar("CursoId", "Curso não corresponde à matrícula informada");
                return false;
            }

            if (!matricula.Ativo)
            {
                Notificar("Matricula", "Matrícula não está ativa");
                return false;
            }

            if (matricula.ProgressoPercentual != 100)
            {
                Notificar("Progresso", $"O curso deve estar 100% concluído para solicitar certificado. Progresso atual: {matricula.ProgressoPercentual}%");
                return false;
            }

            var aluno = _alunoRepository.BuscarPorId(request.AlunoId);
            if (aluno == null)
            {
                Notificar("AlunoId", "Aluno não encontrado");
                return false;
            }

            var curso = await _cursoRepository.BuscarPorIdAsync(request.CursoId);
            if (curso == null)
            {
                Notificar("CursoId", "Curso não encontrado");
                return false;
            }

            var certificadoExistente = _certificadoRepository.BuscarPorAlunoECurso(request.AlunoId, request.CursoId);
            if (certificadoExistente != null)
            {
                Notificar("Certificado", "Já existe um certificado emitido para este curso");
                return false;
            }

            var codigoCertificado = GerarCodigoCertificado(request.AlunoId, request.CursoId);
            try
            {

                aluno.AdicionarCertificado(request.CursoId, codigoCertificado);
                var certificado = aluno.Certificados
                    .FirstOrDefault(c => c.CursoId == request.CursoId && c.Codigo == codigoCertificado);

                if (certificado == null)
                {
                    Notificar("Certificado", "Erro ao criar certificado no agregado");
                    return false;
                }

                _certificadoRepository.Adicionar(certificado);
                _alunoRepository.Alterar(aluno);

                var resultado = await _alunoRepository.UnitOfWork.Commit();
                if (!resultado)
                {
                    Notificar("Certificado", "Erro ao gerar certificado");
                    return false;
                }

                return true;
            }
            catch (InvalidOperationException ex)
            {
                Notificar("Certificado", ex.Message);
                return false;
            }
            catch (ArgumentException ex)
            {
                Notificar("Certificado", ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Notificar("Certificado", $"Erro ao gerar certificado: {ex.Message}");
                return false;
            }
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

        private static string GerarCodigoCertificado(Guid alunoId, Guid cursoId)
        {
            // Gera um código único baseado no aluno, curso e timestamp
            var hash = $"{alunoId}{cursoId}{DateTime.Now.Ticks}";
            var guid = Guid.NewGuid().ToString("N").ToUpper();
            return $"CERT-{guid.Substring(0, 8)}-{DateTime.Now:yyyyMMdd}";
        }
    }
}

