using System.Runtime.InteropServices.ComTypes;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Cursos.Domain.Events
{
    public class CursoAvisarAlunosEventHandler : INotificationHandler<CursoAvisarAlunosEvent>
    {
        private readonly ICursoRepository _cursoRepository;

        public CursoAvisarAlunosEventHandler(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }
        
        public async Task Handle(CursoAvisarAlunosEvent notification, CancellationToken cancellationToken)
        {
            var curso = _cursoRepository.BuscarPorId(notification.AggregateId);
        }
    }
}