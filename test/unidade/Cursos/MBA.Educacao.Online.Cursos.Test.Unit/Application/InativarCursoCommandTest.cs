//using FluentAssertions;
//using MBA.Educacao.Online.Cursos.Application.Commands.Cursos;
//using MBA.Educacao.Online.Cursos.Domain.Enums;
//using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
//using Moq;

//namespace MBA.Educacao.Online.Cursos.Test.Unit.Application
//{
//    public class InativarCursoCommandTest
//    {
//        private readonly Mock<ICursoRepository> _cursoRepositoryMock;
//        private readonly InativarCursoCommandHandler _handler;

//        public InativarCursoCommandTest()
//        {
//            _cursoRepositoryMock = new Mock<ICursoRepository>();
//            _handler = new InativarCursoCommandHandler(_cursoRepositoryMock.Object);
//        }

//        #region Cenários de Sucesso

//        [Fact(DisplayName = "Inativar Curso - Curso Ativo - Deve Inativar com Sucesso")]
//        [Trait("Categoria", "Commands - Curso")]
//        public void Handle_CursoAtivo_DeveInativarComSucesso()
//        {
//            // Arrange
//            var cursoId = Guid.NewGuid();
//            var command = new InativarCursoCommand(cursoId);

//            var cursoAtivo = new Domain.Entities.Curso(
//                "MBA em Gestão",
//                "Descrição do curso",
//                NivelCurso.Basico
//            );

//            _cursoRepositoryMock
//                .Setup(x => x.GetByIdAsync(cursoId))
//                .ReturnsAsync(cursoAtivo);

//            // Act
//            var result = _handler.Handle(command, CancellationToken.None).Result;

//            // Assert
//            result.Should().NotBeNull();
//            result.IsSuccess.Should().BeTrue();
//            cursoAtivo.Ativo.Should().BeFalse();

//            _cursoRepositoryMock.Verify(x => x.GetByIdAsync(cursoId), Times.Once);
//            _cursoRepositoryMock.Verify(x => x.UpdateAsync(cursoAtivo), Times.Once);
//        }

//        #endregion

//        #region Cenários de Falha

//        [Fact(DisplayName = "Inativar Curso - Curso Não Encontrado - Deve Retornar Falha")]
//        [Trait("Categoria", "Commands - Curso")]
//        public void Handle_CursoNaoEncontrado_DeveRetornarFalha()
//        {
//            // Arrange
//            var cursoId = Guid.NewGuid();
//            var command = new InativarCursoCommand(cursoId);

//            _cursoRepositoryMock
//                .Setup(x => x.GetByIdAsync(cursoId))
//                .ReturnsAsync((Domain.Entities.Curso?)null);

//            // Act
//            var result = _handler.Handle(command, CancellationToken.None).Result;

//            // Assert
//            result.Should().NotBeNull();
//            result.IsSuccess.Should().BeFalse();
//            result.Error.Should().Be("Curso não encontrado.");

//            _cursoRepositoryMock.Verify(x => x.GetByIdAsync(cursoId), Times.Once);
//            _cursoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Domain.Entities.Curso>()), Times.Never);
//        }

//        [Fact(DisplayName = "Inativar Curso - Curso Já Inativo - Deve Retornar Falha")]
//        [Trait("Categoria", "Commands - Curso")]
//        public void Handle_CursoJaInativo_DeveRetornarFalha()
//        {
//            // Arrange
//            var cursoId = Guid.NewGuid();
//            var command = new InativarCursoCommand(cursoId);

//            var cursoInativo = new Domain.Entities.Curso(
//                "MBA em Gestão",
//                "Descrição do curso",
//                NivelCurso.Basico
//            );
//            cursoInativo.Inativar();

//            _cursoRepositoryMock
//                .Setup(x => x.GetByIdAsync(cursoId))
//                .ReturnsAsync(cursoInativo);

//            // Act
//            var result = _handler.Handle(command, CancellationToken.None).Result;

//            // Assert
//            result.Should().NotBeNull();
//            result.IsSuccess.Should().BeFalse();
//            result.Error.Should().Be("O curso já está inativo.");

//            _cursoRepositoryMock.Verify(x => x.GetByIdAsync(cursoId), Times.Once);
//            _cursoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Domain.Entities.Curso>()), Times.Never);
//        }

//        [Fact(DisplayName = "Inativar Curso - Erro no Repositório - Deve Retornar Falha")]
//        [Trait("Categoria", "Commands - Curso")]
//        public void Handle_ErroNoRepositorio_DeveRetornarFalha()
//        {
//            // Arrange
//            var cursoId = Guid.NewGuid();
//            var command = new InativarCursoCommand(cursoId);

//            var cursoAtivo = new Domain.Entities.Curso(
//                "MBA em Gestão",
//                "Descrição do curso",
//                NivelCurso.Basico
//            );

//            _cursoRepositoryMock
//                .Setup(x => x.GetByIdAsync(cursoId))
//                .ReturnsAsync(cursoAtivo);

//            _cursoRepositoryMock
//                .Setup(x => x.UpdateAsync(It.IsAny<Domain.Entities.Curso>()))
//                .ThrowsAsync(new Exception("Erro de conexão"));

//            // Act
//            var result = _handler.Handle(command, CancellationToken.None).Result;

//            // Assert
//            result.Should().NotBeNull();
//            result.IsSuccess.Should().BeFalse();
//            result.Error.Should().Contain("Erro interno ao inativar curso");
//        }

//        #endregion
//    }
//}


