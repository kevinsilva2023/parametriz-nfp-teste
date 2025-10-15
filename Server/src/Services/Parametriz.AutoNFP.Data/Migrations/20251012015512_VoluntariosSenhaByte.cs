using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parametriz.AutoNFP.Data.Migrations
{
    /// <inheritdoc />
    public partial class VoluntariosSenhaByte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Senha",
                table: "Voluntarios");

            migrationBuilder.AddColumn<byte[]>(
                name: "Senha",
                table: "Voluntarios",
                type: "bytea",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Senha",
                table: "Voluntarios");

            migrationBuilder.AddColumn<string>(
                name: "Senha",
                table: "Voluntarios",
                type: "text",
                nullable: false);
        }
    }
}
