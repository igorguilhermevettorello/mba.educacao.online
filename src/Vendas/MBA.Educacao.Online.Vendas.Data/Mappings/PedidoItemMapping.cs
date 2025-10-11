using MBA.Educacao.Online.Vendas.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MBA.Educacao.Online.Vendas.Data.Mappings
{
    public class PedidoItemMapping : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {
            builder.ToTable("PedidoItens");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.PedidoId)
                .IsRequired();

            builder.Property(i => i.CursoId)
                .IsRequired();

            builder.Property(i => i.CursoNome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(i => i.Valor)
                .HasPrecision(15, 2)
                .IsRequired();

            // Relacionamento N:1 -> PedidoItem pertence a um Pedido
            builder.HasOne(i => i.Pedido)
                .WithMany(p => p.PedidoItens)
                .HasForeignKey(i => i.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
