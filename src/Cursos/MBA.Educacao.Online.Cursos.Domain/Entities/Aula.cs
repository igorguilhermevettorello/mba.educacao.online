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

        private ConteudoProgramatico ConteudoProgramatico;
        
        private Aula() { }

        public Aula(string titulo, string descricao, int duracaoMinutos, int ordem)
        {
            Id = Guid.NewGuid();
            Titulo = titulo;
            Descricao = descricao;
            DuracaoMinutos = duracaoMinutos;
            Ordem = ordem;
            DataCriacao = DateTime.UtcNow;
            Ativa = true;

            ValidarAula();
        }

        private void ValidarAula()
        {
            if (string.IsNullOrWhiteSpace(Titulo))
                throw new ArgumentException("Título da aula é obrigatório", nameof(Titulo));

            if (string.IsNullOrWhiteSpace(Descricao))
                throw new ArgumentException("Descrição da aula é obrigatória", nameof(Descricao));

            if (DuracaoMinutos <= 0)
                throw new ArgumentException("Duração deve ser maior que zero", nameof(DuracaoMinutos));

            if (Ordem <= 0)
                throw new ArgumentException("Ordem deve ser maior que zero", nameof(Ordem));
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

        public void AdicionarConteudoProgramatico(ConteudoProgramatico conteudo)
        {
            if (conteudo == null)
                throw new ArgumentNullException(nameof(conteudo));

            ConteudoProgramatico = conteudo;
        }
    }
}