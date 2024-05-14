using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EProject_Sem_3.Migrations
{
    /// <inheritdoc />
    public partial class qa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActivated",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsActivated", "Password" },
                values: new object[] { true, "$2a$11$7AKBMODhuv1M1yJQU.smvup/6kqgh66Y/Fiqtxw0MxML08a96dbkO" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActivated",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$AMtvwgHNflS69twxbyoK6ekAyXVj49O1IEltzljeDWrICzUPKR5C2");
        }
    }
}
