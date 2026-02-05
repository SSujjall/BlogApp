using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedUserRoleToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5d8062f4-2da2-4365-a349-5207bfc226bf");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ff7b1325-a9af-4082-97c7-30dd4986c7b1", "24f6962c-1996-4638-bfb4-9360279be5dd" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ff7b1325-a9af-4082-97c7-30dd4986c7b1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "24f6962c-1996-4638-bfb4-9360279be5dd");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2e9803a3-b500-44cf-a361-ec546b8550cd", "2e9803a3-b500-44cf-a361-ec546b8550cd", "Superadmin", "SUPERADMIN" },
                    { "6412e810-0e11-452c-9574-e08c6914b137", "6412e810-0e11-452c-9574-e08c6914b137", "Admin", "ADMIN" },
                    { "d936edc5-7bed-4559-a62e-df3ec05e1c4c", "d936edc5-7bed-4559-a62e-df3ec05e1c4c", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "24a67db5-81c3-45ca-9ac0-79e84227503a", 0, "42b53dea-6174-48dc-8ccf-2600ae001b95", "Users", "superadmin@blog.com", true, false, null, "SUPERADMIN@BLOG.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAELuBFMrnffXTf7iaoP0nWQHYppsutcrvhM78/f+Yu4Hd1TLQ1RtCCcZREHJyIUDC5g==", null, false, null, null, "accb7ccb-4f93-4ec8-8bb1-17e7449d9c79", false, "Superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "2e9803a3-b500-44cf-a361-ec546b8550cd", "24a67db5-81c3-45ca-9ac0-79e84227503a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6412e810-0e11-452c-9574-e08c6914b137");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d936edc5-7bed-4559-a62e-df3ec05e1c4c");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2e9803a3-b500-44cf-a361-ec546b8550cd", "24a67db5-81c3-45ca-9ac0-79e84227503a" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2e9803a3-b500-44cf-a361-ec546b8550cd");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "24a67db5-81c3-45ca-9ac0-79e84227503a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5d8062f4-2da2-4365-a349-5207bfc226bf", "5d8062f4-2da2-4365-a349-5207bfc226bf", "Admin", "ADMIN" },
                    { "ff7b1325-a9af-4082-97c7-30dd4986c7b1", "ff7b1325-a9af-4082-97c7-30dd4986c7b1", "Superadmin", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "24f6962c-1996-4638-bfb4-9360279be5dd", 0, "c1a07b16-49f4-454b-abbf-5229becabd61", "Users", "superadmin@blog.com", true, false, null, "SUPERADMIN@BLOG.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEImoz34AwHuUZCEB+fMthmH+f36xCDo4GhLlKYf3WP7JVp55IFElnc9GjSg2454mxw==", null, false, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "08740e03-55fa-434f-a358-d6c204aa601e", false, "Superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "ff7b1325-a9af-4082-97c7-30dd4986c7b1", "24f6962c-1996-4638-bfb4-9360279be5dd" });
        }
    }
}
