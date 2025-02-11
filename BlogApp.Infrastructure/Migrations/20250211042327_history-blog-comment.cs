using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class historyblogcomment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentHistories_AspNetUsers_ModifiedByUserId",
                table: "CommentHistories");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e350a572-100a-4f2d-b7a0-074f8d3ab64a");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1dee0a26-1a5b-41d2-b89c-a5d7d5990b17", "011fd361-3503-4b49-bcdc-b4b3fa6232a9" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1dee0a26-1a5b-41d2-b89c-a5d7d5990b17");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "011fd361-3503-4b49-bcdc-b4b3fa6232a9");

            migrationBuilder.RenameColumn(
                name: "ModifiedByUserId",
                table: "CommentHistories",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentHistories_ModifiedByUserId",
                table: "CommentHistories",
                newName: "IX_CommentHistories_UserId");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BlogHistories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.CreateIndex(
                name: "IX_BlogHistories_UserId",
                table: "BlogHistories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogHistories_AspNetUsers_UserId",
                table: "BlogHistories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentHistories_AspNetUsers_UserId",
                table: "CommentHistories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogHistories_AspNetUsers_UserId",
                table: "BlogHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentHistories_AspNetUsers_UserId",
                table: "CommentHistories");

            migrationBuilder.DropIndex(
                name: "IX_BlogHistories_UserId",
                table: "BlogHistories");

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

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BlogHistories");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CommentHistories",
                newName: "ModifiedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentHistories_UserId",
                table: "CommentHistories",
                newName: "IX_CommentHistories_ModifiedByUserId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1dee0a26-1a5b-41d2-b89c-a5d7d5990b17", "1dee0a26-1a5b-41d2-b89c-a5d7d5990b17", "Superadmin", "SUPERADMIN" },
                    { "e350a572-100a-4f2d-b7a0-074f8d3ab64a", "e350a572-100a-4f2d-b7a0-074f8d3ab64a", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "011fd361-3503-4b49-bcdc-b4b3fa6232a9", 0, "cd152af3-31e5-410a-bae8-58e476b77c5c", "Users", "superadmin@blog.com", true, false, null, "SUPERADMIN@BLOG.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEGzss2MYFR8azmYIEz7jq2NI9QOF+iz2vvqbnU/VbZ5OO+KKC3UKZhQSDcUV1y660w==", null, false, "777b6f80-0d25-403c-a10c-74609fb65a79", false, "Superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1dee0a26-1a5b-41d2-b89c-a5d7d5990b17", "011fd361-3503-4b49-bcdc-b4b3fa6232a9" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommentHistories_AspNetUsers_ModifiedByUserId",
                table: "CommentHistories",
                column: "ModifiedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
