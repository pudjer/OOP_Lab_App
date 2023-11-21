using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AT_Infrastructure.Migrations
{
    public partial class RemoveSubscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("221d31bb-2827-4c25-99b6-8eba976b1d17"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Password", "UpdatedAt", "Username" },
                values: new object[] { new Guid("aff86cec-4257-464d-b48c-0bab6a5d9b8f"), new DateTime(2023, 11, 17, 9, 10, 32, 120, DateTimeKind.Utc).AddTicks(8614), "$2a$11$bKpyKOdeCAv3Y3ueqHb.duDw8qj9mOwVkGnTi2bgvNoFvQavC6RvW", null, "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aff86cec-4257-464d-b48c-0bab6a5d9b8f"));

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Unsubscribed = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Password", "UpdatedAt", "Username" },
                values: new object[] { new Guid("221d31bb-2827-4c25-99b6-8eba976b1d17"), new DateTime(2023, 11, 14, 19, 27, 2, 820, DateTimeKind.Utc).AddTicks(6946), "$2a$11$6gFNc6m3uLDn2ZXVTU9uwezjqLmyg/7sLq18NaOPm5hFRt.fE624O", null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions",
                column: "UserId");
        }
    }
}
