//using MBA.Educacao.Online.Curso.Application.Commands.Curso;
//using MBA.Educacao.Online.Curso.Application.Common.Interfaces;
//using MBA.Educacao.Online.Curso.Application.Common.Models;
//using MediatR;

//namespace MBA.Educacao.Online.Cursos.Application.Handlers.Cursos

//public class AtualizarCursoCommandHandler : ICommandHandler<AtualizarCursoCommand>
//{
//    private readonly ICursoRepository _cursoRepository;

//    public AtualizarCursoCommandHandler(ICursoRepository cursoRepository)
//    {
//        _cursoRepository = cursoRepository;
//    }

//    public async Task<Result> Handle(AtualizarCursoCommand request, CancellationToken cancellationToken)
//    {
//        try
//        {
//            // Buscar o curso
//            var curso = await _cursoRepository.GetByIdAsync(request.Id);
//            if (curso == null)
//            {
//                return Result.Failure("Curso não encontrado.");
//            }

//            // Verificar se já existe outro curso com o mesmo título
//            if (await _cursoRepository.TituloExistsAsync(request.Titulo, request.Id))
//            {
//                return Result.Failure("Já existe outro curso com este título.");
//            }

//            // Atualizar informações do curso
//            curso.AtualizarInformacoes(request.Titulo, request.Descricao, request.Nivel);
            
//            // Atualizar no repositório
//            await _cursoRepository.UpdateAsync(curso);

//            return Result.Success();
//        }
//        catch (ArgumentException ex)
//        {
//            return Result.Failure(ex.Message);
//        }
//        catch (Exception ex)
//        {
//            return Result.Failure($"Erro interno ao atualizar curso: {ex.Message}");
//        }
//    }
//}
