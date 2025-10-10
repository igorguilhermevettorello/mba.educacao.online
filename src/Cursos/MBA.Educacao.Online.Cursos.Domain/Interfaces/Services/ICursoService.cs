using MBA.Educacao.Online.Cursos.Domain.Entities;

namespace MBA.Educacao.Online.Cursos.Domain.Interfaces.Services
{
    public interface ICursoService : IDisposable
    {
        Task<bool> VerificarAulas(Guid cursoId, Aula aula);   
    }
}