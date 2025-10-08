using MBA.Educacao.Online.Core.Domain.Models;
using MBA.Educacao.Online.Curso.Domain.Enums;
using MBA.Educacao.Online.Curso.Domain.ValueObjects;

namespace MBA.Educacao.Online.Curso.Domain.Entities;

public class Curso: Entity, IAggregateRoot
{
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public NivelCurso Nivel { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public bool Ativo { get; private set; }
    
    private readonly List<Aula> _aulas;
    public IReadOnlyCollection<Aula> Aulas => _aulas.AsReadOnly();
    private readonly List<ConteudoProgramatico> _conteudosProgramaticos;
    public IReadOnlyCollection<ConteudoProgramatico> ConteudosProgramaticos => _conteudosProgramaticos.AsReadOnly();

    private Curso() 
    { 
        _aulas = new List<Aula>();
        _conteudosProgramaticos = new List<ConteudoProgramatico>();
    }

    public Curso(string titulo, string descricao, NivelCurso nivel) : this()
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("Título do curso é obrigatório", nameof(titulo));

        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição do curso é obrigatória", nameof(descricao));

        if (!Enum.IsDefined(typeof(NivelCurso), nivel))
            throw new ArgumentException("Nível do curso inválido", nameof(nivel));

        Id = Guid.NewGuid();
        Titulo = titulo;
        Descricao = descricao;
        Nivel = nivel;
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

    public void AdicionarConteudoProgramatico(ConteudoProgramatico conteudo)
    {
        if (conteudo == null)
            throw new ArgumentNullException(nameof(conteudo));

        if (!Ativo)
            throw new InvalidOperationException("Não é possível adicionar conteúdo programático a um curso inativo");

        _conteudosProgramaticos.Add(conteudo);
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

    public void AtualizarInformacoes(string titulo, string descricao, NivelCurso nivel)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("Título do curso é obrigatório", nameof(titulo));

        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição do curso é obrigatória", nameof(descricao));

        if (!Enum.IsDefined(typeof(NivelCurso), nivel))
            throw new ArgumentException("Nível do curso inválido", nameof(nivel));

        Titulo = titulo;
        Descricao = descricao;
        Nivel = nivel;
    }
}
