using MBA.Educacao.Online.Core.Domain.Messages;

namespace MBA.Educacao.Online.Core.Domain.Models
{
    public class Entity
    {
        public Guid Id { get; set; }
        private List<Event> _notificacoes;
        public IReadOnlyCollection<Event> Notificacoes => _notificacoes?.AsReadOnly();
        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public void AdicionarEvento(Event evento)
        {
            _notificacoes = _notificacoes ?? new List<Event>();
            _notificacoes.Add(evento);
        }

        public void RemoverEvento(Event evento)
        {
            _notificacoes?.Remove(evento);
        }

        public void LimparEventos()
        {
            _notificacoes?.Clear();
        }
    }    
}

