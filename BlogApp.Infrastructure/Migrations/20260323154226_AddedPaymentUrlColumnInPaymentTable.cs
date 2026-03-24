using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPaymentUrlColumnInPaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1ea04015-9169-491b-9e2d-3e17c4e9bd57");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "690bf01a-7322-4dda-a130-94bd0fbce190");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "928113f9-866e-444f-99da-637f74bd2569", "b1c050e4-58ba-4dea-9f71-1d946ada6f9f" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "928113f9-866e-444f-99da-637f74bd2569");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b1c050e4-58ba-4dea-9f71-1d946ada6f9f");

            migrationBuilder.AddColumn<string>(
                name: "PaymentUrl",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "50e512b9-ae15-403a-8eb6-155e211d30fc", "50e512b9-ae15-403a-8eb6-155e211d30fc", "Admin", "ADMIN" },
                    { "bf610df8-18da-4f10-bbb2-f4667aee5427", "bf610df8-18da-4f10-bbb2-f4667aee5427", "Superadmin", "SUPERADMIN" },
                    { "de5eeb41-4885-4e91-9fbd-3ac39a0766a8", "de5eeb41-4885-4e91-9fbd-3ac39a0766a8", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CurrentSubscriptionId", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "07034e3e-80cf-4e84-9416-6be0d9c843b3", 0, "9d307c92-d38d-4c46-8781-8374dfb0a824", 1, "Users", "superadmin@blog.com", true, false, null, "SUPERADMIN@BLOG.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEHxJAH1GMvwnuoiOVgisjC1M5TXcZG7RBlM//AnJU1nQrKGam5WjEZzPUL8gbgFgNQ==", null, false, null, null, "0b94672d-2c6c-49c1-afd6-0a38ab470a9d", false, "Superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "bf610df8-18da-4f10-bbb2-f4667aee5427", "07034e3e-80cf-4e84-9416-6be0d9c843b3" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "50e512b9-ae15-403a-8eb6-155e211d30fc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "de5eeb41-4885-4e91-9fbd-3ac39a0766a8");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "bf610df8-18da-4f10-bbb2-f4667aee5427", "07034e3e-80cf-4e84-9416-6be0d9c843b3" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bf610df8-18da-4f10-bbb2-f4667aee5427");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "07034e3e-80cf-4e84-9416-6be0d9c843b3");

            migrationBuilder.DropColumn(
                name: "PaymentUrl",
                table: "Payments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1ea04015-9169-491b-9e2d-3e17c4e9bd57", "1ea04015-9169-491b-9e2d-3e17c4e9bd57", "Admin", "ADMIN" },
                    { "690bf01a-7322-4dda-a130-94bd0fbce190", "690bf01a-7322-4dda-a130-94bd0fbce190", "User", "USER" },
                    { "928113f9-866e-444f-99da-637f74bd2569", "928113f9-866e-444f-99da-637f74bd2569", "Superadmin", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CurrentSubscriptionId", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "b1c050e4-58ba-4dea-9f71-1d946ada6f9f", 0, "3dfd2e3f-91c7-4d0c-a1d6-a46dc8379fb4", 1, "Users", "superadmin@blog.com", true, false, null, "SUPERADMIN@BLOG.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAENbiBOsmYiTG/2GWzjRd4sJE163tE78n5Qn3B3tQZ2HL0f2pPP2HcSFLNaLdfK2nhw==", null, false, null, null, "c1947de9-7229-4298-9ac1-40d0be82aa3d", false, "Superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "928113f9-866e-444f-99da-637f74bd2569", "b1c050e4-58ba-4dea-9f71-1d946ada6f9f" });
        }
    }
}
