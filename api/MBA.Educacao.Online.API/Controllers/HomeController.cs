using MBA.Educacao.Online.API.Controllers.Base;
using MBA.Educacao.Online.API.DTOs;
using MBA.Educacao.Online.Core.Domain.Interfaces.Identity;
using MBA.Educacao.Online.Core.Domain.Interfaces.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace MBA.Educacao.Online.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class HomeController : MainController
    {
        public HomeController(INotificador notificador, IUser appUser) : base(notificador, appUser)
        {
        }

        [HttpPost("registrar")]
        [ProducesResponseType(typeof(RegistrarDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Registrar([FromBody] RegistrarDto registrarDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var resultado = new
            {
                Mensagem = "Registro recebido com sucesso",
                Nome = registrarDto.Nome,
                Email = registrarDto.Email
            };

            return CustomResponse(resultado);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);


            var resultado = new
            {
                Mensagem = "Login recebido com sucesso",
                Email = loginDto.Email
            };

            return CustomResponse(resultado);
        }
    }
}

