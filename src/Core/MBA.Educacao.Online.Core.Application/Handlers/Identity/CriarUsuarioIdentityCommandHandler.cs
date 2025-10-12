using MBA.Educacao.Online.Core.Application.Commands.Identity;
using MBA.Educacao.Online.Core.Application.Events.Identity;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Notifications;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace MBA.Educacao.Online.Core.Application.Handlers.Identity
{
    public class CriarUsuarioIdentityCommandHandler : IRequestHandler<CriarUsuarioIdentityCommand, bool>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMediator _mediator;
        private readonly INotificador _notificador;

        public CriarUsuarioIdentityCommandHandler(
            UserManager<IdentityUser> userManager, 
            IMediator mediator,
            INotificador notificador)
        {
            _userManager = userManager;
            _mediator = mediator;
            _notificador = notificador;
        }

        public async Task<bool> Handle(CriarUsuarioIdentityCommand request, CancellationToken cancellationToken)
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

            var usuarioExistente = await _userManager.FindByEmailAsync(request.Email);
            if (usuarioExistente != null)
            {
                _notificador.Handle(new Notificacao
                {
                    Campo = "Email",
                    Mensagem = "Este e-mail já está cadastrado"
                });
                return false;
            }

            var usuario = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = true
            };

            var resultado = await _userManager.CreateAsync(usuario, request.Senha);

            if (!resultado.Succeeded)
            {
                foreach (var erro in resultado.Errors)
                {
                    _notificador.Handle(new Notificacao
                    {
                        Campo = erro.Code,
                        Mensagem = erro.Description
                    });
                }
                return false;
            }

            // Define o UsuarioId no command para poder ser retornado
            request.SetUsuarioId(Guid.Parse(usuario.Id));

            // Dispara o evento para que o BC de Alunos possa criar o registro
            await _mediator.Publish(new UsuarioCriadoEvent(
                Guid.Parse(usuario.Id),
                request.Nome,
                request.Email
            ), cancellationToken);

            return true;
        }
    }
}

