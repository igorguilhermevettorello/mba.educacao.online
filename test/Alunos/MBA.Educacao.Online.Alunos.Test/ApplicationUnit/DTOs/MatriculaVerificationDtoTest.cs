using FluentAssertions;
using MBA.Educacao.Online.Alunos.Application.DTOs;

namespace MBA.Educacao.Online.Alunos.Test.ApplicationUnit.DTOs
{
    public class MatriculaVerificationDtoTest
    {
        #region 1. Criar DTO - Construtor Completo

        [Fact(DisplayName = "Criar MatriculaVerificationDto - Com Todos os Parâmetros - Deve Criar com Sucesso")]
        [Trait("Alunos", "DTOs - MatriculaVerification")]
        public void CriarMatriculaVerificationDto_ComTodosParametros_DeveCriarComSucesso()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var estaMatriculado = true;
            var matriculaAtiva = true;
            var dataMatricula = DateTime.Now.AddMonths(-6);

            // Act
            var dto = new MatriculaVerificationDto(cursoId, estaMatriculado, matriculaAtiva, dataMatricula);

            // Assert
            dto.Should().NotBeNull();
            dto.CursoId.Should().Be(cursoId);
            dto.EstaMatriculado.Should().BeTrue();
            dto.MatriculaAtiva.Should().BeTrue();
            dto.DataMatricula.Should().Be(dataMatricula);
        }

        [Fact(DisplayName = "Criar MatriculaVerificationDto - Sem Data Matrícula - Deve Criar com Data Nula")]
        [Trait("Alunos", "DTOs - MatriculaVerification")]
        public void CriarMatriculaVerificationDto_SemDataMatricula_DeveCriarComDataNula()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var estaMatriculado = false;
            var matriculaAtiva = false;

            // Act
            var dto = new MatriculaVerificationDto(cursoId, estaMatriculado, matriculaAtiva);

            // Assert
            dto.Should().NotBeNull();
            dto.CursoId.Should().Be(cursoId);
            dto.EstaMatriculado.Should().BeFalse();
            dto.MatriculaAtiva.Should().BeFalse();
            dto.DataMatricula.Should().BeNull();
        }

        #endregion

        #region 2. Cenários de Matrícula Ativa

        [Fact(DisplayName = "Criar DTO - Aluno Matriculado e Ativo - Deve Ter Status Correto")]
        [Trait("Alunos", "DTOs - MatriculaVerification")]
        public void CriarDto_AlunoMatriculadoEAtivo_DeveTerStatusCorreto()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var dataMatricula = DateTime.Now.AddMonths(-3);

            // Act
            var dto = new MatriculaVerificationDto(cursoId, true, true, dataMatricula);

            // Assert
            dto.EstaMatriculado.Should().BeTrue();
            dto.MatriculaAtiva.Should().BeTrue();
            dto.DataMatricula.Should().NotBeNull();
            dto.DataMatricula.Should().BeBefore(DateTime.Now);
        }

        [Fact(DisplayName = "Criar DTO - Aluno Matriculado Mas Inativo - Deve Ter Status Correto")]
        [Trait("Alunos", "DTOs - MatriculaVerification")]
        public void CriarDto_AlunoMatriculadoMasInativo_DeveTerStatusCorreto()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var dataMatricula = DateTime.Now.AddMonths(-3);

            // Act
            var dto = new MatriculaVerificationDto(cursoId, true, false, dataMatricula);

            // Assert
            dto.EstaMatriculado.Should().BeTrue();
            dto.MatriculaAtiva.Should().BeFalse();
            dto.DataMatricula.Should().NotBeNull();
        }

        [Fact(DisplayName = "Criar DTO - Aluno Não Matriculado - Deve Ter Status Correto")]
        [Trait("Alunos", "DTOs - MatriculaVerification")]
        public void CriarDto_AlunoNaoMatriculado_DeveTerStatusCorreto()
        {
            // Arrange
            var cursoId = Guid.NewGuid();

            // Act
            var dto = new MatriculaVerificationDto(cursoId, false, false, null);

            // Assert
            dto.EstaMatriculado.Should().BeFalse();
            dto.MatriculaAtiva.Should().BeFalse();
            dto.DataMatricula.Should().BeNull();
        }

        #endregion

        #region 3. Propriedades Mutáveis

        [Fact(DisplayName = "Modificar Propriedades - Deve Permitir Alteração")]
        [Trait("Alunos", "DTOs - MatriculaVerification")]
        public void ModificarPropriedades_DevePermitirAlteracao()
        {
            // Arrange
            var dto = new MatriculaVerificationDto(Guid.NewGuid(), false, false);

            // Act
            dto.EstaMatriculado = true;
            dto.MatriculaAtiva = true;
            dto.DataMatricula = DateTime.Now;
            var novoCursoId = Guid.NewGuid();
            dto.CursoId = novoCursoId;

            // Assert
            dto.EstaMatriculado.Should().BeTrue();
            dto.MatriculaAtiva.Should().BeTrue();
            dto.DataMatricula.Should().NotBeNull();
            dto.CursoId.Should().Be(novoCursoId);
        }

        #endregion

        #region 4. Validação de Curso ID

        [Fact(DisplayName = "Criar DTO - Curso ID Vazio - Deve Aceitar")]
        [Trait("Alunos", "DTOs - MatriculaVerification")]
        public void CriarDto_CursoIdVazio_DeveAceitar()
        {
            // Arrange
            var cursoId = Guid.Empty;

            // Act
            var dto = new MatriculaVerificationDto(cursoId, false, false);

            // Assert
            dto.Should().NotBeNull();
            dto.CursoId.Should().Be(Guid.Empty);
        }

        #endregion

        #region 5. Cenários de Data

        [Fact(DisplayName = "Criar DTO - Data Matrícula Futura - Deve Aceitar")]
        [Trait("Alunos", "DTOs - MatriculaVerification")]
        public void CriarDto_DataMatriculaFutura_DeveAceitar()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var dataFutura = DateTime.Now.AddDays(10);

            // Act
            var dto = new MatriculaVerificationDto(cursoId, true, true, dataFutura);

            // Assert
            dto.DataMatricula.Should().Be(dataFutura);
            dto.DataMatricula.Should().BeAfter(DateTime.Now);
        }

        [Fact(DisplayName = "Criar DTO - Data Matrícula Antiga - Deve Aceitar")]
        [Trait("Alunos", "DTOs - MatriculaVerification")]
        public void CriarDto_DataMatriculaAntiga_DeveAceitar()
        {
            // Arrange
            var cursoId = Guid.NewGuid();
            var dataAntiga = DateTime.Now.AddYears(-2);

            // Act
            var dto = new MatriculaVerificationDto(cursoId, true, false, dataAntiga);

            // Assert
            dto.DataMatricula.Should().Be(dataAntiga);
            dto.DataMatricula.Should().BeBefore(DateTime.Now);
        }

        #endregion

        #region 6. Cenários de Integração

        [Fact(DisplayName = "Cenário Completo - Verificação de Matrícula Válida - Deve Criar DTO Completo")]
        [Trait("Alunos", "DTOs - MatriculaVerification")]
        public void CenarioCompleto_VerificacaoMatriculaValida_DeveCriarDtoCompleto()
        {
            // Arrange
            var cursoId = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d");
            var estaMatriculado = true;
            var matriculaAtiva = true;
            var dataMatricula = new DateTime(2024, 6, 15, 10, 30, 0);

            // Act
            var dto = new MatriculaVerificationDto(cursoId, estaMatriculado, matriculaAtiva, dataMatricula);

            // Assert
            dto.CursoId.Should().Be(cursoId);
            dto.EstaMatriculado.Should().BeTrue();
            dto.MatriculaAtiva.Should().BeTrue();
            dto.DataMatricula.Should().Be(dataMatricula);
            dto.DataMatricula.Should().NotBeNull();
            dto.DataMatricula!.Value.Year.Should().Be(2024);
            dto.DataMatricula!.Value.Month.Should().Be(6);
            dto.DataMatricula!.Value.Day.Should().Be(15);
        }

        [Fact(DisplayName = "Cenário Completo - Verificação de Não Matriculado - Deve Criar DTO Correto")]
        [Trait("Alunos", "DTOs - MatriculaVerification")]
        public void CenarioCompleto_VerificacaoNaoMatriculado_DeveCriarDtoCorreto()
        {
            // Arrange
            var cursoId = Guid.NewGuid();

            // Act
            var dto = new MatriculaVerificationDto(cursoId, false, false, null);

            // Assert
            dto.CursoId.Should().Be(cursoId);
            dto.EstaMatriculado.Should().BeFalse();
            dto.MatriculaAtiva.Should().BeFalse();
            dto.DataMatricula.Should().BeNull();
        }

        #endregion

        #region 7. Múltiplas Instâncias

        [Fact(DisplayName = "Criar Múltiplas Instâncias - Deve Ter Valores Independentes")]
        [Trait("Alunos", "DTOs - MatriculaVerification")]
        public void CriarMultiplasInstancias_DeveTerValoresIndependentes()
        {
            // Arrange & Act
            var dto1 = new MatriculaVerificationDto(Guid.NewGuid(), true, true, DateTime.Now);
            var dto2 = new MatriculaVerificationDto(Guid.NewGuid(), false, false, null);
            var dto3 = new MatriculaVerificationDto(Guid.NewGuid(), true, false, DateTime.Now.AddMonths(-1));

            // Assert
            dto1.CursoId.Should().NotBe(dto2.CursoId);
            dto1.CursoId.Should().NotBe(dto3.CursoId);
            dto2.CursoId.Should().NotBe(dto3.CursoId);

            dto1.EstaMatriculado.Should().BeTrue();
            dto2.EstaMatriculado.Should().BeFalse();
            dto3.EstaMatriculado.Should().BeTrue();

            dto1.MatriculaAtiva.Should().BeTrue();
            dto2.MatriculaAtiva.Should().BeFalse();
            dto3.MatriculaAtiva.Should().BeFalse();
        }

        #endregion
    }
}

