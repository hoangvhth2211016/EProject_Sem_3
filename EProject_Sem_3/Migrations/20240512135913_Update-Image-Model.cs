using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EProject_Sem_3.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImageModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RecipeImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BookImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$AMtvwgHNflS69twxbyoK6ekAyXVj49O1IEltzljeDWrICzUPKR5C2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "RecipeImages");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BookImages");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$6MoK2bp9caE9a94a2Tt8uOLx0HOSpCoF4BdYOnOfSiRRQsWwFY8ZG");
        }
    }
}
