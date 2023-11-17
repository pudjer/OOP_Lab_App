using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AT_Infrastructure.Migrations
{
    public partial class CheckOut01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("97fccbfe-22d7-4073-91c3-214676eb01dd"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Password", "UpdatedAt", "Username" },
                values: new object[] { new Guid("221d31bb-2827-4c25-99b6-8eba976b1d17"), new DateTime(2023, 11, 14, 19, 27, 2, 820, DateTimeKind.Utc).AddTicks(6946), "$2a$11$6gFNc6m3uLDn2ZXVTU9uwezjqLmyg/7sLq18NaOPm5hFRt.fE624O", null, "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("221d31bb-2827-4c25-99b6-8eba976b1d17"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Password", "UpdatedAt", "Username" },
                values: new object[] { new Guid("97fccbfe-22d7-4073-91c3-214676eb01dd"), new DateTime(2023, 11, 12, 23, 35, 45, 973, DateTimeKind.Local).AddTicks(3430), "$2a$11$xe.WkmmSnbiBcn0F/D.aVeFUSFuwD3Yx7A/h5wPlNuZNlNvKaJVw6", null, "admin" });
        }
    }
}
