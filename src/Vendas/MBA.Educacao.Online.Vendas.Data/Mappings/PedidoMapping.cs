

using MBA.Educacao.Online.Vendas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBA.Educacao.Online.Vendas.Data.Mappings
{
    public class PedidoMapping : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Codigo)
                .IsRequired();

            builder.Property(p => p.AlunoId)
                .IsRequired();

            builder.Property(p => p.ValorTotal)
                .HasColumnType("decimal(15,2)")
                .IsRequired();

            builder.Property(p => p.DataCadastro)
                .IsRequired();

            builder.Property(p => p.PedidoStatus)
                .HasConversion<int>() // grava enum como inteiro
                .IsRequired();

            // Relacionamento 1:N -> Pedido tem muitos PedidoItens
            builder.HasMany<PedidoItem>(p => p.PedidoItens)
                .WithOne(i => i.Pedido)
                .HasForeignKey(i => i.PedidoId);
        }
    }
}
