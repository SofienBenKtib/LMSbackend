using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eduflowbackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class defaultrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resource_User_CreatorId",
                table: "Resource");

            migrationBuilder.DropForeignKey(
                name: "FK_Session_User_InstructorId",
                table: "Session");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Session_SessionId",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_User_SessionId",
                table: "Users",
                newName: "IX_Users_SessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resource_Users_CreatorId",
                table: "Resource",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Session_Users_InstructorId",
                table: "Session",
                column: "InstructorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Session_SessionId",
                table: "Users",
                column: "SessionId",
                principalTable: "Session",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resource_Users_CreatorId",
                table: "Resource");

            migrationBuilder.DropForeignKey(
                name: "FK_Session_Users_InstructorId",
                table: "Session");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Session_SessionId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_Users_SessionId",
                table: "User",
                newName: "IX_User_SessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resource_User_CreatorId",
                table: "Resource",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Session_User_InstructorId",
                table: "Session",
                column: "InstructorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Session_SessionId",
                table: "User",
                column: "SessionId",
                principalTable: "Session",
                principalColumn: "Id");
        }
    }
}
