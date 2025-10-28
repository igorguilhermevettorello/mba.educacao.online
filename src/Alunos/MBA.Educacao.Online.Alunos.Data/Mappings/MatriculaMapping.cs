using MBA.Educacao.Online.Alunos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBA.Educacao.Online.Alunos.Data.Mappings
{
    public class MatriculaMapping : IEntityTypeConfiguration<Matricula>
    {
        public void Configure(EntityTypeBuilder<Matricula> builder)
        {
            builder.ToTable("Matriculas");

            builder.HasKey(m => m.Id);

            // Adiciona a FK para Aluno
            builder.Property<Guid>("AlunoId")
                .IsRequired();

            builder.Property(m => m.CursoId)
                .IsRequired();

            builder.Property(m => m.DataMatricula)
                .IsRequired();

            builder.Property(m => m.DataValidade)
                .IsRequired();

            builder.Property(m => m.Ativo)
                .IsRequired();

            // Configuração de HistoricoAprendizado como owned entity (classe de valor)
            builder.OwnsMany(m => m.HistoricosAprendizado, h =>
            {
                h.ToTable("HistoricosAprendizado");
                
                h.WithOwner().HasForeignKey("MatriculaId");
                
                h.HasKey("Id");
                
                h.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();
                
                h.Property(h => h.AulaId)
                    .IsRequired(false);
                
                h.Property(h => h.DataInicio)
                    .IsRequired();
                
                h.Property(h => h.DataConclusao)
                    .IsRequired(false);
                
                h.Property(h => h.ProgressoPercentual)
                    .HasPrecision(5, 2)
                    .IsRequired();
                
                h.Property(h => h.Status)
                    .HasConversion<int>()
                    .IsRequired();
            });

            // Relacionamento com Aluno (muitos para um)
            builder.HasOne<Aluno>()
                .WithMany(a => a.Matriculas)
                .HasForeignKey("AlunoId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

