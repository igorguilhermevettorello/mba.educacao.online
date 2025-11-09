using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MBA.Educacao.Online.Alunos.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterarTableProgressoPercentual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProgressoPercentual",
                table: "HistoricosAprendizado");

            migrationBuilder.AddColumn<int>(
                name: "ProgressoPercentual",
                table: "Matriculas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProgressoPercentual",
                table: "Matriculas");

            migrationBuilder.AddColumn<int>(
                name: "ProgressoPercentual",
                table: "HistoricosAprendizado",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
