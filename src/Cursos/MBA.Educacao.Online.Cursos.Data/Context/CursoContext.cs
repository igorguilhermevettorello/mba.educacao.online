using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using MBA.Educacao.Online.Cursos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MBA.Educacao.Online.Cursos.Data.Context
{
    public class CursoContext : DbContext, IUnitOfWork
    {
        // private readonly IMediator _mediator;

        public CursoContext(DbContextOptions<CursoContext> options, IMediator mediator) : base(options)
        {
            // _mediator = mediator;
        }

        public DbSet<Curso> Cursos { get; set; }
        
        public DbSet<Aula> Aulas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model
                         .GetEntityTypes()
                         .SelectMany(e => e.GetProperties())
                         .Where(p => p.ClrType == typeof(string)))
            {
                property.SetColumnType("varchar(100)");
            }
            
            // Ignorar propriedades de domínio que não devem ser persistidas
            modelBuilder.Ignore<Core.Domain.Messages.Event>();
            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CursoContext).Assembly);
        }
        
        public async Task<bool> Commit()
        {
            var success = (await base.SaveChangesAsync()) > 0;
            // if (success) await _mediator.PublicarEventos(this);
            return success;
        }
    }
}

