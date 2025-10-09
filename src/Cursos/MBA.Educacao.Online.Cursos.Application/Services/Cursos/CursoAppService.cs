using MBA.Educacao.Online.Cursos.Domain.DTOs.Cursos;
using MBA.Educacao.Online.Cursos.Domain.Interfaces.Services;

namespace MBA.Educacao.Online.Cursos.Application.Services.Cursos
{
    public class CursoAppService : ICursoAppService
    {
        public Task<CursoListarDto> ObterPorId(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CursoListarDto>> ObterTodos()
        {
            throw new NotImplementedException();
        }

        public Task Adicionar(CursoCriarDto cursoDto)
        {
            throw new NotImplementedException();
        }
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

