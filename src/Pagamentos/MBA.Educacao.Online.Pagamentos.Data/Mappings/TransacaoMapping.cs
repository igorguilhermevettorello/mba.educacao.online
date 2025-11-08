using MBA.Educacao.Online.Pagamentos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBA.Educacao.Online.Pagamentos.Data.Mappings
{
    public class TransacaoMapping : IEntityTypeConfiguration<Transacao>
    {
        public void Configure(EntityTypeBuilder<Transacao> builder)
        {
            builder.ToTable("Transacoes");

            // Chave primária
            builder.HasKey(t => t.Id);

            // Propriedades
            builder.Property(t => t.PedidoId)
                .IsRequired();

            builder.Property(t => t.PagamentoId)
                .IsRequired();

            builder.Property(t => t.Total)
                .HasPrecision(15, 2)
                .IsRequired();

            // Enum - persistido como int
            builder.Property(t => t.StatusTransacao)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(t => t.DataTransacao)
                .IsRequired();

            // Relacionamento 1:1 com Pagamento
            builder.HasOne(t => t.Pagamento)
                .WithOne(p => p.Transacao)
                .HasForeignKey<Transacao>(t => t.PagamentoId);
        }
    }
}

