using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parametriz.AutoNFP.Data.Migrations
{
    /// <inheritdoc />
    public partial class VoluntariosParaUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Voluntarios");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Desativado = table.Column<bool>(type: "boolean", nullable: false),
                    InstituicaoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Instituicoes_InstituicaoId",
                        column: x => x.InstituicaoId,
                        principalTable: "Instituicoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_InstituicaoId_Nome",
                table: "Usuarios",
                columns: new[] { "InstituicaoId", "Nome" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.CreateTable(
                name: "Voluntarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InstituicaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Desativado = table.Column<bool>(type: "boolean", nullable: false),
                    Nome = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
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
    }
}
