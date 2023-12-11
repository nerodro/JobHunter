using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyRepository.Migrations
{
    /// <inheritdoc />
    public partial class RoleFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companie_Role_RoleModelId",
                table: "Companie");

            migrationBuilder.AlterColumn<int>(
                name: "RoleModelId",
                table: "Companie",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Companie",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Moder" },
                    { 3, "CompanyModer" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Companie_Role_RoleModelId",
                table: "Companie",
                column: "RoleModelId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companie_Role_RoleModelId",
                table: "Companie");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Companie");

            migrationBuilder.AlterColumn<int>(
                name: "RoleModelId",
                table: "Companie",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Companie_Role_RoleModelId",
                table: "Companie",
                column: "RoleModelId",
                principalTable: "Role",
                principalColumn: "Id");
        }
    }
}
