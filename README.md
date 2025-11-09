# **MBA Plataforma de Educação Online - Aplicação API RESTful**

## **1. Apresentação**

Bem-vindo ao repositório do projeto **MBA Plataforma de Educação Online**. Este projeto é uma entrega do MBA DevXpert Full Stack .NET e é referente ao módulo **Arquitetura, Modelagem e Qualidade de Software**.
O **MBA Educação Online** é uma plataforma completa de gerenciamento de cursos online desenvolvida em **ASP.NET Core 8.0** seguindo os princípios da **Clean Architecture**, **Domain-Driven Design (DDD)** e **SOLID**. O sistema permite o cadastro e gestão de cursos, matrículas de alunos, processamento de pagamentos e acompanhamento do progresso acadêmico.

### **Autor(es)**
- **Igor Guilherme Vettorello**


## **2. Proposta do Projeto**

O projeto consiste em:

- **API RESTful:** Exposição dos recursos do sistema para integração com outras aplicações ou desenvolvimento de front-ends alternativos.
- **Autenticação e Autorização:** Implementação de controle de acesso. O sistema garante que apenas o Administradores e Alunos acessem o sistema.
- **Acesso a Dados:** Implementação de acesso ao banco de dados através de ORM.

## **3. Tecnologias Utilizadas**

- **Documentação da API:** Swagger
- **Framework**: .NET 8.0
- **ORM**: Entity Framework Core
- **Database**: SQLite (dev)
- **Auth**: ASP.NET Identity + JWT
- **Patterns**: CQRS (MediatR), Repository, Unit of Work
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **Docs**: Swagger/OpenAPI
- **Tests**: xUnit + Coverlet
- **Monitoring**: Elmah.io

## **4. Estrutura do Projeto**

A estrutura do projeto é organizada da seguinte forma:

mba.educacao.online/
├── api/                    # Camada de apresentação (Controllers, DTOs)
├── src/                    # Código fonte (5 bounded contexts)
│   ├── Core/               # Infraestrutura compartilhada
│   ├── Cursos/             # Bounded Context: Cursos
│   ├── Vendas/             # Bounded Context: Pedidos
│   ├── Pagamentos/         # Bounded Context: Transações
│   └── Alunos/             # Bounded Context: Matrículas
├── test/                   # Testes automatizados
├── comandos/               # Documentação e collections Postman
└── sqlite/ 

## **5. Funcionalidades Implementadas**

- **CRUD para Categoria e Produtos:** Permite criar, editar, visualizar e excluir categorias e produtos.
- **Autenticação e Autorização:** Diferenciação entre vendedores.
- **API RESTful:** Exposição de endpoints para operações CRUD via API.
- **Documentação da API:** Documentação automática dos endpoints da API utilizando Swagger.

### 5.1. **CQRS (Command Query Responsibility Segregation)**
- Separação clara entre Commands (escrita) e Queries (leitura)
- Handlers dedicados para cada operação
- Exemplo: `CriarCursoCommand` → `CriarCursoCommandHandler`

### 5.2. **Repository Pattern**
- Abstrações de acesso a dados via interfaces
- Implementações concretas na camada de Data
- Unit of Work para transações

### 5.3. **Mediator Pattern**
- MediatR para desacoplamento entre camadas
- Comunicação via commands e events
- Handlers registrados via Dependency Injection

### 5.4. **Domain Events**
- Eventos de domínio para comunicação entre bounded contexts
- Eventos de integração:
  - `PagamentoRealizadoEvent`
  - `MatriculaConfirmadaEvent`
  - `EfetuarMatriculaEvent`
  - `AtualizarPedidoItemMatriculaEvent`

### 5.5. **Anti-Corruption Layer**
- Isolamento de dependências externas (gateway de pagamento)
- Camada de tradução entre modelos externos e domínio interno

### 5.6. **Validator Pattern**
- FluentValidation para validações complexas
- Validators dedicados para cada Command

## **6. Como Executar o Projeto**

### **Pré-requisitos**

- .NET SDK 8.0 ou superior
- SQL Server
- SQLite
- Visual Studio 2022 ou superior (ou qualquer IDE de sua preferência)
- Git

### **Passos para Execução**

1. **Clone o Repositório:**
   - `git clone https://github.com/igorguilhermevettorello/mba.educacao.online.git`
   - `cd mba.educacao.online`

2. **Configuração do Banco de Dados:**
   - No arquivo `appsettings.json`, configure a string de conexão do SQLite e SQL Server.
   - Rode o projeto para que a configuração do Seed crie o banco e popule com os dados básicos

3. **Executar a Aplicação:**
   - `cd mba.educacao.online`
   - `dotnet build`
   - `cd api/MBA.Educacao.Online.API`
   - `dotnet run`
   - Acesse a aplicação em: http://localhost:5042/swagger/index.html

## **7. Instruções de Configuração**

- **JWT para API:** As chaves de configuração do JWT estão no `appsettings.json`.
- **Migrações do Banco de Dados:** As migrações são gerenciadas pelo Entity Framework Core. Não é necessário aplicar devido a configuração do Seed de dados.

## **8. Documentação da API**

A documentação da API está disponível através do Swagger. Após iniciar a API, acesse a documentação em:

https://localhost:7053/swagger/index.html

## **9. Avaliação**

- Este projeto é parte de um curso acadêmico e não aceita contribuições externas. 
- Para feedbacks ou dúvidas utilize o recurso de Issues
- O arquivo `FEEDBACK.md` é um resumo das avaliações do instrutor e deverá ser modificado apenas por ele.