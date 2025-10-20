using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EloisaSantos.Migrations
{
    /// <inheritdoc />
    public partial class AjusteNaClasseConsumo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "adicionalBandeira",
                table: "Consumos",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "consumoFaturado",
                table: "Consumos",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "tarifa",
                table: "Consumos",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "taxaEsgoto",
                table: "Consumos",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "total",
                table: "Consumos",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "valorAgua",
                table: "Consumos",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "adicionalBandeira",
                table: "Consumos");

            migrationBuilder.DropColumn(
                name: "consumoFaturado",
                table: "Consumos");

            migrationBuilder.DropColumn(
                name: "tarifa",
                table: "Consumos");

            migrationBuilder.DropColumn(
                name: "taxaEsgoto",
                table: "Consumos");

            migrationBuilder.DropColumn(
                name: "total",
                table: "Consumos");

            migrationBuilder.DropColumn(
                name: "valorAgua",
                table: "Consumos");
        }
    }
}
