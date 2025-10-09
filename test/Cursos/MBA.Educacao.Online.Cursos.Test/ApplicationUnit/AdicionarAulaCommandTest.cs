//using MBA.Educacao.Online.Cursos.Application.Handlers.Aula;
//using MBA.Educacao.Online.Cursos.Domain.Interfaces.Repositories;
//using Moq;

//namespace MBA.Educacao.Online.Cursos.Test.Unit.Application
//{

//    public class AdicionarAulaCommandTest
//    {
//        private readonly Mock<ICursoRepository> _cursoRepositoryMock;
//        private readonly AdicionarAulaCommandHandler _handler;

//        public AdicionarAulaCommandTest()
//        {
//            _cursoRepositoryMock = new Mock<ICursoRepository>();
            
//        }

//        //#region Cenários de Sucesso

//        //[Fact(DisplayName = "Adicionar Aula - Dados Válidos - Deve Adicionar Aula com Sucesso")]
//        //[Trait("Categoria", "Commands - Aula")]
//        //public void AdicionarAula_DadosValidos_DeveAdicionarAulaComSucesso()
//        //{
//        //    // Arrange
//        //    var cursoId = Guid.NewGuid();
//        //    var command = new AdicionarAulaCommand(
//        //        cursoId,
//        //        "Introdução ao MBA",
//        //        "Primeira aula do curso",
//        //        60,
//        //        1
//        //    );

//        //    var cursoAtivo = new Domain.Entities.Curso(
//        //        "MBA em Gestão",
//        //        "Descrição do curso",
//        //        NivelCurso.Basico
//        //    );


//        //    var mocker = new AutoMocker();
//        //    var cursoHandler = mocker.CreateInstance<CriarCursoCommandHandler>();
//        //    var result = await cursoHandler.Handle(command, new CancellationToken());

//        //    // Assert
//        //    Assert.True(result);

//        //    mocker.GetMock<ICursoRepository>().Verify(r => r.Adicionar(It.IsAny<Domain.Entities.Curso>()), Times.Once);
//        //    mocker.GetMock<IMediator>().Verify(r => r.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);


//        //    // Act
//        //    var result = _handler.Handle(command, CancellationToken.None).Result;

//        //    // Assert
//        //    //result.Should().NotBeNull();
//        //    //result.IsSuccess.Should().BeTrue();
//        //    //result.Value.Should().NotBe(Guid.Empty);
//        //    //cursoAtivo.Aulas.Should().HaveCount(1);

//        //    //_cursoRepositoryMock.Verify(x => x.GetByIdAsync(cursoId), Times.Once);
//        //    //_cursoRepositoryMock.Verify(x => x.UpdateAsync(cursoAtivo), Times.Once);
//        //}

//        //#endregion

//        //#region Cenários de Falha

//        //[Fact(DisplayName = "Adicionar Aula - Curso Não Encontrado - Deve Retornar Falha")]
//        //[Trait("Categoria", "Commands - Aula")]
//        //public void Handle_CursoNaoEncontrado_DeveRetornarFalha()
//        //{
//        //    // Arrange
//        //    var cursoId = Guid.NewGuid();
//        //    var command = new AdicionarAulaCommand(
//        //        cursoId,
//        //        "Introdução ao MBA",
//        //        "Primeira aula do curso",
//        //        60,
//        //        1
//        //    );

//        //    _cursoRepositoryMock
//        //        .Setup(x => x.GetByIdAsync(cursoId))
//        //        .ReturnsAsync((Domain.Entities.Curso?)null);

//        //    // Act
//        //    var result = _handler.Handle(command, CancellationToken.None).Result;

//        //    // Assert
//        //    result.Should().NotBeNull();
//        //    result.IsSuccess.Should().BeFalse();
//        //    result.Error.Should().Be("Curso não encontrado.");

//        //    _cursoRepositoryMock.Verify(x => x.GetByIdAsync(cursoId), Times.Once);
//        //    _cursoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Domain.Entities.Curso>()), Times.Never);
//        //}

//        //[Fact(DisplayName = "Adicionar Aula - Curso Inativo - Deve Retornar Falha")]
//        //[Trait("Categoria", "Commands - Aula")]
//        //public void Handle_CursoInativo_DeveRetornarFalha()
//        //{
//        //    // Arrange
//        //    var cursoId = Guid.NewGuid();
//        //    var command = new AdicionarAulaCommand(
//        //        cursoId,
//        //        "Introdução ao MBA",
//        //        "Primeira aula do curso",
//        //        60,
//        //        1
//        //    );

//        //    var cursoInativo = new Domain.Entities.Curso(
//        //        "MBA em Gestão",
//        //        "Descrição do curso",
//        //        NivelCurso.Basico
//        //    );
//        //    cursoInativo.Inativar();

//        //    _cursoRepositoryMock
//        //        .Setup(x => x.GetByIdAsync(cursoId))
//        //        .ReturnsAsync(cursoInativo);

//        //    // Act
//        //    var result = _handler.Handle(command, CancellationToken.None).Result;

//        //    // Assert
//        //    result.Should().NotBeNull();
//        //    result.IsSuccess.Should().BeFalse();
//        //    result.Error.Should().Be("Não é possível adicionar aulas a um curso inativo.");

//        //    _cursoRepositoryMock.Verify(x => x.GetByIdAsync(cursoId), Times.Once);
//        //    _cursoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Domain.Entities.Curso>()), Times.Never);
//        //}

//        //[Theory(DisplayName = "Adicionar Aula - Dados Inválidos - Deve Retornar Falha")]
//        //[Trait("Categoria", "Commands - Aula")]
//        //[InlineData("", "Descrição válida", 60, 1, "Título da aula é obrigatório")]
//        //[InlineData(null, "Descrição válida", 60, 1, "Título da aula é obrigatório")]
//        //[InlineData("Título válido", "", 60, 1, "Descrição da aula é obrigatória")]
//        //[InlineData("Título válido", null, 60, 1, "Descrição da aula é obrigatória")]
//        //[InlineData("Título válido", "Descrição válida", 0, 1, "Duração deve ser maior que zero")]
//        //[InlineData("Título válido", "Descrição válida", -1, 1, "Duração deve ser maior que zero")]
//        //[InlineData("Título válido", "Descrição válida", 60, 0, "Ordem deve ser maior que zero")]
//        //[InlineData("Título válido", "Descrição válida", 60, -1, "Ordem deve ser maior que zero")]
//        //public void Handle_DadosInvalidos_DeveRetornarFalha(string titulo, string descricao, int duracao, int ordem, string mensagemEsperada)
//        //{
//        //    // Arrange
//        //    var cursoId = Guid.NewGuid();
//        //    var command = new AdicionarAulaCommand(cursoId, titulo, descricao, duracao, ordem);

//        //    var cursoAtivo = new Domain.Entities.Curso(
//        //        "MBA em Gestão",
//        //        "Descrição do curso",
//        //        NivelCurso.Basico
//        //    );

//        //    _cursoRepositoryMock
//        //        .Setup(x => x.GetByIdAsync(cursoId))
//        //        .ReturnsAsync(cursoAtivo);

//        //    // Act
//        //    var result = _handler.Handle(command, CancellationToken.None).Result;

//        //    // Assert
//        //    result.Should().NotBeNull();
//        //    result.IsSuccess.Should().BeFalse();
//        //    result.Error.Should().Be(mensagemEsperada);

//        //    _cursoRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Domain.Entities.Curso>()), Times.Never);
//        //}

//        //[Fact(DisplayName = "Adicionar Aula - Erro no Repositório - Deve Retornar Falha")]
//        //[Trait("Categoria", "Commands - Aula")]
//        //public void Handle_ErroNoRepositorio_DeveRetornarFalha()
//        //{
//        //    // Arrange
//        //    var cursoId = Guid.NewGuid();
//        //    var command = new AdicionarAulaCommand(
//        //        cursoId,
//        //        "Introdução ao MBA",
//        //        "Primeira aula do curso",
//        //        60,
//        //        1
//        //    );

//        //    var cursoAtivo = new Domain.Entities.Curso(
//        //        "MBA em Gestão",
//        //        "Descrição do curso",
//        //        NivelCurso.Basico
//        //    );

//        //    _cursoRepositoryMock
//        //        .Setup(x => x.GetByIdAsync(cursoId))
//        //        .ReturnsAsync(cursoAtivo);

//        //    _cursoRepositoryMock
//        //        .Setup(x => x.UpdateAsync(It.IsAny<Domain.Entities.Curso>()))
//        //        .ThrowsAsync(new Exception("Erro de conexão"));

//        //    // Act
//        //    var result = _handler.Handle(command, CancellationToken.None).Result;

//        //    // Assert
//        //    result.Should().NotBeNull();
//        //    result.IsSuccess.Should().BeFalse();
//        //    result.Error.Should().Contain("Erro interno ao adicionar aula");
//        //}

//        //#endregion
//    }
//}