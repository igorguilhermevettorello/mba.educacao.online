//using MBA.Educacao.Online.Cursos.Application.Commands.Cursos;
//using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;

//namespace MBA.Educacao.Online.Cursos.Application.Handlers.Cursos
//{
//    public class InativarCursoCommandHandler : ICommandHandler<InativarCursoCommand>
//    {
//        private readonly ICursoRepository _cursoRepository;

//        public InativarCursoCommandHandler(ICursoRepository cursoRepository)
//        {
//            _cursoRepository = cursoRepository;
//        }

//        public async Task<Result> Handle(InativarCursoCommand request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                // Buscar o curso
//                var curso = await _cursoRepository.GetByIdAsync(request.Id);
//                if (curso == null)
//                {
//                    return Result.Failure("Curso não encontrado.");
//                }

//                // Verificar se já está inativo
//                if (!curso.Ativo)
//                {
//                    return Result.Failure("O curso já está inativo.");
//                }

//                // Inativar o curso
//                curso.Inativar();

//                // Atualizar no repositório
//                await _cursoRepository.UpdateAsync(curso);

//                return Result.Success();
//            }
//            catch (Exception ex)
//            {
//                return Result.Failure($"Erro interno ao inativar curso: {ex.Message}");
//            }
//        }
//    }
//}