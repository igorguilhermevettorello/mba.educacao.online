using MBA.Educacao.Online.Alunos.Application.Commands;
using MBA.Educacao.Online.Alunos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MBA.Educacao.Online.Alunos.Application.Handlers.Commands
{
    public class ProcessarMatriculaCommandHandler : IRequestHandler<ProcessarMatriculaCommand, bool>
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly INotificador _notificador;

        public ProcessarMatriculaCommandHandler(
            IAlunoRepository alunoRepository,
            INotificador notificador)
        {
            _alunoRepository = alunoRepository;
            _notificador = notificador;
        }

        public async Task<bool> Handle(ProcessarMatriculaCommand request, CancellationToken cancellationToken)
        {
            // Valida o comando
            if (!request.IsValid())
            {
                foreach (var erro in request.ValidationResult.Errors)
                {
                    Notificar(erro.PropertyName, erro.ErrorMessage);
                }
                return false;
            }

            // Busca o aluno
            var aluno = _alunoRepository.BuscarPorId(request.AlunoId);
            if (aluno == null)
            {
                Notificar("AlunoId", $"Aluno com ID {request.AlunoId} não encontrado");
                return false;
            }

            // Verifica se o aluno está ativo
            if (!aluno.Ativo)
            {
                Notificar("Aluno", "Não é possível matricular um aluno inativo");
                return false;
            }

            // Valida se existem cursos na lista
            if (!request.ListaCursos.Itens.Any())
            {
                Notificar("ListaCursos", "Não há cursos para matricular");
                return false;
            }

            // Processa cada curso e adiciona a matrícula
            var cursosMatriculados = 0;
            var cursosIgnorados = 0;

            foreach (var curso in request.ListaCursos.Itens)
            {
                try
                {
                    // Verifica se o aluno já está matriculado neste curso
                    if (aluno.EstaMatriculadoNoCurso(curso.Id))
                    {
                        cursosIgnorados++;
                        continue;
                    }

                    // Define a data de validade da matrícula (exemplo: 1 ano a partir de hoje)
                    var dataValidade = DateTime.Now.AddYears(1);

                    // Adiciona a matrícula
                    aluno.AdicionarMatricula(curso.Id, dataValidade);
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

            // Atualiza o aluno no repositório
            _alunoRepository.Alterar(aluno);

            // Salva as alterações
            var resultado = await _alunoRepository.UnitOfWork.Commit();

            if (!resultado)
            {
                Notificar("Matricula", "Erro ao salvar as matrículas no banco de dados");
                return false;
            }

            return true;
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
