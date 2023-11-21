using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AT_Infrastructure.Migrations
{
    public partial class AddSuperuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Password", "UpdatedAt", "Username" },
                values: new object[] { new Guid("97fccbfe-22d7-4073-91c3-214676eb01dd"), new DateTime(2023, 11, 12, 23, 35, 45, 973, DateTimeKind.Local).AddTicks(3430), "$2a$11$xe.WkmmSnbiBcn0F/D.aVeFUSFuwD3Yx7A/h5wPlNuZNlNvKaJVw6", null, "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("97fccbfe-22d7-4073-91c3-214676eb01dd"));
        }
    }
}
