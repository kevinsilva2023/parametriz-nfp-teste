using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parametriz.AutoNFP.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnderecosRemoveTipoLogradouro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoLogradouro",
                table: "Instituicoes");

            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "Certificados");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Certificados");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipoLogradouro",
                table: "Instituicoes",
                type: "character varying(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "Certificados",
                type: "character varying(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Certificados",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }
    }
}
