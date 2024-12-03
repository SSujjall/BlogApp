using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Rolesupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7a214121-b34d-4cea-8cf2-0e2fca3c9f80", "79474565-7035-4287-94d1-95348181e584" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7a214121-b34d-4cea-8cf2-0e2fca3c9f80");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "79474565-7035-4287-94d1-95348181e584");

            migrationBuilder.AddColumn<int>(
                name: "DownVoteCount",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PopularityScore",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UpVoteCount",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c9274285-72de-482d-aeba-3897e196b605", "c9274285-72de-482d-aeba-3897e196b605", "Superadmin", "SUPERADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "db73e4ec-2ce5-4007-b2b9-aedeca1714da", 0, "cf5ddd0e-9941-4eb8-b9e5-a05c884b861d", "User", "superadmin@blog.com", true, false, null, "SUPERADMIN@BLOG.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEGA/i2fSOToRACRtf38pcpa6KO1enJHpdefbnj9GwWLY4Tu/+3NCHLyec1Gd2ucAOA==", null, false, "0a17155a-cc7a-4a93-9670-c49ca5f5b00f", false, "Superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "c9274285-72de-482d-aeba-3897e196b605", "db73e4ec-2ce5-4007-b2b9-aedeca1714da" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "c9274285-72de-482d-aeba-3897e196b605", "db73e4ec-2ce5-4007-b2b9-aedeca1714da" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c9274285-72de-482d-aeba-3897e196b605");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "db73e4ec-2ce5-4007-b2b9-aedeca1714da");

            migrationBuilder.DropColumn(
                name: "DownVoteCount",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "PopularityScore",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "UpVoteCount",
                table: "Blogs");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7a214121-b34d-4cea-8cf2-0e2fca3c9f80", "7a214121-b34d-4cea-8cf2-0e2fca3c9f80", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "79474565-7035-4287-94d1-95348181e584", 0, "02cc9ba3-23de-444e-a703-de5ca957d59b", "User", "admin@user.com", true, false, null, "ADMIN@USER.COM", "ADMIN@USER.COM", "AQAAAAIAAYagAAAAEH4LxRiMqCdrUoy/XsWuF4rzsB63bptxi+oDkCzAiOVrZ4hr52Qx/WCwMpzpWHoQQw==", null, false, "d2083f26-96b0-4c4a-984f-95534987650c", false, "admin@user.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "7a214121-b34d-4cea-8cf2-0e2fca3c9f80", "79474565-7035-4287-94d1-95348181e584" });
        }
    }
}
