using MBA.Educacao.Online.Cursos.Data.Context;
using MBA.Educacao.Online.Cursos.Domain.Entities;
using MBA.Educacao.Online.Cursos.Domain.Enums;
using MBA.Educacao.Online.Cursos.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MBA.Educacao.Online.Cursos.Data.Seeds
{
    public static class SeederAplicacao
    {
        public static async Task EnsureSeedCursosAulas(CursoContext cursoContext, string env)
        {
            try
            {
                // Verifica se já existem cursos cadastrados
                if (await cursoContext.Cursos.AnyAsync())
                {
                    return; // Já existem dados, não precisa popular
                }

                // Curso 1: Fundamentos de C# 
                var cursoCSharp = new Curso(
                    "Fundamentos de C# e .NET",
                    "Aprenda os fundamentos da linguagem C# e do ecossistema .NET para desenvolver aplicações modernas e robustas.",
                    "Dr. João Silva",
                    NivelCurso.Basico,
                    299.90m
                );

                var conteudoCSharp = new ConteudoProgramatico(
                    "Introdução ao C#, tipos de dados, estruturas de controle, orientação a objetos, exceções, coleções e LINQ.",
                    "Capacitar o aluno a desenvolver aplicações básicas utilizando C# e compreender os conceitos fundamentais da plataforma .NET.",
                    "Microsoft .NET Documentation, C# Programming Guide, Clean Code by Robert C. Martin",
                    "https://docs.microsoft.com/dotnet"
                );
                cursoCSharp.AdicionarConteudoProgramatico(conteudoCSharp);

                cursoCSharp.AdicionarAula(new Aula(
                    "Introdução ao C# e .NET",
                    "Visão geral da linguagem C#, história do .NET e configuração do ambiente de desenvolvimento.",
                    45,
                    1
                ));

                cursoCSharp.AdicionarAula(new Aula(
                    "Tipos de Dados e Variáveis",
                    "Aprenda sobre tipos primitivos, variáveis, constantes e conversões de tipos em C#.",
                    60,
                    2
                ));

                cursoCSharp.AdicionarAula(new Aula(
                    "Estruturas de Controle",
                    "Estruturas condicionais (if, switch) e estruturas de repetição (for, while, foreach).",
                    55,
                    3
                ));

                cursoCSharp.AdicionarAula(new Aula(
                    "Orientação a Objetos em C#",
                    "Classes, objetos, herança, polimorfismo, encapsulamento e abstração.",
                    90,
                    4
                ));

                cursoCSharp.AdicionarAula(new Aula(
                    "Coleções e LINQ",
                    "Trabalhando com listas, dicionários, arrays e consultas com LINQ.",
                    75,
                    5
                ));

                await cursoContext.Cursos.AddAsync(cursoCSharp);

                // Curso 2: ASP.NET Core
                var cursoAspNet = new Curso(
                    "Desenvolvimento Web com ASP.NET Core",
                    "Crie aplicações web modernas e escaláveis utilizando ASP.NET Core MVC e Web API.",
                    "Profa. Maria Santos",
                    NivelCurso.Intermediario,
                    499.90m
                );

                var conteudoAspNet = new ConteudoProgramatico(
                    "ASP.NET Core MVC, Web API, Entity Framework Core, autenticação, autorização, middleware e deployment.",
                    "Desenvolver aplicações web completas utilizando as melhores práticas de desenvolvimento com ASP.NET Core.",
                    "ASP.NET Core Documentation, Pro ASP.NET Core by Adam Freeman, Building Microservices by Sam Newman",
                    "https://docs.microsoft.com/aspnet/core"
                );
                cursoAspNet.AdicionarConteudoProgramatico(conteudoAspNet);

                cursoAspNet.AdicionarAula(new Aula(
                    "Introdução ao ASP.NET Core",
                    "Arquitetura do ASP.NET Core, pipeline de requisições e configuração inicial do projeto.",
                    50,
                    1
                ));

                cursoAspNet.AdicionarAula(new Aula(
                    "ASP.NET Core MVC",
                    "Padrão MVC, Controllers, Views, Models e Razor Pages.",
                    80,
                    2
                ));

                cursoAspNet.AdicionarAula(new Aula(
                    "Web API RESTful",
                    "Criando APIs RESTful com ASP.NET Core, verbos HTTP e serialização JSON.",
                    70,
                    3
                ));

                cursoAspNet.AdicionarAula(new Aula(
                    "Entity Framework Core",
                    "ORM, Code First, Migrations, relacionamentos e consultas eficientes.",
                    85,
                    4
                ));

                cursoAspNet.AdicionarAula(new Aula(
                    "Autenticação e Autorização",
                    "Identity, JWT, políticas de autorização e segurança em APIs.",
                    90,
                    5
                ));

                cursoAspNet.AdicionarAula(new Aula(
                    "Deployment e Boas Práticas",
                    "Deploy em Azure, Docker, containerização e CI/CD.",
                    65,
                    6
                ));

                await cursoContext.Cursos.AddAsync(cursoAspNet);

                // Curso 3: Clean Architecture
                var cursoCleanArch = new Curso(
                    "Clean Architecture e Design Patterns",
                    "Domine os princípios de Clean Architecture e os principais Design Patterns para construir sistemas sustentáveis.",
                    "Dr. Carlos Oliveira",
                    NivelCurso.Avancado,
                    699.90m
                );

                var conteudoCleanArch = new ConteudoProgramatico(
                    "Princípios SOLID, Clean Architecture, DDD, CQRS, padrões de projeto e boas práticas de arquitetura de software.",
                    "Aplicar conceitos avançados de arquitetura de software para criar sistemas escaláveis, testáveis e de fácil manutenção.",
                    "Clean Architecture by Robert C. Martin, Design Patterns by Gang of Four, Domain-Driven Design by Eric Evans",
                    "https://blog.cleancoder.com"
                );
                cursoCleanArch.AdicionarConteudoProgramatico(conteudoCleanArch);

                cursoCleanArch.AdicionarAula(new Aula(
                    "Princípios SOLID",
                    "Single Responsibility, Open-Closed, Liskov Substitution, Interface Segregation e Dependency Inversion.",
                    95,
                    1
                ));

                cursoCleanArch.AdicionarAula(new Aula(
                    "Introdução à Clean Architecture",
                    "Camadas da arquitetura, regras de dependência e independência de frameworks.",
                    80,
                    2
                ));

                cursoCleanArch.AdicionarAula(new Aula(
                    "Domain-Driven Design Fundamentos",
                    "Entidades, Value Objects, Agregados, Repositórios e Domain Services.",
                    100,
                    3
                ));

                cursoCleanArch.AdicionarAula(new Aula(
                    "Design Patterns Essenciais",
                    "Repository, Unit of Work, Factory, Strategy, Observer e Mediator.",
                    90,
                    4
                ));

                cursoCleanArch.AdicionarAula(new Aula(
                    "CQRS e Event Sourcing",
                    "Command Query Responsibility Segregation e arquitetura orientada a eventos.",
                    85,
                    5
                ));

                await cursoContext.Cursos.AddAsync(cursoCleanArch);

                // Curso 4: Microserviços com .NET
                var cursoMicroservices = new Curso(
                    "Arquitetura de Microserviços com .NET",
                    "Construa sistemas distribuídos escaláveis utilizando microserviços, containers e mensageria.",
                    "Eng. Pedro Costa",
                    NivelCurso.Avancado,
                    799.90m
                );

                var conteudoMicroservices = new ConteudoProgramatico(
                    "Microserviços, Docker, Kubernetes, RabbitMQ, API Gateway, Service Discovery, resiliência e observabilidade.",
                    "Projetar e implementar arquiteturas de microserviços robustas e escaláveis utilizando as melhores práticas da indústria.",
                    "Building Microservices by Sam Newman, .NET Microservices Architecture, Kubernetes Documentation",
                    "https://docs.microsoft.com/dotnet/architecture/microservices"
                );
                cursoMicroservices.AdicionarConteudoProgramatico(conteudoMicroservices);

                cursoMicroservices.AdicionarAula(new Aula(
                    "Fundamentos de Microserviços",
                    "O que são microserviços, vantagens, desafios e quando utilizar.",
                    60,
                    1
                ));

                cursoMicroservices.AdicionarAula(new Aula(
                    "Containerização com Docker",
                    "Criando imagens Docker, Docker Compose e boas práticas de containerização.",
                    75,
                    2
                ));

                cursoMicroservices.AdicionarAula(new Aula(
                    "Orquestração com Kubernetes",
                    "Pods, Services, Deployments, ConfigMaps e Secrets no Kubernetes.",
                    90,
                    3
                ));

                cursoMicroservices.AdicionarAula(new Aula(
                    "Comunicação entre Microserviços",
                    "REST, gRPC, mensageria com RabbitMQ e padrões de comunicação.",
                    85,
                    4
                ));

                cursoMicroservices.AdicionarAula(new Aula(
                    "API Gateway e Service Discovery",
                    "Implementando API Gateway com Ocelot e Service Discovery com Consul.",
                    70,
                    5
                ));

                cursoMicroservices.AdicionarAula(new Aula(
                    "Resiliência e Observabilidade",
                    "Circuit Breaker, Retry Policies, logging distribuído e monitoramento.",
                    80,
                    6
                ));

                cursoMicroservices.AdicionarAula(new Aula(
                    "Segurança em Microserviços",
                    "Autenticação distribuída, OAuth 2.0, IdentityServer e segurança de comunicação.",
                    75,
                    7
                ));

                await cursoContext.Cursos.AddAsync(cursoMicroservices);

                // Curso 5: Testes Automatizados
                var cursoTestes = new Curso(
                    "Testes Automatizados em .NET",
                    "Aprenda a criar testes unitários, de integração e end-to-end para garantir a qualidade do seu software.",
                    "Profa. Ana Paula Lima",
                    NivelCurso.Intermediario,
                    399.90m
                );

                var conteudoTestes = new ConteudoProgramatico(
                    "xUnit, NUnit, Moq, FluentAssertions, testes de integração, TDD, BDD e cobertura de código.",
                    "Implementar estratégias de testes automatizados para garantir a qualidade, confiabilidade e manutenibilidade do software.",
                    "Test Driven Development by Kent Beck, The Art of Unit Testing by Roy Osherove, xUnit Documentation",
                    "https://xunit.net"
                );
                cursoTestes.AdicionarConteudoProgramatico(conteudoTestes);

                cursoTestes.AdicionarAula(new Aula(
                    "Fundamentos de Testes Automatizados",
                    "Pirâmide de testes, tipos de testes e ferramentas do ecossistema .NET.",
                    50,
                    1
                ));

                cursoTestes.AdicionarAula(new Aula(
                    "Testes Unitários com xUnit",
                    "Criando testes unitários, assertions, testes parametrizados e organização.",
                    65,
                    2
                ));

                cursoTestes.AdicionarAula(new Aula(
                    "Mocking com Moq",
                    "Criando mocks, stubs, verificações e isolamento de dependências.",
                    70,
                    3
                ));

                cursoTestes.AdicionarAula(new Aula(
                    "Testes de Integração",
                    "WebApplicationFactory, banco de dados em memória e testes de API.",
                    75,
                    4
                ));

                cursoTestes.AdicionarAula(new Aula(
                    "Test-Driven Development (TDD)",
                    "Ciclo Red-Green-Refactor, desenvolvimento guiado por testes na prática.",
                    80,
                    5
                ));

                await cursoContext.Cursos.AddAsync(cursoTestes);

                // Salvar todas as alterações
                await cursoContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var _ = ex.Message;
                throw; // Re-lançar a exceção para facilitar o debug
            }
        }
    }
}
