namespace MBA.Educacao.Online.Core.Domain.Models
{
    public class Entity
    {
        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }    
}

