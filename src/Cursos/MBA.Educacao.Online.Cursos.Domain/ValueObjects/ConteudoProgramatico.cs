namespace MBA.Educacao.Online.Cursos.Domain.ValueObjects;

public class ConteudoProgramatico
{
    public string Ementa { get; private set; }
    public string Objetivo { get; private set; }
    public string Bibliografia { get; private set; }
    public string MaterialUrl { get; private set; }
    private ConteudoProgramatico() { }

    public ConteudoProgramatico(string ementa, string objetivo, string bibliografia, string materialUrl)
    {
        Ementa = ementa;
        Objetivo = objetivo;
        Bibliografia = bibliografia;
        MaterialUrl = materialUrl;

        ValidarConteudoProgramatico();
    }

    private void ValidarConteudoProgramatico()
    {
        if (string.IsNullOrWhiteSpace(Ementa))
            throw new ArgumentException("Ementa do conteúdo programático é obrigatório", nameof(Ementa));

        if (string.IsNullOrWhiteSpace(Objetivo))
            throw new ArgumentException("Objetivo do conteúdo programático é obrigatória", nameof(Objetivo));
        
        if (string.IsNullOrWhiteSpace(Bibliografia))
            throw new ArgumentException("Bibliografia do conteúdo programático é obrigatória", nameof(Bibliografia));

        if (string.IsNullOrWhiteSpace(MaterialUrl))
            throw new ArgumentException("Url do Material do conteúdo programático é obrigatória", nameof(MaterialUrl));
    }

    public void AtualizarEmenta(string ementa)
    {
        if (string.IsNullOrWhiteSpace(ementa))
            throw new ArgumentException("Ementa do conteúdo programático é obrigatório", nameof(Ementa));

        Ementa = ementa;
    }
    
    public void AtualizarObjetivo(string objetivo)
    {
        if (string.IsNullOrWhiteSpace(objetivo))
            throw new ArgumentException("Objetivo do conteúdo programático é obrigatório", nameof(Objetivo));

        Objetivo = objetivo;
    }
    
    public void AtualizarBibliografia(string bibliografia)
    {
        if (string.IsNullOrWhiteSpace(bibliografia))
            throw new ArgumentException("Bibliografia do conteúdo programático é obrigatório", nameof(Bibliografia));

        Bibliografia = bibliografia;
    }
    
    public void AtualizarMaterialUrl(string materialUrl)
    {
        if (string.IsNullOrWhiteSpace(materialUrl))
            throw new ArgumentException("Url do Material do conteúdo programático é obrigatório", nameof(MaterialUrl));

        MaterialUrl = materialUrl;
    }

    
}
