using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class commentreactioncounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "DownVoteCount",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UpVoteCount",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "DownVoteCount",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UpVoteCount",
                table: "Comments");

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
    }
}
