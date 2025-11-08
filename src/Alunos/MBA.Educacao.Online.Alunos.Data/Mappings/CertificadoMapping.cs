using MBA.Educacao.Online.Alunos.Domain.Entities;
using MBA.Educacao.Online.Core.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MBA.Educacao.Online.Alunos.Data.Mappings
{
    public class CertificadoMapping : IEntityTypeConfiguration<Certificado>
    {
        public void Configure(EntityTypeBuilder<Certificado> builder)
        {
            builder.ToTable("Certificados");

            builder.HasKey(c => c.Id);

            // Configura o AlunoId para ser armazenado em minúsculas no SQLite usando GuidExtensions
            builder.Property<Guid>("AlunoId")
                .IsRequired()
                .HasConversion(new ValueConverter<Guid, string>(
                    guid => guid.ToLowercaseString(),
                    str => Guid.Parse(str)));

            builder.Property(c => c.CursoId)
                .IsRequired();

            builder.Property(c => c.DataEmissao)
                .IsRequired();

            builder.Property(c => c.Codigo)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(c => c.Ativo)
                .IsRequired();

            // Relacionamento com Aluno (muitos para um)
            builder.HasOne<Aluno>()
                .WithMany(a => a.Certificados)
                .HasForeignKey("AlunoId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

