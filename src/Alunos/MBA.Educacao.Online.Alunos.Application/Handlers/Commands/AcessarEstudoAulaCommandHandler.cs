using MBA.Educacao.Online.Alunos.Application.Commands;
using MBA.Educacao.Online.Alunos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Core.Domain.Notifications;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Alunos.Application.Handlers.Commands
{
    public class AcessarEstudoAulaCommandHandler : IRequestHandler<AcessarEstudoAulaCommand, bool>
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly ICursoRepository _cursoRepository;
        private readonly IAulaRepository _aulaRepository;
        private readonly INotificador _notificador;

        public AcessarEstudoAulaCommandHandler(
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

        public async Task<bool> Handle(AcessarEstudoAulaCommand request, CancellationToken cancellationToken)
        {
            // Valida o comando
            if (!ValidarComando(request)) return false;

            var curso = await _cursoRepository.BuscarPorIdAsync(request.CursoId);
            if (curso == null)
            {
                Notificar("CursoId", $"Curso com ID {request.CursoId} não encontrado");
                return false;
            }

            var aula = await _aulaRepository.BuscarPorIdAsync(request.AulaId);
            if (aula == null)
            {
                Notificar("AulaId", $"Aula com ID {request.AulaId} não encontrada");
                return false;
            }

            if (aula.CursoId != request.CursoId)
            {
                Notificar("AulaId", "Aula não pertence ao curso informado");
                return false;
            }

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

            if (matricula.EstaVencida())
            {
                Notificar("Matricula", "Matrícula está vencida");
                return false;
            }

            var historicoExistente = matricula.HistoricosAprendizado
                .FirstOrDefault(h => h.AulaId == request.AulaId);

            if (historicoExistente != null)
            {
                return true;
            }

            try
            {
                matricula.IniciarAprendizado(request.AulaId);
            }
            catch (InvalidOperationException ex)
            {
                Notificar("Estudo", ex.Message);
                return false;
            }

            _matriculaRepository.Alterar(matricula);

            var resultado = await _matriculaRepository.UnitOfWork.Commit();
            if (!resultado)
            {
                Notificar("Estudo", "Erro ao registrar acesso à aula");
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

