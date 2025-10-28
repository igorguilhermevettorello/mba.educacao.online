using MBA.Educacao.Online.Alunos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBA.Educacao.Online.Alunos.Data.Mappings
{
    public class CertificadoMapping : IEntityTypeConfiguration<Certificado>
    {
        public void Configure(EntityTypeBuilder<Certificado> builder)
        {
            builder.ToTable("Certificados");

            builder.HasKey(c => c.Id);

            // Adiciona a FK para Aluno
            builder.Property<Guid>("AlunoId")
                .IsRequired();

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

