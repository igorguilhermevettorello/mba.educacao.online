using MBA.Educacao.Online.Cursos.Domain.DTOs.Cursos;

namespace MBA.Educacao.Online.Cursos.Domain.Interfaces.Services
{
    public interface ICursoAppService : IDisposable
    {
        Task<CursoListarDto> ObterPorId(Guid id);
        Task<IEnumerable<CursoListarDto>> ObterTodos();
        Task Adicionar(CursoCriarDto cursoDto);
    }
}

