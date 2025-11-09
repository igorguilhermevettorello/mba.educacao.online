using FluentAssertions;
using FluentValidation;
using MBA.Educacao.Online.Alunos.Application;
using MBA.Educacao.Online.Alunos.Application.Commands;
using MBA.Educacao.Online.Alunos.Application.Interfaces;
using MBA.Educacao.Online.Alunos.Application.Services;
using MBA.Educacao.Online.Alunos.Application.Validators;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MBA.Educacao.Online.Alunos.Test.ApplicationUnit
{
    public class DependencyInjectionTest
    {
        #region 1. Registro Básico

        [Fact(DisplayName = "AddAlunosApplication - Deve Retornar ServiceCollection")]
        [Trait("Alunos", "DependencyInjection")]
        public void AddAlunosApplication_DeveRetornarServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddAlunosApplication();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeSameAs(services);
        }

        [Fact(DisplayName = "AddAlunosApplication - Deve Registrar Serviços")]
        [Trait("Alunos", "DependencyInjection")]
        public void AddAlunosApplication_DeveRegistrarServicos()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddAlunosApplication();

            // Assert
            services.Should().NotBeEmpty();
            services.Count.Should().BeGreaterThan(0);
        }

        #endregion

        #region 2. Registro de MediatR

        //[Fact(DisplayName = "AddAlunosApplication - Deve Registrar MediatR")]
        //[Trait("Alunos", "DependencyInjection - MediatR")]
        //public void AddAlunosApplication_DeveRegistrarMediatR()
        //{
        //    // Arrange
        //    var services = new ServiceCollection();
        //    services.AddLogging(); // Adicionar logger necessário para MediatR
        //    services.AddAlunosApplication();

        //    // Act
        //    var serviceProvider = services.BuildServiceProvider();
        //    var mediator = serviceProvider.GetService<IMediator>();

        //    // Assert
        //    mediator.Should().NotBeNull();
        //}

        #endregion

        #region 3. Registro de FluentValidation

        //[Fact(DisplayName = "AddAlunosApplication - Deve Registrar FluentValidation")]
        //[Trait("Alunos", "DependencyInjection - FluentValidation")]
        //public void AddAlunosApplication_DeveRegistrarFluentValidation()
        //{
        //    // Arrange
        //    var services = new ServiceCollection();
        //    services.AddAlunosApplication();

        //    // Act
        //    var validatorDescriptors = services.Where(s =>
        //        s.ServiceType.IsGenericType &&
        //        s.ServiceType.GetGenericTypeDefinition() == typeof(IValidator<>)).ToList();

        //    // Assert
        //    validatorDescriptors.Should().NotBeEmpty();
        //    validatorDescriptors.Count.Should().BeGreaterOrEqualTo(4);
        //}

        [Fact(DisplayName = "AddAlunosApplication - Deve Registrar Validators Específicos")]
        [Trait("Alunos", "DependencyInjection - FluentValidation")]
        public void AddAlunosApplication_DeveRegistrarValidatorsEspecificos()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddAlunosApplication();

            // Act
            var serviceProvider = services.BuildServiceProvider();
            var iniciarEstudoValidator = serviceProvider.GetService<IValidator<IniciarEstudoAulaCommand>>();
            var finalizarEstudoValidator = serviceProvider.GetService<IValidator<FinalizarEstudoAulaCommand>>();
            var processarMatriculaValidator = serviceProvider.GetService<IValidator<ProcessarMatriculaCommand>>();
            var acessarEstudoValidator = serviceProvider.GetService<IValidator<AcessarEstudoAulaCommand>>();

            // Assert
            iniciarEstudoValidator.Should().NotBeNull();
            iniciarEstudoValidator.Should().BeOfType<IniciarEstudoAulaCommandValidator>();

            finalizarEstudoValidator.Should().NotBeNull();
            finalizarEstudoValidator.Should().BeOfType<FinalizarEstudoAulaCommandValidator>();

            processarMatriculaValidator.Should().NotBeNull();
            processarMatriculaValidator.Should().BeOfType<ProcessarMatriculaCommandValidator>();

            acessarEstudoValidator.Should().NotBeNull();
        }

        #endregion

        #region 4. Registro de Serviços de Aplicação

        [Fact(DisplayName = "AddAlunosApplication - Deve Registrar IMatriculaService como Scoped")]
        [Trait("Alunos", "DependencyInjection - Services")]
        public void AddAlunosApplication_DeveRegistrarIMatriculaServiceComoScoped()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddAlunosApplication();

            // Act
            var descriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IMatriculaService));

            // Assert
            descriptor.Should().NotBeNull();
            descriptor!.Lifetime.Should().Be(ServiceLifetime.Scoped);
            descriptor.ImplementationType.Should().Be(typeof(MatriculaService));
        }

        #endregion

        #region 5. Verificação de Tipos de Serviços

        [Fact(DisplayName = "AddAlunosApplication - Deve Registrar Tipos Corretos de Validators")]
        [Trait("Alunos", "DependencyInjection - Validators")]
        public void AddAlunosApplication_DeveRegistrarTiposCorretosDeValidators()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddAlunosApplication();

            // Act
            var validatorDescriptors = services.Where(s =>
                s.ServiceType.IsGenericType &&
                s.ServiceType.GetGenericTypeDefinition() == typeof(IValidator<>)).ToList();

            // Assert
            validatorDescriptors.Should().NotBeEmpty();
            validatorDescriptors.Count.Should().BeGreaterOrEqualTo(4); // Pelo menos 4 validators
        }

        #endregion

        #region 6. Múltiplas Chamadas

        [Fact(DisplayName = "AddAlunosApplication - Múltiplas Chamadas - Deve Adicionar Serviços Múltiplas Vezes")]
        [Trait("Alunos", "DependencyInjection")]
        public void AddAlunosApplication_MultiplasChamadas_DeveAdicionarServicosMultiplasVezes()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddAlunosApplication();
            var countPrimeiraChamada = services.Count;

            services.AddAlunosApplication();
            var countSegundaChamada = services.Count;

            // Assert
            countSegundaChamada.Should().BeGreaterThan(countPrimeiraChamada);
        }

        #endregion

        #region 7. Verificação de Assembly

        [Fact(DisplayName = "AddAlunosApplication - Deve Registrar Serviços do Assembly Correto")]
        [Trait("Alunos", "DependencyInjection - Assembly")]
        public void AddAlunosApplication_DeveRegistrarServicosDoAssemblyCorreto()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddAlunosApplication();

            // Act
            var serviceProvider = services.BuildServiceProvider();
            var validator = serviceProvider.GetService<IValidator<IniciarEstudoAulaCommand>>();

            // Assert
            validator.Should().NotBeNull();
            validator!.GetType().Assembly.GetName().Name.Should().Be("MBA.Educacao.Online.Alunos.Application");
        }

        #endregion

        #region 8. Verificação de Lifetime

        //[Fact(DisplayName = "AddAlunosApplication - Validators Devem Ser Registrados Com Lifetime Adequado")]
        //[Trait("Alunos", "DependencyInjection - Lifetime")]
        //public void AddAlunosApplication_ValidatorsDevemSerRegistradosComLifetimeAdequado()
        //{
        //    // Arrange
        //    var services = new ServiceCollection();
        //    services.AddAlunosApplication();

        //    // Act
        //    var validatorDescriptors = services.Where(s =>
        //        s.ServiceType.IsGenericType &&
        //        s.ServiceType.GetGenericTypeDefinition() == typeof(IValidator<>)).ToList();

        //    // Assert
        //    validatorDescriptors.Should().NotBeEmpty();
        //    // FluentValidation por padrão registra como Scoped
        //    validatorDescriptors.Should().OnlyContain(d => d.Lifetime == ServiceLifetime.Scoped);
        //}

        #endregion

        #region 9. Verificação de Handlers Registrados

        [Fact(DisplayName = "AddAlunosApplication - Deve Registrar Pelo Menos Um Handler")]
        [Trait("Alunos", "DependencyInjection - Handlers")]
        public void AddAlunosApplication_DeveRegistrarPeloMenosUmHandler()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddAlunosApplication();

            // Act
            var handlerDescriptors = services.Where(s =>
                s.ServiceType.IsGenericType &&
                s.ServiceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)).ToList();

            // Assert
            handlerDescriptors.Should().NotBeEmpty();
            handlerDescriptors.Count.Should().BeGreaterOrEqualTo(4); // Pelo menos 4 handlers
        }

        #endregion

        #region 10. Teste de Não Duplicação

        [Fact(DisplayName = "AddAlunosApplication - Mesma ServiceCollection - Deve Permitir Múltiplos Registros")]
        [Trait("Alunos", "DependencyInjection")]
        public void AddAlunosApplication_MesmaServiceCollection_DevePermitirMultiplosRegistros()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result1 = services.AddAlunosApplication();
            var result2 = services.AddAlunosApplication();

            // Assert
            result1.Should().BeSameAs(services);
            result2.Should().BeSameAs(services);
            services.Should().NotBeEmpty();
        }

        #endregion
    }
}
