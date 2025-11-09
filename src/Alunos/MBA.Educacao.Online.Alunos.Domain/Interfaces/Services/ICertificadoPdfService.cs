namespace MBA.Educacao.Online.Alunos.Domain.Interfaces.Services
{
    public interface ICertificadoPdfService
    {
        Task<byte[]> GerarCertificadoPdfAsync(
            string nomeAluno,
            string nomeCurso,
            DateTime dataConclusao,
            string codigoCertificado);
    }
}

