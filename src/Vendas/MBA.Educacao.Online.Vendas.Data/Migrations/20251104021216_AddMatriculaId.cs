using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MBA.Educacao.Online.Vendas.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMatriculaId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MatriculaId",
                table: "PedidoItens",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatriculaId",
                table: "PedidoItens");
        }
    }
}
