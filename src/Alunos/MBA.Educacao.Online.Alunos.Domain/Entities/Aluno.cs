using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Core.Domain.Models;

namespace MBA.Educacao.Online.Alunos.Domain.Entities
{
    public class Aluno : Entity, IAggregateRoot
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public bool Ativo { get; private set; }

        // Construtor para EF
        protected Aluno() { }

        public Aluno(Guid usuarioId, string nome, string email)
        {
            Id = usuarioId;
            Nome = nome;
            Email = email;
            DataCadastro = DateTime.Now;
            Ativo = true;
        }

        public void Inativar()
        {
            Ativo = false;
        }

        public void Ativar()
        {
            Ativo = true;
        }

        public void AtualizarNome(string nome)
        {
            Nome = nome;
        }
    }
}

