using MBA.Educacao.Online.API.Configurations;
using MBA.Educacao.Online.Cursos.Application;

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

// Configuração de Dependency Injection
builder.Services.AddDependencyInjectionConfig(builder.Configuration);

// Configuração de Application Layer (MediatR, FluentValidation, AutoMapper)
builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
