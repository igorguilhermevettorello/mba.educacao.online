using MBA.Educacao.Online.Core.Domain.Interfaces;
using MBA.Educacao.Online.Core.Domain.Models;
using MBA.Educacao.Online.Cursos.Domain.Enums;
using MBA.Educacao.Online.Cursos.Domain.ValueObjects;

namespace MBA.Educacao.Online.Cursos.Domain.Entities
{
    public class Curso : Entity, IAggregateRoot
    {
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public string Instrutor { get; private set; }
        public NivelCurso Nivel { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public bool Ativo { get; private set; }
        private readonly List<Aula> _aulas;
        public IReadOnlyCollection<Aula> Aulas => _aulas.AsReadOnly();
        public ConteudoProgramatico ConteudoProgramatico { get; private set; }
        
        private Curso()
        {
            _aulas = new List<Aula>();
        }

        public Curso(string titulo, string descricao, string instrutor, NivelCurso nivel, decimal valor) : this()
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("Título do curso é obrigatório", nameof(titulo));

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição do curso é obrigatória", nameof(descricao));

            if (string.IsNullOrWhiteSpace(instrutor))
                throw new ArgumentException("Instrutor do curso é obrigatório", nameof(Instrutor));
            
            if (!Enum.IsDefined(typeof(NivelCurso), nivel))
                throw new ArgumentException("Nível do curso inválido", nameof(nivel));

            if (valor <= 0)
                throw new ArgumentException("Valor do curso deve ser maior que zero", nameof(valor));
            
            Titulo = titulo;
            Descricao = descricao;
            Instrutor = instrutor;
            Nivel = nivel;
            Valor = valor;
            DataCriacao = DateTime.UtcNow;
            Ativo = true;
        }

        public void AdicionarAula(Aula aula)
        {
            if (aula == null)
                throw new ArgumentNullException(nameof(aula));

            if (!Ativo)
                throw new InvalidOperationException("Não é possível adicionar aulas a um curso inativo");

            _aulas.Add(aula);
        }

        public bool VerificarSeAulaEstaCadastrada(Guid aulaId)
        {
            return _aulas.Any(a => a.Id == aulaId);
        }

        public void Inativar()
        {
            Ativo = false;
        }

        public void Ativar()
        {
            Ativo = true;
        }

        public void AtualizarNivel(NivelCurso novoNivel)
        {
            if (!Enum.IsDefined(typeof(NivelCurso), novoNivel))
                throw new ArgumentException("Nível do curso inválido", nameof(novoNivel));

            Nivel = novoNivel;
        }

        public void AtualizarInformacoes(string titulo, string descricao, string instrutor, NivelCurso nivel, decimal valor)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("Título do curso é obrigatório", nameof(titulo));

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição do curso é obrigatória", nameof(descricao));
            
            if (string.IsNullOrWhiteSpace(instrutor))
                throw new ArgumentException("Instrutor do curso é obrigatório", nameof(instrutor));

            if (!Enum.IsDefined(typeof(NivelCurso), nivel))
                throw new ArgumentException("Nível do curso inválido", nameof(nivel));
            
            if (valor <= 0)
                throw new ArgumentException("Valor do curso deve ser maior que zero", nameof(valor));
                    
            Titulo = titulo;
            Descricao = descricao;
            Instrutor = instrutor;
            Nivel = nivel;
            Valor = valor;
        }
        
        public void AdicionarConteudoProgramatico(ConteudoProgramatico conteudo)
        {
            if (conteudo == null)
                throw new ArgumentNullException(nameof(conteudo));

            ConteudoProgramatico = conteudo;
        }

    }
}