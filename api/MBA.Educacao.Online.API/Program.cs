using MBA.Educacao.Online.API.Configurations;

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

// Configuração de MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// builder.Services.AddAutoMapper(typeof(AutomapperConfig));

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
