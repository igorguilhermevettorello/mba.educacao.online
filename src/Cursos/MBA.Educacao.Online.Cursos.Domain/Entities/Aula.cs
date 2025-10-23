using MBA.Educacao.Online.Core.Domain.Models;
using MBA.Educacao.Online.Cursos.Domain.ValueObjects;

namespace MBA.Educacao.Online.Cursos.Domain.Entities
{
    public class Aula : Entity
    {
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public int DuracaoMinutos { get; private set; }
        public int Ordem { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public bool Ativa { get; private set; }
        public Guid CursoId { get; private set; }
        public Curso? Curso { get; private set; }
        private Aula() { }
        public Aula(string titulo, string descricao, int duracaoMinutos, int ordem)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("Título da aula é obrigatório", nameof(titulo));

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição da aula é obrigatória", nameof(descricao));

            if (duracaoMinutos <= 0)
                throw new ArgumentException("Duração deve ser maior que zero", nameof(duracaoMinutos));

            if (ordem <= 0)
                throw new ArgumentException("Ordem deve ser maior que zero", nameof(ordem));
            
            Id = Guid.NewGuid();
            Titulo = titulo;
            Descricao = descricao;
            DuracaoMinutos = duracaoMinutos;
            Ordem = ordem;
            DataCriacao = DateTime.UtcNow;
            Ativa = true;
        }
        
        public void AtualizarTitulo(string novoTitulo)
        {
            if (string.IsNullOrWhiteSpace(novoTitulo))
                throw new ArgumentException("Título da aula é obrigatório", nameof(novoTitulo));

            Titulo = novoTitulo;
        }

        public void AtualizarDescricao(string novaDescricao)
        {
            if (string.IsNullOrWhiteSpace(novaDescricao))
                throw new ArgumentException("Descrição da aula é obrigatória", nameof(novaDescricao));

            Descricao = novaDescricao;
        }

        public void AtualizarDuracao(int novaDuracao)
        {
            if (novaDuracao <= 0)
                throw new ArgumentException("Duração deve ser maior que zero", nameof(novaDuracao));

            DuracaoMinutos = novaDuracao;
        }

        public void AtualizarOrdem(int novaOrdem)
        {
            if (novaOrdem <= 0)
                throw new ArgumentException("Ordem deve ser maior que zero", nameof(novaOrdem));

            Ordem = novaOrdem;
        }

        public void Inativar()
        {
            Ativa = false;
        }

        public void Ativar()
        {
            Ativa = true;
        }

        public void AssociarCurso(Guid cursoId)
        {
            if (cursoId == Guid.Empty)
                throw new ArgumentException("ID do curso é inválido", nameof(cursoId));

            CursoId = cursoId;
        }
    }
}