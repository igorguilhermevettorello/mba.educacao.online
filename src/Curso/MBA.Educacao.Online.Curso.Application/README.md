# MBA.Educacao.Online.Curso.Application

## Implementação CQRS para Gerenciamento de Cursos

Este projeto implementa o padrão CQRS (Command Query Responsibility Segregation) para o gerenciamento de cursos, utilizando MediatR e FluentValidation.

## Arquitetura

### Estrutura de Pastas

```
Application/
├── Commands/                    # Commands para operações de escrita
│   ├── Curso/                  # Commands relacionados a cursos
│   ├── Aula/                   # Commands relacionados a aulas
│   └── ConteudoProgramatico/   # Commands relacionados a conteúdo programático
├── Handlers/                   # Handlers para processar Commands
│   ├── Curso/                  # Handlers de curso
│   ├── Aula/                   # Handlers de aula
│   └── ConteudoProgramatico/   # Handlers de conteúdo programático
├── Queries/                    # Queries para operações de leitura (futuro)
├── Common/                     # Interfaces e modelos comuns
│   ├── Interfaces/            # Interfaces base e repositórios
│   ├── Models/                # Modelos de resultado e DTOs
│   └── Implementations/       # Implementações mock para desenvolvimento
├── Validators/                # Validadores FluentValidation
├── Examples/                  # Exemplos de uso
└── DependencyInjection.cs     # Configuração de DI
```

## Commands Implementados

### Curso
- **CriarCursoCommand**: Cria um novo curso
- **AtualizarCursoCommand**: Atualiza informações de um curso existente
- **InativarCursoCommand**: Inativa um curso
- **AtivarCursoCommand**: Ativa um curso

### Aula
- **AdicionarAulaCommand**: Adiciona uma nova aula a um curso

### Conteúdo Programático
- **AdicionarConteudoProgramaticoCommand**: Adiciona conteúdo programático a um curso

## Como Usar

### 1. Configuração de Dependências

```csharp
// Program.cs ou Startup.cs
services.AddApplication();
```

### 2. Uso Básico de Commands

```csharp
public class CursoController : ControllerBase
{
    private readonly IMediator _mediator;

    public CursoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CriarCurso(CriarCursoRequest request)
    {
        var command = new CriarCursoCommand(
            request.Titulo,
            request.Descricao,
            request.Nivel
        );

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }
}
```

### 3. Tratamento de Erros

```csharp
var result = await _mediator.Send(command);

if (!result.IsSuccess)
{
    // Tratar erro específico
    return BadRequest(new { error = result.Error });
}
```

## Padrões Implementados

### 1. Result Pattern
Todos os Commands retornam um `Result<T>` que encapsula o sucesso/falha da operação:

```csharp
public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }
    public T Value { get; }
}
```

### 2. Command Pattern
Cada operação de escrita é representada por um Command:

```csharp
public record CriarCursoCommand(
    string Titulo,
    string Descricao,
    NivelCurso Nivel
) : ICommand<Guid>;
```

### 3. Handler Pattern
Cada Command tem seu Handler correspondente na pasta `Handlers/`:

```csharp
// Commands/CriarCursoCommand.cs
public record CriarCursoCommand(string Titulo, string Descricao, NivelCurso Nivel) : ICommand<Guid>;

// Handlers/Curso/CriarCursoCommandHandler.cs
public class CriarCursoCommandHandler : ICommandHandler<CriarCursoCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CriarCursoCommand request, CancellationToken cancellationToken)
    {
        // Lógica de negócio
    }
}
```

### 4. Validation Pattern
Validações são feitas usando FluentValidation:

```csharp
public class CriarCursoCommandValidator : AbstractValidator<CriarCursoCommand>
{
    public CriarCursoCommandValidator()
    {
        RuleFor(x => x.Titulo).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Descricao).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.Nivel).IsInEnum();
    }
}
```

## Testes

Todos os Commands possuem testes unitários completos na pasta `test/unidade/Curso/MBA.Educacao.Online.Curso.Test.Unit/Commands/`.

### Exemplo de Teste

```csharp
[Fact(DisplayName = "Criar Curso - Dados Válidos - Deve Criar Curso com Sucesso")]
[Trait("Categoria", "Commands - Curso")]
public void Handle_DadosValidos_DeveCriarCursoComSucesso()
{
    // Arrange
    var command = new CriarCursoCommand("MBA", "Descrição", NivelCurso.Basico);
    
    // Act
    var result = _handler.Handle(command, CancellationToken.None).Result;
    
    // Assert
    result.IsSuccess.Should().BeTrue();
    result.Value.Should().NotBe(Guid.Empty);
}
```

## Próximos Passos

1. **Implementar Queries**: Criar Queries para operações de leitura
2. **Implementar Repository Real**: Substituir MockCursoRepository por implementação com Entity Framework
3. **Adicionar Eventos**: Implementar Domain Events para notificações
4. **Adicionar Logging**: Implementar logging estruturado
5. **Adicionar Cache**: Implementar cache para queries frequentes

## Dependências

- **MediatR**: Para implementação do padrão Mediator
- **FluentValidation**: Para validação de Commands
- **FluentAssertions**: Para testes mais legíveis
- **Moq**: Para mocking em testes
- **xUnit**: Framework de testes

## Princípios SOLID Aplicados

- **SRP**: Cada Command tem uma única responsabilidade
- **OCP**: Fácil extensão com novos Commands
- **LSP**: Interfaces bem definidas
- **ISP**: Interfaces segregadas por responsabilidade
- **DIP**: Dependência de abstrações, não de implementações
