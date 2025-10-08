using MBA.Educacao.Online.Curso.Application.Commands.Curso;
using MBA.Educacao.Online.Curso.Application.Events.Curso;
using MBA.Educacao.Online.Curso.Domain.Interfaces.Repositories;
using MediatR;

namespace MBA.Educacao.Online.Curso.Application.Handlers.Curso;

public class CriarCursoCommandHandler : IRequestHandler<CriarCursoCommand, bool>
{
    private readonly ICursoRepository _cursoRepository;
    private readonly IMediator  _mediator;

    public CriarCursoCommandHandler(ICursoRepository cursoRepository, IMediator mediator)
    {
        _cursoRepository = cursoRepository;
        _mediator = mediator;
    }

    public async Task<bool> Handle(CriarCursoCommand request, CancellationToken cancellationToken)
    {
        var curso = new Domain.Entities.Curso(request.Titulo, request.Descricao, request.Nivel);
        
        _cursoRepository.Adicionar(curso);

        await _mediator.Publish(new CriarCursoEvent(curso.Id, request.Titulo, request.Descricao), cancellationToken);
        
        return true;
    }

    // public async Task<Result<Guid>> Handle(CriarCursoCommand request, CancellationToken cancellationToken)
    // {
    //     try
    //     {
    //         // Verificar se já existe um curso com o mesmo título
    //         if (await _cursoRepository.TituloExistsAsync(request.Titulo))
    //         {
    //             return Result.Failure<Guid>("Já existe um curso com este título.");
    //         }
    //
    //         // Criar o curso
    //         var curso = new Domain.Entities.Curso(request.Titulo, request.Descricao, request.Nivel);
    //
    //         // Salvar no repositório
    //         var cursoSalvo = await _cursoRepository.AddAsync(curso);
    //
    //         return Result.Success(cursoSalvo.Id);
    //     }
    //     catch (ArgumentException ex)
    //     {
    //         return Result.Failure<Guid>(ex.Message);
    //     }
    //     catch (Exception ex)
    //     {
    //         return Result.Failure<Guid>($"Erro interno ao criar curso: {ex.Message}");
    //     }
    // }
}
