using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CommentCountAddedInBlogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c0af7f6d-91f9-40e1-b9b9-c85bdafb7ece");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7364b46d-5e01-48a3-8551-87daf7d21503", "497ed5ea-ec6c-46cf-a1d0-d46b7606504a" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7364b46d-5e01-48a3-8551-87daf7d21503");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "497ed5ea-ec6c-46cf-a1d0-d46b7606504a");

            migrationBuilder.AddColumn<int>(
                name: "CommentCount",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "CommentCount",
                table: "Blogs");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7364b46d-5e01-48a3-8551-87daf7d21503", "7364b46d-5e01-48a3-8551-87daf7d21503", "Superadmin", "SUPERADMIN" },
                    { "c0af7f6d-91f9-40e1-b9b9-c85bdafb7ece", "c0af7f6d-91f9-40e1-b9b9-c85bdafb7ece", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "497ed5ea-ec6c-46cf-a1d0-d46b7606504a", 0, "6a143213-3d24-47fe-9138-6b563d09b776", "Users", "superadmin@blog.com", true, false, null, "SUPERADMIN@BLOG.COM", "SUPERADMIN", "AQAAAAIAAYagAAAAEE9GP4pi/1X2qmyIcbmJir0asoMWuhf1rrUQugv8uTSDCqCyfgYOfFmQob91x4LmWw==", null, false, "8bcd8fd7-34cd-48bd-8ab0-956cf64e61e3", false, "Superadmin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "7364b46d-5e01-48a3-8551-87daf7d21503", "497ed5ea-ec6c-46cf-a1d0-d46b7606504a" });
        }
    }
}
