using MBA.Educacao.Online.Alunos.Domain.Entities;
using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MBA.Educacao.Online.Alunos.Data.Context
{
    public class AlunoContext : DbContext, IUnitOfWork
    {
        public AlunoContext(DbContextOptions<AlunoContext> options) : base(options) { }

        public DbSet<Aluno> Alunos { get; set; }

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
            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AlunoContext).Assembly);
        }
        
        public async Task<bool> Commit()
        {
            var success = (await base.SaveChangesAsync()) > 0;
            // if (success) await _mediator.PublicarEventos(this);
            return success;
        }
    }
}

