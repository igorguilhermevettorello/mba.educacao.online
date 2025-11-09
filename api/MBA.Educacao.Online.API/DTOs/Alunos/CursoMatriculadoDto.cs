namespace MBA.Educacao.Online.API.DTOs.Alunos
{
    public class CursoMatriculadoDto
    {
        public Guid MatriculaId { get; set; }
        public Guid CursoId { get; set; }
        public string CursoNome { get; set; }
        public DateTime DataMatricula { get; set; }
        public DateTime DataValidade { get; set; }
        public bool Ativo { get; set; }
        public bool EstaVencida { get; set; }
        public int DiasRestantes { get; set; }
        public int ProgressoPercentual { get; set; }

        public CursoMatriculadoDto()
        {
            CursoNome = string.Empty;
        }
    }
}

