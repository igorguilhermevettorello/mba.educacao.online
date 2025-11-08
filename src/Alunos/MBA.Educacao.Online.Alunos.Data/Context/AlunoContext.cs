using MBA.Educacao.Online.Alunos.Domain.Entities;
using MBA.Educacao.Online.Core.Domain.Interfaces.Mediator;
using MBA.Educacao.Online.Core.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MBA.Educacao.Online.Alunos.Data.Context
{
    public class AlunoContext : DbContext, IUnitOfWork
    {

        private readonly IMediatorHandler _mediatorHandler;

        public AlunoContext(DbContextOptions<AlunoContext> options, IMediatorHandler mediatorHandler) : base(options)
        {
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Certificado> Certificados { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }

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
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }
            }

            var success = (await base.SaveChangesAsync()) > 0;
            if (success) await _mediatorHandler.PublicarEventos(this);
            return success;
        }
    }
}

