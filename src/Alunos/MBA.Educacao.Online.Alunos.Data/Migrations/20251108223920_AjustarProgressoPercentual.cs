using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MBA.Educacao.Online.Alunos.Data.Migrations
{
    /// <inheritdoc />
    public partial class AjustarProgressoPercentual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProgressoPercentual",
                table: "HistoricosAprendizado",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT",
                oldPrecision: 5,
                oldScale: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ProgressoPercentual",
                table: "HistoricosAprendizado",
                type: "TEXT",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
