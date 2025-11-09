namespace MBA.Educacao.Online.API.DTOs.Alunos
{
    public class AulaCursoDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int Ordem { get; set; }
        public int DuracaoMinutos { get; set; }
        public bool Ativo { get; set; }

        public AulaCursoDto()
        {
            Titulo = string.Empty;
            Descricao = string.Empty;
        }
    }
}

