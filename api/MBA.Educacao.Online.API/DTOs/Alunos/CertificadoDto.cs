namespace MBA.Educacao.Online.API.DTOs.Alunos
{
    public class CertificadoDto
    {
        public Guid Id { get; set; }
        public Guid CursoId { get; set; }
        public string CursoNome { get; set; } = string.Empty;
        public DateTime DataEmissao { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}

