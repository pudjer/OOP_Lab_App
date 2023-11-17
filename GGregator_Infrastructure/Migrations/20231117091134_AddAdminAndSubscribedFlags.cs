using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GGregator_Infrastructure.Migrations
{
    public partial class AddAdminAndSubscribedFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aff86cec-4257-464d-b48c-0bab6a5d9b8f"));

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubscribed",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "IsAdmin", "IsSubscribed", "Password", "UpdatedAt", "Username" },
                values: new object[] { new Guid("8d76de5e-8e04-44a9-ae0b-c7f0014bf5f7"), new DateTime(2023, 11, 17, 9, 11, 34, 330, DateTimeKind.Utc).AddTicks(9595), false, false, "$2a$11$ZkcgTqTH4uHiLNtSLikmTehXCioYUaVBiDFMEmPra9iSBaE73iwHy", null, "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8d76de5e-8e04-44a9-ae0b-c7f0014bf5f7"));

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsSubscribed",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Password", "UpdatedAt", "Username" },
                values: new object[] { new Guid("aff86cec-4257-464d-b48c-0bab6a5d9b8f"), new DateTime(2023, 11, 17, 9, 10, 32, 120, DateTimeKind.Utc).AddTicks(8614), "$2a$11$bKpyKOdeCAv3Y3ueqHb.duDw8qj9mOwVkGnTi2bgvNoFvQavC6RvW", null, "admin" });
        }
    }
}
