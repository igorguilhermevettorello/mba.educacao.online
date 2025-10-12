using MBA.Educacao.Online.Alunos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBA.Educacao.Online.Alunos.Data.Mappings
{
    public class AlunoMapping : IEntityTypeConfiguration<Aluno>
    {
        public void Configure(EntityTypeBuilder<Aluno> builder)
        {
            builder.ToTable("Alunos");

            builder.HasKey(a => a.Id);
            
            builder.Property(a => a.Nome)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(a => a.Email)
                .IsRequired()
                .HasColumnType("varchar(256)");

            builder.HasIndex(a => a.Email)
                .IsUnique();

            builder.Property(a => a.DataCadastro)
                .IsRequired();

            builder.Property(a => a.Ativo)
                .IsRequired();
        }
    }
}

