using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parametriz.AutoNFP.Api.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokenUsuarioId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "RefreshTokens");

            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "RefreshTokens",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "RefreshTokens");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "RefreshTokens",
                type: "text",
                nullable: true);
        }
    }
}
