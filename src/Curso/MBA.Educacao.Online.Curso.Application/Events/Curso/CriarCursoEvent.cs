using MBA.Educacao.Online.Core.Domain.Messages;
using MediatR;

namespace MBA.Educacao.Online.Curso.Application.Events.Curso;

public class CriarCursoEvent : Event
{
    public Guid Id { get; private set; }
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }

    public CriarCursoEvent(Guid id, string titulo, string descricao)
    {
        Id = id;   
        Titulo = titulo;
        Descricao = descricao;
    }
    
}