using MBA.Educacao.Online.Core.Domain.Models;

namespace MBA.Educacao.Online.Alunos.Domain.Entities
{
    public class Certificado : Entity
    {
        public Guid CursoId { get; private set; }
        public DateTime DataEmissao { get; private set; }
        public string Codigo { get; private set; }
        public bool Ativo { get; private set; }

        protected Certificado() 
        {
            Codigo = string.Empty;
        }

        public Certificado(Guid cursoId, string codigo)
        {
            ValidarCursoId(cursoId);
            ValidarCodigo(codigo);

            CursoId = cursoId;
            Codigo = codigo;
            DataEmissao = DateTime.Now;
            Ativo = true;
        }

        public void Invalidar()
        {
            if (!Ativo)
                throw new InvalidOperationException("Certificado já está invalidado");

            Ativo = false;
        }

        public void Revalidar()
        {
            if (Ativo)
                throw new InvalidOperationException("Certificado já está válido");

            Ativo = true;
        }

        public bool EstaValido()
        {
            return Ativo;
        }

        public int ObterIdadeEmDias()
        {
            return (DateTime.Now - DataEmissao).Days;
        }

        public bool FoiEmitidoHaMaisDe(int dias)
        {
            return ObterIdadeEmDias() > dias;
        }

        private static void ValidarCursoId(Guid cursoId)
        {
            if (cursoId == Guid.Empty)
                throw new ArgumentException("ID do curso é inválido", nameof(cursoId));
        }

        private static void ValidarCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("Código do certificado é obrigatório", nameof(codigo));

            if (codigo.Length < 8)
                throw new ArgumentException("Código do certificado deve ter no mínimo 8 caracteres", nameof(codigo));

            if (codigo.Length > 50)
                throw new ArgumentException("Código do certificado deve ter no máximo 50 caracteres", nameof(codigo));
        }
    }
}

