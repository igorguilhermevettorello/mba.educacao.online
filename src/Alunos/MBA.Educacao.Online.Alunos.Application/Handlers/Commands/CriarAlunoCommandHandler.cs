using MBA.Educacao.Online.Alunos.Application.Commands;
using MBA.Educacao.Online.Alunos.Domain.Entities;
using MBA.Educacao.Online.Alunos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Core.Domain.Extensions;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Notifications;
using MediatR;

namespace MBA.Educacao.Online.Alunos.Application.Handlers.Commands
{
    public class CriarAlunoCommandHandler : IRequestHandler<CriarAlunoCommand, bool>
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly INotificador _notificador;

        public CriarAlunoCommandHandler(IAlunoRepository alunoRepository, INotificador notificador)
        {
            _alunoRepository = alunoRepository;
            _notificador = notificador;
        }

        public async Task<bool> Handle(CriarAlunoCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                foreach (var erro in request.ValidationResult.Errors)
                {
                    _notificador.Handle(new Notificacao
                    {
                        Campo = erro.PropertyName,
                        Mensagem = erro.ErrorMessage
                    });
                }
                return false;
            }

            // Verifica se já existe um aluno com este UsuarioId ou Email
            var alunoExistente = _alunoRepository.BuscarPorUsuarioId(request.UsuarioId);
            if (alunoExistente != null)
            {
                _notificador.Handle(new Notificacao
                {
                    Campo = "UsuarioId",
                    Mensagem = "Já existe um aluno cadastrado para este usuário"
                });
                return false;
            }

            // Cria a entidade Aluno
            var aluno = new Aluno(request.UsuarioId.Normalize(), request.Nome, request.Email);

            _alunoRepository.Adicionar(aluno);
            return await _alunoRepository.UnitOfWork.Commit();
        }
    }
}

