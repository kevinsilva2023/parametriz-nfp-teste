using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parametriz.AutoNFP.Data.Migrations
{
    /// <inheritdoc />
    public partial class CuponsFiscaisEmitidoEm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Competencia",
                table: "CuponsFiscais",
                newName: "EmitidoEm");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmitidoEm",
                table: "CuponsFiscais",
                newName: "Competencia");
        }
    }
}
