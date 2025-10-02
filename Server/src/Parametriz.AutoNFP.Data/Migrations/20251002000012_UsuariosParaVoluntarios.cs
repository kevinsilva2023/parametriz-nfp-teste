using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parametriz.AutoNFP.Data.Migrations
{
    /// <inheritdoc />
    public partial class UsuariosParaVoluntarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.CreateTable(
                name: "Instituicoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RazaoSocial = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    TipoPessoa = table.Column<int>(type: "integer", fixedLength: true, maxLength: 1, nullable: true),
                    CnpjCpf = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    TipoLogradouro = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    Logradouro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Numero = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Complemento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Bairro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CEP = table.Column<string>(type: "character(8)", fixedLength: true, maxLength: 8, nullable: true),
                    Municipio = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UF = table.Column<string>(type: "character(2)", fixedLength: true, maxLength: 2, nullable: true),
                    Desativada = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instituicoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Voluntarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    InstituicaoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voluntarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Voluntarios_Instituicoes_InstituicaoId",
                        column: x => x.InstituicaoId,
                        principalTable: "Instituicoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Instituicoes_CnpjCpf",
                table: "Instituicoes",
                column: "CnpjCpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Voluntarios_Email",
                table: "Voluntarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Voluntarios_InstituicaoId_Nome",
                table: "Voluntarios",
                columns: new[] { "InstituicaoId", "Nome" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Voluntarios");

            migrationBuilder.DropTable(
                name: "Instituicoes");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }
    }
}
