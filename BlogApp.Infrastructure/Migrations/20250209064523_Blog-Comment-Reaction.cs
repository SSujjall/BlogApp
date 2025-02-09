using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BlogCommentReaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c6d49bef-a693-4c6c-8ecf-3442da793a9f");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4ab90107-b1d0-49e8-9a97-c58a7d250a20", "486faf3b-3eb1-4fbe-b2ab-922cdc5f0bc9" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4ab90107-b1d0-49e8-9a97-c58a7d250a20");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "486faf3b-3eb1-4fbe-b2ab-922cdc5f0bc9");

            migrationBuilder.RenameColumn(
                name: "CommentReactionId",
                table: "CommentReactions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "BlogReactionId",
                table: "BlogReactions",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CommentReactions",
                newName: "CommentReactionId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "BlogReactions",
                newName: "BlogReactionId");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Blogs",
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
                    { "4ab90107-b1d0-49e8-9a97-c58a7d250a20", "4ab90107-b1d0-49e8-9a97-c58a7d250a20", "Superadmin", "SUPERADMIN" },
                    { "c6d49bef-a693-4c6c-8ecf-3442da793a9f", "c6d49bef-a693-4c6c-8ecf-3442da793a9f", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "486faf3b-3eb1-4fbe-b2ab-922cdc5f0bc9", 0, "227a1ba7-dace-45c8-8e55-2a3478b9e031", "User", "superadmin@blog.com", true, false, null, "SUPERADMIN@BLOG.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAENGlb2NWLPhvczdtV36mSscmi7kmvT4w/d2DsT2xL8sPWuqu6uhJseNwYZzUQi45kw==", null, false, "2dccddc7-cd99-4719-9223-d4c5ca1391fd", false, "Superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "4ab90107-b1d0-49e8-9a97-c58a7d250a20", "486faf3b-3eb1-4fbe-b2ab-922cdc5f0bc9" });
        }
    }
}
