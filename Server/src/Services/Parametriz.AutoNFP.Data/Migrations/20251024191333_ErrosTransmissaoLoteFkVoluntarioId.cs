using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parametriz.AutoNFP.Data.Migrations
{
    /// <inheritdoc />
    public partial class ErrosTransmissaoLoteFkVoluntarioId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ErrosTransmissaoLote_Voluntarios_InstituicaoId",
                table: "ErrosTransmissaoLote");

            migrationBuilder.CreateIndex(
                name: "IX_ErrosTransmissaoLote_VoluntarioId",
                table: "ErrosTransmissaoLote",
                column: "VoluntarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_ErrosTransmissaoLote_Voluntarios_VoluntarioId",
                table: "ErrosTransmissaoLote",
                column: "VoluntarioId",
                principalTable: "Voluntarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ErrosTransmissaoLote_Voluntarios_VoluntarioId",
                table: "ErrosTransmissaoLote");

            migrationBuilder.DropIndex(
                name: "IX_ErrosTransmissaoLote_VoluntarioId",
                table: "ErrosTransmissaoLote");

            migrationBuilder.AddForeignKey(
                name: "FK_ErrosTransmissaoLote_Voluntarios_InstituicaoId",
                table: "ErrosTransmissaoLote",
                column: "InstituicaoId",
                principalTable: "Voluntarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
