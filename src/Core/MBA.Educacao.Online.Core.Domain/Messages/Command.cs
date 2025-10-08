using FluentValidation.Results;
using MediatR;

namespace MBA.Educacao.Online.Core.Domain.Messages
{
    public abstract class Command : Message, IRequest<bool>
    {
        public DateTime Timestamp { get; set; }
        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
        }
        
        public abstract bool IsValid();
    }
}

