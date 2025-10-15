using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parametriz.AutoNFP.Data.Migrations
{
    /// <inheritdoc />
    public partial class CuponsFiscaisCompetenciaNumeroCadastradoEm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MesEmissao",
                table: "CuponsFiscais",
                newName: "Competencia");

            migrationBuilder.AddColumn<DateTime>(
                name: "CadastradoEm",
                table: "CuponsFiscais",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Numero",
                table: "CuponsFiscais",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CadastradoEm",
                table: "CuponsFiscais");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "CuponsFiscais");

            migrationBuilder.RenameColumn(
                name: "Competencia",
                table: "CuponsFiscais",
                newName: "MesEmissao");
        }
    }
}
