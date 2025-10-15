using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parametriz.AutoNFP.Data.Migrations
{
    /// <inheritdoc />
    public partial class Voluntarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Voluntarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    TipoPessoa = table.Column<int>(type: "integer", fixedLength: true, maxLength: 1, nullable: true),
                    CnpjCpf = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    Requerente = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ValidoAPartirDe = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ValidoAte = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Emissor = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Upload = table.Column<byte[]>(type: "bytea", nullable: false),
                    Senha = table.Column<string>(type: "text", nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Voluntarios_InstituicaoId",
                table: "Voluntarios",
                column: "InstituicaoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Voluntarios");
        }
    }
}
