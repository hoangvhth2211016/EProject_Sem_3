using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EProject_Sem_3.Migrations
{
    /// <inheritdoc />
    public partial class update5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$8iBlO7.H1eDIYbmfIIvlGOOuS.kGq0xwnurTOUr8A8tiTi01qHPw.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$7AKBMODhuv1M1yJQU.smvup/6kqgh66Y/Fiqtxw0MxML08a96dbkO");
        }
    }
}
