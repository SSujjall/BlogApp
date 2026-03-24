using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExternalTransactionIdPropertyMadeNullableInPaymentsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1800143d-b1de-4280-a1c0-d3ece064f386");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "96fd2b9b-3cdc-4d99-b4c1-1e53b4041af3");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "18bbf262-1422-46ea-bb9a-47b200b041a3", "4b20ca5e-1f1d-481a-aab1-71dd67eaf1ee" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "18bbf262-1422-46ea-bb9a-47b200b041a3");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b20ca5e-1f1d-481a-aab1-71dd67eaf1ee");

            migrationBuilder.AlterColumn<string>(
                name: "ExternalTransactionId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "ExternalTransactionId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1800143d-b1de-4280-a1c0-d3ece064f386", "1800143d-b1de-4280-a1c0-d3ece064f386", "Admin", "ADMIN" },
                    { "18bbf262-1422-46ea-bb9a-47b200b041a3", "18bbf262-1422-46ea-bb9a-47b200b041a3", "Superadmin", "SUPERADMIN" },
                    { "96fd2b9b-3cdc-4d99-b4c1-1e53b4041af3", "96fd2b9b-3cdc-4d99-b4c1-1e53b4041af3", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CurrentSubscriptionId", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "4b20ca5e-1f1d-481a-aab1-71dd67eaf1ee", 0, "749562ae-fd30-49bd-9361-1d9662d280c7", 1, "Users", "superadmin@blog.com", true, false, null, "SUPERADMIN@BLOG.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEHfL6Kqb4NLLvBp3o6tqDN0mSqf9C3ekltJzz1AAAbfgxu4z6VIcCT4W7mwrdB2ljA==", null, false, null, null, "a06b6ebe-f0eb-4a89-9444-3f6c99046538", false, "Superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "18bbf262-1422-46ea-bb9a-47b200b041a3", "4b20ca5e-1f1d-481a-aab1-71dd67eaf1ee" });
        }
    }
}
