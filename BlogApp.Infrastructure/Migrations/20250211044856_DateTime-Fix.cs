using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DateTimeFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d8ee08d2-73cf-4de6-8ea4-ee75be216397");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "e484c5a2-89c0-43ff-881b-14f66dc82a64", "3820b8a9-74bd-4889-ad02-87fd924d1f2c" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e484c5a2-89c0-43ff-881b-14f66dc82a64");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3820b8a9-74bd-4889-ad02-87fd924d1f2c");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CommentReactions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "CommentHistories",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Blogs",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Blogs",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "BlogReactions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "BlogHistories",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedAt",
                table: "Notifications",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "UpdatedAt",
                table: "Comments",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedAt",
                table: "Comments",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedAt",
                table: "CommentReactions",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "UpdatedAt",
                table: "CommentHistories",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "UpdatedAt",
                table: "Blogs",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedAt",
                table: "Blogs",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedAt",
                table: "BlogReactions",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedAt",
                table: "BlogHistories",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d8ee08d2-73cf-4de6-8ea4-ee75be216397", "d8ee08d2-73cf-4de6-8ea4-ee75be216397", "Admin", "ADMIN" },
                    { "e484c5a2-89c0-43ff-881b-14f66dc82a64", "e484c5a2-89c0-43ff-881b-14f66dc82a64", "Superadmin", "SUPERADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "3820b8a9-74bd-4889-ad02-87fd924d1f2c", 0, "d1b1f881-1e14-4e36-ad88-69199f6b30c9", "Users", "superadmin@blog.com", true, false, null, "SUPERADMIN@BLOG.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEE8aC8B+R1mriZpK2Lv2lEzyq67st0kyrUT7t23DPbOVJp/h5NmkP6O2VbPnXlbzCA==", null, false, "08638734-1f95-4558-8cd1-16e488c543eb", false, "Superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "e484c5a2-89c0-43ff-881b-14f66dc82a64", "3820b8a9-74bd-4889-ad02-87fd924d1f2c" });
        }
    }
}
