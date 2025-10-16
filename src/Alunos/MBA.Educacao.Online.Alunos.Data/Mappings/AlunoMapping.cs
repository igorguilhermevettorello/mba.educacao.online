using MBA.Educacao.Online.Alunos.Domain.Entities;
using MBA.Educacao.Online.Core.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MBA.Educacao.Online.Alunos.Data.Mappings
{
    public class AlunoMapping : IEntityTypeConfiguration<Aluno>
    {
        public void Configure(EntityTypeBuilder<Aluno> builder)
        {
            builder.ToTable("Alunos");

            builder.HasKey(a => a.Id);
            
            // Configura o Id para ser armazenado em minúsculas no SQLite usando GuidExtensions
            builder.Property(a => a.Id)
                .HasConversion(new ValueConverter<Guid, string>(
                    guid => guid.ToLowercaseString(),
                    str => Guid.Parse(str)
                ));
            
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

