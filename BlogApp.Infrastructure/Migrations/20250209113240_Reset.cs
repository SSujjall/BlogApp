using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Reset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3a2b302-1dd5-4cb6-b83e-c8a17dcf3463");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "86a6e37f-fb9b-4237-92ec-95a0b47ba8a9", "b01217fa-ec15-4bfc-820c-150d52d664dd" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "86a6e37f-fb9b-4237-92ec-95a0b47ba8a9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b01217fa-ec15-4bfc-820c-150d52d664dd");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1dee0a26-1a5b-41d2-b89c-a5d7d5990b17", "1dee0a26-1a5b-41d2-b89c-a5d7d5990b17", "Superadmin", "SUPERADMIN" },
                    { "e350a572-100a-4f2d-b7a0-074f8d3ab64a", "e350a572-100a-4f2d-b7a0-074f8d3ab64a", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "011fd361-3503-4b49-bcdc-b4b3fa6232a9", 0, "cd152af3-31e5-410a-bae8-58e476b77c5c", "Users", "superadmin@blog.com", true, false, null, "SUPERADMIN@BLOG.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEGzss2MYFR8azmYIEz7jq2NI9QOF+iz2vvqbnU/VbZ5OO+KKC3UKZhQSDcUV1y660w==", null, false, "777b6f80-0d25-403c-a10c-74609fb65a79", false, "Superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1dee0a26-1a5b-41d2-b89c-a5d7d5990b17", "011fd361-3503-4b49-bcdc-b4b3fa6232a9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e350a572-100a-4f2d-b7a0-074f8d3ab64a");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1dee0a26-1a5b-41d2-b89c-a5d7d5990b17", "011fd361-3503-4b49-bcdc-b4b3fa6232a9" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1dee0a26-1a5b-41d2-b89c-a5d7d5990b17");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "011fd361-3503-4b49-bcdc-b4b3fa6232a9");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "86a6e37f-fb9b-4237-92ec-95a0b47ba8a9", "86a6e37f-fb9b-4237-92ec-95a0b47ba8a9", "Superadmin", "SUPERADMIN" },
                    { "b3a2b302-1dd5-4cb6-b83e-c8a17dcf3463", "b3a2b302-1dd5-4cb6-b83e-c8a17dcf3463", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "b01217fa-ec15-4bfc-820c-150d52d664dd", 0, "1e55c6d3-7bc0-46bf-9528-5493fa2e27ce", "Users", "superadmin@blog.com", true, false, null, "SUPERADMIN@BLOG.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEO2RjohGa0PKgbQtynUxX6KaIAW0EpzQoKrDknAmByKV/OAOtTTG+GDgsMmTJmHp6A==", null, false, "a10e7908-25c7-4a06-a406-395de3bddeb1", false, "Superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "86a6e37f-fb9b-4237-92ec-95a0b47ba8a9", "b01217fa-ec15-4bfc-820c-150d52d664dd" });
        }
    }
}
