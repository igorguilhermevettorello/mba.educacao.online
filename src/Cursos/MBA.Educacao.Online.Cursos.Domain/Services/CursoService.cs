using MBA.Educacao.Online.Core.Domain.Interfaces.Mediator;
using MBA.Educacao.Online.Cursos.Domain.Entities;
using MBA.Educacao.Online.Cursos.Domain.Events;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Services;

namespace MBA.Educacao.Online.Cursos.Domain.Services
{
    public class CursoService : ICursoService
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly IMediatorHandler  _mediatorHandler;
        
        public CursoService(ICursoRepository cursoRepository,  IMediatorHandler mediatorHandler)
        {
            _cursoRepository = cursoRepository;
            _mediatorHandler = mediatorHandler;
        }
        
        public async Task<bool> VerificarAulas(Guid cursoId, Aula aula)
        {
            var curso = await _cursoRepository.BuscarPorIdAsync(cursoId);
            
            var teste = curso.Aulas.FirstOrDefault(p => p.Descricao.Equals(aula.Descricao));
            
            if (teste != null) return false;
            
            await _mediatorHandler.PublicarEvento(new CursoAvisarAlunosEvent(cursoId, aula.Descricao));
            
            return true;
        }
        
        public void Dispose()
        {
            _cursoRepository.Dispose();
        }
    }
}

