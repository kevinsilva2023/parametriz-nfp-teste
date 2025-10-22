using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parametriz.AutoNFP.Data.Migrations
{
    /// <inheritdoc />
    public partial class InstituicaoNovaEstrutura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CuponsFiscais_Usuarios_CadastradoPorId",
                table: "CuponsFiscais");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Voluntarios_InstituicaoId",
                table: "Voluntarios");

            migrationBuilder.DropColumn(
                name: "Emissor",
                table: "Voluntarios");

            migrationBuilder.DropColumn(
                name: "EntidadeNomeNFP",
                table: "Voluntarios");

            migrationBuilder.DropColumn(
                name: "Requerente",
                table: "Voluntarios");

            migrationBuilder.DropColumn(
                name: "Senha",
                table: "Voluntarios");

            migrationBuilder.DropColumn(
                name: "TipoPessoa",
                table: "Voluntarios");

            migrationBuilder.DropColumn(
                name: "Upload",
                table: "Voluntarios");

            migrationBuilder.DropColumn(
                name: "ValidoAPartirDe",
                table: "Voluntarios");

            migrationBuilder.DropColumn(
                name: "ValidoAte",
                table: "Voluntarios");

            migrationBuilder.DropColumn(
                name: "TipoPessoa",
                table: "Instituicoes");

            migrationBuilder.RenameColumn(
                name: "CnpjCpf",
                table: "Voluntarios",
                newName: "Cpf");

            migrationBuilder.RenameColumn(
                name: "CnpjCpf",
                table: "Instituicoes",
                newName: "Cnpj");

            migrationBuilder.RenameIndex(
                name: "IX_Instituicoes_CnpjCpf",
                table: "Instituicoes",
                newName: "IX_Instituicoes_Cnpj");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Voluntarios",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Voluntarios",
                type: "character(11)",
                fixedLength: true,
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(14)",
                oldMaxLength: 14,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Administrador",
                table: "Voluntarios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Contato",
                table: "Voluntarios",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Desativado",
                table: "Voluntarios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Voluntarios",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FotoUpload",
                table: "Voluntarios",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntidadeNomeNFP",
                table: "Instituicoes",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Certificados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VoluntarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true),
                    Requerente = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ValidoAPartirDe = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ValidoAte = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Emissor = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Upload = table.Column<byte[]>(type: "bytea", nullable: false),
                    Senha = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificados_Voluntarios_VoluntarioId",
                        column: x => x.VoluntarioId,
                        principalTable: "Voluntarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_Certificados_VoluntarioId",
                table: "Certificados",
                column: "VoluntarioId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CuponsFiscais_Voluntarios_CadastradoPorId",
                table: "CuponsFiscais",
                column: "CadastradoPorId",
                principalTable: "Voluntarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CuponsFiscais_Voluntarios_CadastradoPorId",
                table: "CuponsFiscais");

            migrationBuilder.DropTable(
                name: "Certificados");

            migrationBuilder.DropIndex(
                name: "IX_Voluntarios_Email",
                table: "Voluntarios");

            migrationBuilder.DropIndex(
                name: "IX_Voluntarios_InstituicaoId_Nome",
                table: "Voluntarios");

            migrationBuilder.DropColumn(
                name: "Administrador",
                table: "Voluntarios");

            migrationBuilder.DropColumn(
                name: "Contato",
                table: "Voluntarios");

            migrationBuilder.DropColumn(
                name: "Desativado",
                table: "Voluntarios");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Voluntarios");

            migrationBuilder.DropColumn(
                name: "FotoUpload",
                table: "Voluntarios");

            migrationBuilder.DropColumn(
                name: "EntidadeNomeNFP",
                table: "Instituicoes");

            migrationBuilder.RenameColumn(
                name: "Cpf",
                table: "Voluntarios",
                newName: "CnpjCpf");

            migrationBuilder.RenameColumn(
                name: "Cnpj",
                table: "Instituicoes",
                newName: "CnpjCpf");

            migrationBuilder.RenameIndex(
                name: "IX_Instituicoes_Cnpj",
                table: "Instituicoes",
                newName: "IX_Instituicoes_CnpjCpf");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Voluntarios",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CnpjCpf",
                table: "Voluntarios",
                type: "character varying(14)",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character(11)",
                oldFixedLength: true,
                oldMaxLength: 11,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Emissor",
                table: "Voluntarios",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EntidadeNomeNFP",
                table: "Voluntarios",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Requerente",
                table: "Voluntarios",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Senha",
                table: "Voluntarios",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<int>(
                name: "TipoPessoa",
                table: "Voluntarios",
                type: "integer",
                fixedLength: true,
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Upload",
                table: "Voluntarios",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidoAPartirDe",
                table: "Voluntarios",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidoAte",
                table: "Voluntarios",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TipoPessoa",
                table: "Instituicoes",
                type: "integer",
                fixedLength: true,
                maxLength: 1,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InstituicaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Administrador = table.Column<bool>(type: "boolean", nullable: false),
                    Desativado = table.Column<bool>(type: "boolean", nullable: false),
                    FotoUpload = table.Column<string>(type: "text", nullable: true),
                    Nome = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Instituicoes_InstituicaoId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_CuponsFiscais_Usuarios_CadastradoPorId",
                table: "CuponsFiscais",
                column: "CadastradoPorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
