using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDatatypeForSubscriptionPriceProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6fbb6300-b971-43cf-9a73-36227ce07b19");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "753ba3c2-28ba-4e1f-9241-2f892285f641");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "d929650e-d8d3-4d75-9737-159bbb090bee", "3d254e77-12f1-4980-9cc6-e3299cc4d326" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d929650e-d8d3-4d75-9737-159bbb090bee");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3d254e77-12f1-4980-9cc6-e3299cc4d326");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Subscriptions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

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

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 1,
                column: "Price",
                value: 0.00m);

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "f6012cfb-11d8-4903-b39c-a8cd7f341257", "e2296a08-0f30-4695-b309-ab31c4a229c2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Subscriptions",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6fbb6300-b971-43cf-9a73-36227ce07b19", "6fbb6300-b971-43cf-9a73-36227ce07b19", "Admin", "ADMIN" },
                    { "753ba3c2-28ba-4e1f-9241-2f892285f641", "753ba3c2-28ba-4e1f-9241-2f892285f641", "User", "USER" },
                    { "d929650e-d8d3-4d75-9737-159bbb090bee", "d929650e-d8d3-4d75-9737-159bbb090bee", "Superadmin", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CurrentSubscriptionId", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "3d254e77-12f1-4980-9cc6-e3299cc4d326", 0, "a3552756-a448-4b07-9e15-7346936c628c", 1, "Users", "superadmin@blog.com", true, false, null, "SUPERADMIN@BLOG.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEF2k45EuAq+9b2iemsNBnXAALUEQzzdDZKSQ3yI/328x5uWBWJu5iV43UmoCsNCRSg==", null, false, null, null, "4b0571d8-451b-4462-93bb-47519636360e", false, "Superadmin" });

            migrationBuilder.UpdateData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 1,
                column: "Price",
                value: 0.0);

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "d929650e-d8d3-4d75-9737-159bbb090bee", "3d254e77-12f1-4980-9cc6-e3299cc4d326" });
        }
    }
}
