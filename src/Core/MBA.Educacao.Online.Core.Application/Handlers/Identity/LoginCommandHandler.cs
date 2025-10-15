using MBA.Educacao.Online.Core.Application.Commands.Identity;
using MBA.Educacao.Online.Core.Application.Models;
using MBA.Educacao.Online.Core.Domain.Interfaces.Identity;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Core.Domain.Notifications;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace MBA.Educacao.Online.Core.Application.Handlers.Identity
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly INotificador _notificador;

        public LoginCommandHandler(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IJwtTokenService jwtTokenService,
            INotificador notificador)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _notificador = notificador;
        }

        public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Validação do comando
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
                return LoginResult.Falha();
            }

            // Buscar usuário por email
            var usuario = await _userManager.FindByEmailAsync(request.Email);
            if (usuario == null)
            {
                _notificador.Handle(new Notificacao
                {
                    Campo = "Login",
                    Mensagem = "E-mail ou senha inválidos"
                });
                return LoginResult.Falha();
            }

            // Verificar senha
            var resultadoLogin = await _signInManager.CheckPasswordSignInAsync(usuario, request.Senha, lockoutOnFailure: false);
            if (!resultadoLogin.Succeeded)
            {
                _notificador.Handle(new Notificacao
                {
                    Campo = "Login",
                    Mensagem = "E-mail ou senha inválidos"
                });
                return LoginResult.Falha();
            }

            // Gerar token JWT
            var usuarioId = Guid.Parse(usuario.Id);
            var token = _jwtTokenService.GerarToken(usuarioId, usuario.Email, usuario.UserName);

            // Retornar resultado de sucesso
            var resultado = LoginResult.Sucesso(token, usuario.Email, usuario.UserName, usuarioId);
            request.SetResult(resultado);

            return resultado;
        }
    }
}

