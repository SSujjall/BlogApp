using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "726fdab9-7b71-4060-8744-37874a21b3a5");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "94a3707c-e2f0-40e7-8150-5cc90dba1191", "19cac0bf-399e-464d-b28a-b1f0726987f7" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "94a3707c-e2f0-40e7-8150-5cc90dba1191");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "19cac0bf-399e-464d-b28a-b1f0726987f7");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiry",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiry",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "726fdab9-7b71-4060-8744-37874a21b3a5", "726fdab9-7b71-4060-8744-37874a21b3a5", "Admin", "ADMIN" },
                    { "94a3707c-e2f0-40e7-8150-5cc90dba1191", "94a3707c-e2f0-40e7-8150-5cc90dba1191", "Superadmin", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "19cac0bf-399e-464d-b28a-b1f0726987f7", 0, "03d78afa-b9df-473b-b74f-c031d073e2a6", "Users", "superadmin@blog.com", true, false, null, "SUPERADMIN@BLOG.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEFcSrUP4djopTG3bA8MrDWHvCmQSxq1+T0zy6hquIkn4DZI/45k676D3bzhQfhmnAg==", null, false, "79ef42d4-6da1-43e7-b4b5-b80853183a60", false, "Superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "94a3707c-e2f0-40e7-8150-5cc90dba1191", "19cac0bf-399e-464d-b28a-b1f0726987f7" });
        }
    }
}
