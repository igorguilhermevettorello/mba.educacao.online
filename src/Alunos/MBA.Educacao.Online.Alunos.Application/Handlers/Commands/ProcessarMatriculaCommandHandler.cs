using MBA.Educacao.Online.Alunos.Application.Commands;
using MBA.Educacao.Online.Alunos.Domain.Entities;
using MBA.Educacao.Online.Alunos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Core.Domain.DTOs;
using MBA.Educacao.Online.Core.Domain.Interfaces.Mediator;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Messages;
using MBA.Educacao.Online.Core.Domain.Messages.CommonMessages.IntegrationEvents;
using MBA.Educacao.Online.Core.Domain.Notifications;
using MediatR;

namespace MBA.Educacao.Online.Alunos.Application.Handlers.Commands
{
    public class ProcessarMatriculaCommandHandler : IRequestHandler<ProcessarMatriculaCommand, bool>
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly INotificador _notificador;
        private readonly IMediatorHandler _mediatorHandler;

        public ProcessarMatriculaCommandHandler(
            IAlunoRepository alunoRepository,
            IMatriculaRepository matriculaRepository,
            INotificador notificador,
            IMediatorHandler mediatorHandler)
        {
            _alunoRepository = alunoRepository;
            _matriculaRepository = matriculaRepository;
            _notificador = notificador;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(ProcessarMatriculaCommand request, CancellationToken cancellationToken)
        {
            // Valida o comando
            if (!ValidarComando(request)) return false;

            var aluno = _alunoRepository.BuscarPorId(request.AlunoId);
            if (aluno == null)
            {
                Notificar("AlunoId", $"Aluno com ID {request.AlunoId} não encontrado");
                return false;
            }

            if (!aluno.Ativo)
            {
                Notificar("Aluno", "Não é possível matricular um aluno inativo");
                return false;
            }

            if (!request.ListaCursos.Itens.Any())
            {
                Notificar("ListaCursos", "Não há cursos para matricular");
                return false;
            }

            var cursosMatriculados = 0;
            var cursosIgnorados = 0;
            var matriculasCriadas = new List<Matricula>();

            foreach (var curso in request.ListaCursos.Itens)
            {
                try
                {
                    // Verifica se já existe matrícula para este aluno e curso
                    var matriculaExistente = _matriculaRepository.BuscarPorAlunoECurso(request.AlunoId, curso.Id);
                    if (matriculaExistente != null && matriculaExistente.Ativo)
                    {
                        cursosIgnorados++;
                        continue;
                    }

                    // Define a data de validade da matrícula (exemplo: 1 ano a partir de hoje)
                    var dataValidade = DateTime.Now.AddYears(1);

                    
                    var novaMatricula = new Matricula(request.AlunoId, curso.Id, dataValidade);
                    _matriculaRepository.Adicionar(novaMatricula);
                    matriculasCriadas.Add(novaMatricula);
                    cursosMatriculados++;
                }
                catch (InvalidOperationException ex)
                {
                    Notificar("Matricula", $"Erro ao matricular no curso {curso.Id}: {ex.Message}");
                    cursosIgnorados++;
                }
                catch (ArgumentException ex)
                {
                    Notificar("Matricula", $"Erro ao matricular no curso {curso.Id}: {ex.Message}");
                    cursosIgnorados++;
                }
            }

            // Verifica se pelo menos uma matrícula foi processada
            if (cursosMatriculados == 0)
            {
                Notificar("Matricula", "Nenhuma matrícula foi processada. Verifique se os cursos já não estão matriculados.");
                return false;
            }

            // Adiciona o evento ao agregado Aluno antes de salvar
            if (matriculasCriadas.Any())
            {
                var listaMatriculas = matriculasCriadas
                    .Select(m => new MatriculaItemDto(m.CursoId, m.Id))
                    .ToList();

                var atualizarPedidoItemEvent = new AtualizarPedidoItemMatriculaEvent(
                    request.PedidoId,
                    listaMatriculas
                );

                aluno.AdicionarEvento(atualizarPedidoItemEvent);
                _alunoRepository.Alterar(aluno);
            }

            var resultado = await _matriculaRepository.UnitOfWork.Commit();
            if (!resultado)
            {
                Notificar("Matricula", "Erro ao salvar as matrículas no banco de dados");
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
