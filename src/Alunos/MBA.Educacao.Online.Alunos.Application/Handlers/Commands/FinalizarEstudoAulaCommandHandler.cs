using MBA.Educacao.Online.Alunos.Application.Commands;
using MBA.Educacao.Online.Alunos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Core.Domain.Notifications;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Alunos.Application.Handlers.Commands
{
    public class FinalizarEstudoAulaCommandHandler : IRequestHandler<FinalizarEstudoAulaCommand, bool>
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly ICursoRepository _cursoRepository;
        private readonly IAulaRepository _aulaRepository;
        private readonly INotificador _notificador;

        public FinalizarEstudoAulaCommandHandler(
            IMatriculaRepository matriculaRepository,
            ICursoRepository cursoRepository,
            IAulaRepository aulaRepository,
            INotificador notificador)
        {
            _matriculaRepository = matriculaRepository;
            _cursoRepository = cursoRepository;
            _aulaRepository = aulaRepository;
            _notificador = notificador;
        }

        public async Task<bool> Handle(FinalizarEstudoAulaCommand request, CancellationToken cancellationToken)
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

            // Verifica se a aula existe
            var aula = await _aulaRepository.BuscarPorIdAsync(request.AulaId);
            if (aula == null)
            {
                Notificar("AulaId", $"Aula com ID {request.AulaId} não encontrada");
                return false;
            }

            // Verifica se a aula pertence ao curso
            if (aula.CursoId != request.CursoId)
            {
                Notificar("AulaId", "Aula não pertence ao curso informado");
                return false;
            }

            // Busca a matrícula pelo ID
            var matricula = _matriculaRepository.BuscarPorId(request.MatriculaId);
            if (matricula == null)
            {
                Notificar("MatriculaId", $"Matrícula com ID {request.MatriculaId} não encontrada");
                return false;
            }

            // Verifica se a matrícula pertence ao aluno
            if (matricula.AlunoId != request.AlunoId)
            {
                Notificar("Matricula", "Matrícula não pertence ao aluno informado");
                return false;
            }

            // Verifica se o curso da matrícula corresponde ao informado
            if (matricula.CursoId != request.CursoId)
            {
                Notificar("CursoId", "Curso não corresponde à matrícula informada");
                return false;
            }

            // Verifica se a matrícula está ativa
            if (!matricula.Ativo)
            {
                Notificar("Matricula", "Matrícula não está ativa");
                return false;
            }

            // Verifica se a matrícula está vencida
            if (matricula.EstaVencida())
            {
                Notificar("Matricula", "Matrícula está vencida");
                return false;
            }

            // Obtém o total de aulas do curso
            var totalAulas = curso.ObterTotalAulas();
            if (totalAulas == 0)
            {
                Notificar("Curso", "Curso não possui aulas cadastradas");
                return false;
            }

            // Finaliza o estudo da aula e recalcula o progresso
            try
            {
                matricula.FinalizarEstudoAula(request.AulaId, totalAulas);
            }
            catch (InvalidOperationException ex)
            {
                Notificar("Estudo", ex.Message);
                return false;
            }
            catch (ArgumentException ex)
            {
                Notificar("Estudo", ex.Message);
                return false;
            }

            // Marca a matrícula como alterada
            _matriculaRepository.Alterar(matricula);

            // Salva as alterações
            var resultado = await _matriculaRepository.UnitOfWork.Commit();
            if (!resultado)
            {
                Notificar("Estudo", "Erro ao finalizar o estudo da aula");
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

