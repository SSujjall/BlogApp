using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class adminroleseeded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d30cb2d3-58ec-4350-ac98-222b2e8f2fa9", "d30cb2d3-58ec-4350-ac98-222b2e8f2fa9", "Admin", "ADMIN" },
                    { "d91e3363-7708-4b0a-87c4-9dcd779437c8", "d91e3363-7708-4b0a-87c4-9dcd779437c8", "Superadmin", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "8edceed0-ac71-4761-a5f6-58f589f39d13", 0, "f69494dd-18f0-4415-b14a-be1739b4c710", "User", "superadmin@blog.com", true, false, null, "SUPERADMIN@BLOG.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEFSr2a9dY7NuNRGbr3j98v0rLwE0gYwD/K3I8zlQ3rdWKsZqYDPWGrvfw4Qhl/edqA==", null, false, "fcdc9995-eff4-473f-97d1-6b60f31fdd08", false, "Superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "d91e3363-7708-4b0a-87c4-9dcd779437c8", "8edceed0-ac71-4761-a5f6-58f589f39d13" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d30cb2d3-58ec-4350-ac98-222b2e8f2fa9");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "d91e3363-7708-4b0a-87c4-9dcd779437c8", "8edceed0-ac71-4761-a5f6-58f589f39d13" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d91e3363-7708-4b0a-87c4-9dcd779437c8");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8edceed0-ac71-4761-a5f6-58f589f39d13");

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
    }
}
