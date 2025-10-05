namespace MBA.Educacao.Online.Curso.Domain.ValueObjects;

public class ConteudoProgramatico
{
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public int Ordem { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public bool Ativo { get; private set; }

    private ConteudoProgramatico() { }

    public ConteudoProgramatico(string titulo, string descricao, int ordem)
    {
        Titulo = titulo;
        Descricao = descricao;
        Ordem = ordem;
        DataCriacao = DateTime.UtcNow;
        Ativo = true;

        ValidarConteudoProgramatico();
    }

    private void ValidarConteudoProgramatico()
    {
        if (string.IsNullOrWhiteSpace(Titulo))
            throw new ArgumentException("Título do conteúdo programático é obrigatório", nameof(Titulo));

        if (string.IsNullOrWhiteSpace(Descricao))
            throw new ArgumentException("Descrição do conteúdo programático é obrigatória", nameof(Descricao));

        if (Ordem <= 0)
            throw new ArgumentException("Ordem deve ser maior que zero", nameof(Ordem));
    }

    public void AtualizarTitulo(string novoTitulo)
    {
        if (string.IsNullOrWhiteSpace(novoTitulo))
            throw new ArgumentException("Título do conteúdo programático é obrigatório", nameof(novoTitulo));

        Titulo = novoTitulo;
    }

    public void AtualizarDescricao(string novaDescricao)
    {
        if (string.IsNullOrWhiteSpace(novaDescricao))
            throw new ArgumentException("Descrição do conteúdo programático é obrigatória", nameof(novaDescricao));

        Descricao = novaDescricao;
    }

    public void AtualizarOrdem(int novaOrdem)
    {
        if (novaOrdem <= 0)
            throw new ArgumentException("Ordem deve ser maior que zero", nameof(novaOrdem));

        Ordem = novaOrdem;
    }

    public void Inativar()
    {
        Ativo = false;
    }

    public void Ativar()
    {
        Ativo = true;
    }
}
