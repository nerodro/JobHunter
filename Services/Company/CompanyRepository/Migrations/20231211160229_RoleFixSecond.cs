using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyRepository.Migrations
{
    /// <inheritdoc />
    public partial class RoleFixSecond : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companie_Role_RoleModelId",
                table: "Companie");

            migrationBuilder.DropIndex(
                name: "IX_Companie_RoleModelId",
                table: "Companie");

            migrationBuilder.DropColumn(
                name: "RoleModelId",
                table: "Companie");

            migrationBuilder.CreateIndex(
                name: "IX_Companie_RoleId",
                table: "Companie",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companie_Role_RoleId",
                table: "Companie",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companie_Role_RoleId",
                table: "Companie");

            migrationBuilder.DropIndex(
                name: "IX_Companie_RoleId",
                table: "Companie");

            migrationBuilder.AddColumn<int>(
                name: "RoleModelId",
                table: "Companie",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Companie_RoleModelId",
                table: "Companie",
                column: "RoleModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companie_Role_RoleModelId",
                table: "Companie",
                column: "RoleModelId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
