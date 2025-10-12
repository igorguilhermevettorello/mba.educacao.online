using MBA.Educacao.Online.Core.Domain.Messages;

namespace MBA.Educacao.Online.Core.Application.Events.Identity
{
    public class UsuarioCriadoEvent : Event
    {
        public Guid UsuarioId { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }

        public UsuarioCriadoEvent(Guid usuarioId, string nome, string email)
        {
            UsuarioId = usuarioId;
            Nome = nome;
            Email = email;
        }
    }
}

