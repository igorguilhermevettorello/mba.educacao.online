using MBA.Educacao.Online.API.Configurations;
using MBA.Educacao.Online.Core.Application;
using MBA.Educacao.Online.Alunos.Application;
using MBA.Educacao.Online.Cursos.Application;
using MBA.Educacao.Online.Vendas.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração de Database (SQLite para Development, SQL Server para Production)
builder.AddDatabaseSelector();

// Configuração de Identity
builder.Services.AddIdentityConfig(builder.Configuration);
builder.Services.AddApiConfig(builder.Configuration);

// Configuração de Dependency Injection
builder.Services.AddDependencyInjectionConfig(builder.Configuration);

// Configuração de Application Layer (MediatR, FluentValidation, AutoMapper)
builder.Services.AddCoreApplication();
builder.Services.AddAlunosApplication();
builder.Services.AddApplication();
builder.Services.AddVendasApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMigrationsAndSeedsConfig();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
