using MBA.Educacao.Online.API.Controllers.Base;
using MBA.Educacao.Online.API.DTOs;
using MBA.Educacao.Online.Core.Application.Commands.Identity;
using MBA.Educacao.Online.Core.Application.DTOs;
using MBA.Educacao.Online.Core.Domain.Interfaces.Identity;
using MBA.Educacao.Online.Core.Domain.Interfaces.Mediator;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using MBA.Educacao.Online.Vendas.Application.Commands;
using Microsoft.AspNetCore.Mvc;

namespace MBA.Educacao.Online.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class HomeController : MainController
    {

        private readonly IMediatorHandler _mediatorHandler;

        public HomeController(IMediatorHandler mediatorHandler, INotificador notificador, IUser appUser) : base(notificador, appUser)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpPost("registrar")]
        [ProducesResponseType(typeof(ResultDto<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Registrar([FromBody] RegistrarDto registrarDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var command = new CriarUsuarioIdentityCommand(
                registrarDto.Nome,
                registrarDto.Email,
                registrarDto.Senha,
                registrarDto.Confirmar
            );

            var resultado = await _mediatorHandler.EnviarComando(command);

            if (!resultado)
            {
                // Os erros já foram adicionados ao notificador pelo Handler
                return CustomResponse();
            }

            var response = ResultDto.Ok(command.UsuarioId.Value, "Usuário criado com sucesso");
            return CustomResponse(response);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ResultDto<LoginResultDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var command = new LoginCommand(loginDto.Email, loginDto.Senha);
            var resultado = await _mediatorHandler.EnviarComando(command);
            if (resultado == null || !resultado.Success)
            {
                return CustomResponse();
            }

            //var response = Result.Ok(resultado, "Login realizado com sucesso");
            return CustomResponse(resultado);
        }
    }
}

