namespace MBA.Educacao.Online.Core.Domain.DTOs
{
    public class MatriculaItemDto
    {
        public Guid CursoId { get; set; }
        public Guid MatriculaId { get; set; }

        public MatriculaItemDto() { }

        public MatriculaItemDto(Guid cursoId, Guid matriculaId)
        {
            CursoId = cursoId;
            MatriculaId = matriculaId;
        }
    }
}

