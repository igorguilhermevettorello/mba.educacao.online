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
            CursoId = cursoId;
            Codigo = codigo;
            DataEmissao = DateTime.Now;
            Ativo = true;
        }

        public void Invalidar()
        {
            Ativo = false;
        }

        public void Revalidar()
        {
            Ativo = true;
        }

        public bool EstaValido()
        {
            return Ativo;
        }
    }
}

