using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parametriz.AutoNFP.Data.Migrations
{
    /// <inheritdoc />
    public partial class CuponsFiscais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Instituicoes_InstituicaoId",
                table: "Usuarios");

            migrationBuilder.CreateTable(
                name: "CuponsFiscais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Chave = table.Column<string>(type: "character(44)", fixedLength: true, maxLength: 44, nullable: true),
                    MesEmissao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Cnpj = table.Column<string>(type: "character(14)", fixedLength: true, maxLength: 14, nullable: true),
                    CadastradoPorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    EnviadoEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MensagemErro = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    InstituicaoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuponsFiscais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CuponsFiscais_Instituicoes_InstituicaoId",
                        column: x => x.InstituicaoId,
                        principalTable: "Instituicoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CuponsFiscais_Usuarios_CadastradoPorId",
                        column: x => x.CadastradoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CuponsFiscais_CadastradoPorId",
                table: "CuponsFiscais",
                column: "CadastradoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_CuponsFiscais_Chave",
                table: "CuponsFiscais",
                column: "Chave",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CuponsFiscais_InstituicaoId",
                table: "CuponsFiscais",
                column: "InstituicaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Instituicoes_InstituicaoId",
                table: "Usuarios",
                column: "InstituicaoId",
                principalTable: "Instituicoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Instituicoes_InstituicaoId",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "CuponsFiscais");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Instituicoes_InstituicaoId",
                table: "Usuarios",
                column: "InstituicaoId",
                principalTable: "Instituicoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
