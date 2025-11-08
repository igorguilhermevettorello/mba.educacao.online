namespace MBA.Educacao.Online.Alunos.Application.DTOs
{
    public class VerificacaoPedidoMatriculaDto
    {
        public bool PodeProsseguir { get; set; }
        public string MensagemErro { get; set; }
        public List<string> CursosComMatriculaAtiva { get; set; }

        public VerificacaoPedidoMatriculaDto()
        {
            MensagemErro = string.Empty;
            CursosComMatriculaAtiva = new List<string>();
        }

        public static VerificacaoPedidoMatriculaDto Sucesso()
        {
            return new VerificacaoPedidoMatriculaDto
            {
                PodeProsseguir = true,
                MensagemErro = string.Empty,
                CursosComMatriculaAtiva = new List<string>()
            };
        }

        public static VerificacaoPedidoMatriculaDto Falha(string mensagem, List<string> cursosComMatricula)
        {
            return new VerificacaoPedidoMatriculaDto
            {
                PodeProsseguir = false,
                MensagemErro = mensagem,
                CursosComMatriculaAtiva = cursosComMatricula
            };
        }
    }
}

