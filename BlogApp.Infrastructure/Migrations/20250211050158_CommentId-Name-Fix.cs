using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CommentIdNameFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "049ce140-81c4-43ed-b6b1-b563db5e74a0");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "97b0f48b-ef99-4176-9e9c-5454dd6ebd19", "78cbc9d5-275a-4d93-a3c6-18bf3fe98ac7" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "97b0f48b-ef99-4176-9e9c-5454dd6ebd19");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "78cbc9d5-275a-4d93-a3c6-18bf3fe98ac7");

            migrationBuilder.RenameColumn(
                name: "CommendId",
                table: "Comments",
                newName: "CommentId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "f7d66ddd-1d47-43ff-ac03-0963fff7f7c5", "f7d66ddd-1d47-43ff-ac03-0963fff7f7c5", "Admin", "ADMIN" },
                    { "ffb16d35-eff3-4598-9302-97a1527cd1fd", "ffb16d35-eff3-4598-9302-97a1527cd1fd", "Superadmin", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "5ba0581e-0d8d-45b6-b1e8-c56c55f0406d", 0, "c0b374be-4343-4140-a5e0-ec14c7c66aa5", "Users", "superadmin@blog.com", true, false, null, "SUPERADMIN@BLOG.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEDj8QgcwV4qOm5NBozYIoWnWNVvNMO1jyoVtbtsj7N/iPUSfbWiQsCx/yDohPHPcEg==", null, false, "60c1f831-2b19-43f6-967e-b8a1d99ba645", false, "Superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "ffb16d35-eff3-4598-9302-97a1527cd1fd", "5ba0581e-0d8d-45b6-b1e8-c56c55f0406d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f7d66ddd-1d47-43ff-ac03-0963fff7f7c5");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ffb16d35-eff3-4598-9302-97a1527cd1fd", "5ba0581e-0d8d-45b6-b1e8-c56c55f0406d" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ffb16d35-eff3-4598-9302-97a1527cd1fd");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "5ba0581e-0d8d-45b6-b1e8-c56c55f0406d");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Comments",
                newName: "CommendId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "049ce140-81c4-43ed-b6b1-b563db5e74a0", "049ce140-81c4-43ed-b6b1-b563db5e74a0", "Admin", "ADMIN" },
                    { "97b0f48b-ef99-4176-9e9c-5454dd6ebd19", "97b0f48b-ef99-4176-9e9c-5454dd6ebd19", "Superadmin", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "78cbc9d5-275a-4d93-a3c6-18bf3fe98ac7", 0, "c202c774-363a-4d96-81ee-5b619827cc9a", "Users", "superadmin@blog.com", true, false, null, "SUPERADMIN@BLOG.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEP/ZgzQWAvMJia8URFOXtBH4dCxIoVja3f1rlL+pjdS445AjGxAgfYwaq0hQshCXew==", null, false, "e3cbf07c-07ec-4320-aec5-5dc9f39e5efc", false, "Superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "97b0f48b-ef99-4176-9e9c-5454dd6ebd19", "78cbc9d5-275a-4d93-a3c6-18bf3fe98ac7" });
        }
    }
}
