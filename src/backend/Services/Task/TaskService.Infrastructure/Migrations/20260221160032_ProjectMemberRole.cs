using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProjectMemberRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkspaceMembers",
                table: "WorkspaceMembers");

            migrationBuilder.DropIndex(
                name: "IX_WorkspaceMembers_WorkspaceId",
                table: "WorkspaceMembers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkspaceMembers",
                table: "WorkspaceMembers",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceMembers_UserId",
                table: "WorkspaceMembers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkspaceMembers",
                table: "WorkspaceMembers");

            migrationBuilder.DropIndex(
                name: "IX_WorkspaceMembers_UserId",
                table: "WorkspaceMembers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkspaceMembers",
                table: "WorkspaceMembers",
                columns: new[] { "UserId", "WorkspaceId" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceMembers_WorkspaceId",
                table: "WorkspaceMembers",
                column: "WorkspaceId");
        }
    }
}
