using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EProject_Sem_3.Migrations
{
    /// <inheritdoc />
    public partial class ab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$n0gS6CKcBY/BN8NNyYo3yOdSzISn3WtIWPcSc2/8FUbWyqRxcSWbO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$ufcUdfsy.WO2/FfOy31XhubxtuyDQSwdlsGfP2QRKsnFopd0ef9ry");
        }
    }
}
