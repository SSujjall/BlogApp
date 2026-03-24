using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDataTypeForAmountsFromDoubleToDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "abfc82f4-07b6-4696-8f13-595e61e4aae1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bbd31fc0-9a5d-43be-be97-ecebb8bdbb8a");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f6012cfb-11d8-4903-b39c-a8cd7f341257", "e2296a08-0f30-4695-b309-ab31c4a229c2" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6012cfb-11d8-4903-b39c-a8cd7f341257");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e2296a08-0f30-4695-b309-ab31c4a229c2");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Refunds",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "Refunds",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "Payments",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "Orders",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "abfc82f4-07b6-4696-8f13-595e61e4aae1", "abfc82f4-07b6-4696-8f13-595e61e4aae1", "User", "USER" },
                    { "bbd31fc0-9a5d-43be-be97-ecebb8bdbb8a", "bbd31fc0-9a5d-43be-be97-ecebb8bdbb8a", "Admin", "ADMIN" },
                    { "f6012cfb-11d8-4903-b39c-a8cd7f341257", "f6012cfb-11d8-4903-b39c-a8cd7f341257", "Superadmin", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CurrentSubscriptionId", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "e2296a08-0f30-4695-b309-ab31c4a229c2", 0, "775681ed-14ef-4549-937b-e6c3d0528999", 1, "Users", "superadmin@blog.com", true, false, null, "SUPERADMIN@BLOG.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEAFuc+bQ/ikVjtYWiUKQ6hYutVWPBA0ckhNa/6cWPOOwfA9N+2CtpVSZyX4UWfqOvA==", null, false, null, null, "f3685291-bafb-42d7-824b-3e679c18ec9b", false, "Superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "f6012cfb-11d8-4903-b39c-a8cd7f341257", "e2296a08-0f30-4695-b309-ab31c4a229c2" });
        }
    }
}
