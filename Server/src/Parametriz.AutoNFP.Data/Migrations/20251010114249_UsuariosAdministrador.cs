using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parametriz.AutoNFP.Data.Migrations
{
    /// <inheritdoc />
    public partial class UsuariosAdministrador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Administrador",
                table: "Usuarios",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Administrador",
                table: "Usuarios");
        }
    }
}
