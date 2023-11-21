using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AT_Infrastructure.Migrations
{
    public partial class ForgotWhatThatOneIsDoing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8d76de5e-8e04-44a9-ae0b-c7f0014bf5f7"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "IsAdmin", "IsSubscribed", "Password", "UpdatedAt", "Username" },
                values: new object[] { new Guid("b3325b48-a690-4839-91bd-8bf1d60dca6c"), new DateTime(2023, 9, 17, 19, 19, 19, 97, DateTimeKind.Utc), true, true, "$2a$11$uCBSToyuBC9QsjgVWoiuDO8ttnnvK4eW.I8XrxVFeIGsUJm/WP0O6", null, "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b3325b48-a690-4839-91bd-8bf1d60dca6c"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "IsAdmin", "IsSubscribed", "Password", "UpdatedAt", "Username" },
                values: new object[] { new Guid("8d76de5e-8e04-44a9-ae0b-c7f0014bf5f7"), new DateTime(2023, 11, 17, 9, 11, 34, 330, DateTimeKind.Utc).AddTicks(9595), false, false, "$2a$11$ZkcgTqTH4uHiLNtSLikmTehXCioYUaVBiDFMEmPra9iSBaE73iwHy", null, "admin" });
        }
    }
}
