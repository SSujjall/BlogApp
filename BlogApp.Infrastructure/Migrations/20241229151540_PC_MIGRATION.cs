using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PC_MIGRATION : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[,]
                {
                    { "4ab90107-b1d0-49e8-9a97-c58a7d250a20", "4ab90107-b1d0-49e8-9a97-c58a7d250a20", "Superadmin", "SUPERADMIN" },
                    { "c6d49bef-a693-4c6c-8ecf-3442da793a9f", "c6d49bef-a693-4c6c-8ecf-3442da793a9f", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "486faf3b-3eb1-4fbe-b2ab-922cdc5f0bc9", 0, "227a1ba7-dace-45c8-8e55-2a3478b9e031", "User", "superadmin@blog.com", true, false, null, "SUPERADMIN@BLOG.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAENGlb2NWLPhvczdtV36mSscmi7kmvT4w/d2DsT2xL8sPWuqu6uhJseNwYZzUQi45kw==", null, false, "2dccddc7-cd99-4719-9223-d4c5ca1391fd", false, "Superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "4ab90107-b1d0-49e8-9a97-c58a7d250a20", "486faf3b-3eb1-4fbe-b2ab-922cdc5f0bc9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c6d49bef-a693-4c6c-8ecf-3442da793a9f");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4ab90107-b1d0-49e8-9a97-c58a7d250a20", "486faf3b-3eb1-4fbe-b2ab-922cdc5f0bc9" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4ab90107-b1d0-49e8-9a97-c58a7d250a20");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "486faf3b-3eb1-4fbe-b2ab-922cdc5f0bc9");

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
    }
}
