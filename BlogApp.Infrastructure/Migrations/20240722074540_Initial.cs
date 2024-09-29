using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7c997d63-f1a7-4c87-a552-bb599ea536ad", "996fcf89-4920-43ef-a957-1106e3eb91a0" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c997d63-f1a7-4c87-a552-bb599ea536ad");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "996fcf89-4920-43ef-a957-1106e3eb91a0");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "70538d21-b4e6-4a01-bded-49977e433ac4", "70538d21-b4e6-4a01-bded-49977e433ac4", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "430bf0eb-54c3-47bb-9f9c-1cf0793c44c0", 0, "5cd66456-d623-4ec6-891f-625afb56a2b0", "User", "admin@user.com", true, false, null, "ADMIN@USER.COM", "ADMIN@USER.COM", "AQAAAAIAAYagAAAAEBKoiC9UbnD2FY+nquBT5irr88yMYbAf/X1VrzqABAiCo5qv+WZwjCAKFfzeDw52IA==", null, false, "fe950ab6-74ad-4356-9894-285f5106dcaa", false, "admin@user.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "70538d21-b4e6-4a01-bded-49977e433ac4", "430bf0eb-54c3-47bb-9f9c-1cf0793c44c0" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "70538d21-b4e6-4a01-bded-49977e433ac4", "430bf0eb-54c3-47bb-9f9c-1cf0793c44c0" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "70538d21-b4e6-4a01-bded-49977e433ac4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "430bf0eb-54c3-47bb-9f9c-1cf0793c44c0");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7c997d63-f1a7-4c87-a552-bb599ea536ad", "7c997d63-f1a7-4c87-a552-bb599ea536ad", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "996fcf89-4920-43ef-a957-1106e3eb91a0", 0, "67482450-2776-44cd-a3ae-1854efd6462b", "User", "admin@user.com", true, false, null, "ADMIN@USER.COM", "ADMIN@USER.COM", "AQAAAAIAAYagAAAAEAJsqocunneR81meRQnKdYIpMGNVKWA0rdkLQtxX8r2+zaNEARX3L7VVFsF9dHOCdA==", null, false, "5805960c-c3d1-4b54-9c37-86bf6fa7b8db", false, "admin@user.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "7c997d63-f1a7-4c87-a552-bb599ea536ad", "996fcf89-4920-43ef-a957-1106e3eb91a0" });
        }
    }
}
