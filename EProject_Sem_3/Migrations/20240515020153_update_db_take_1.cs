using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EProject_Sem_3.Migrations
{
    /// <inheritdoc />
    public partial class update_db_take_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$VbtXYj3OwnPNcuCxe7pE.OiI1hL4Z9K6pWU.IsqLZoT41fLNHsb8G");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$yjWf5Jnjf1Cm0W4q8ZHfr.8TYc3PACRlBiXN9mOabGxdYrddF96dS");
        }
    }
}
