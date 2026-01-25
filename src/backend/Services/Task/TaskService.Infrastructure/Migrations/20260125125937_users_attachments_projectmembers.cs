using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class users_attachments_projectmembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReporterId",
                table: "Issues");

            migrationBuilder.RenameColumn(
                name: "TaskTypeId",
                table: "Issues",
                newName: "IssueTypeId");

            migrationBuilder.RenameColumn(
                name: "TaskStatusId",
                table: "Issues",
                newName: "IssueStatusId");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KeycloakId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: false),
                    Username = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IssueId = table.Column<Guid>(type: "uuid", nullable: false),
                    UploaderId = table.Column<Guid>(type: "uuid", nullable: false),
                    StoragePath = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attachments_Users_UploaderId",
                        column: x => x.UploaderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMembers",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    JoinedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMembers", x => new { x.ProjectId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workspaces_OwnerId",
                table: "Workspaces",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_WorkspaceId",
                table: "Projects",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_IssueId",
                table: "Attachments",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_UploaderId",
                table: "Attachments",
                column: "UploaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_UserId",
                table: "ProjectMembers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Workspaces_WorkspaceId",
                table: "Projects",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workspaces_Users_OwnerId",
                table: "Workspaces",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Workspaces_WorkspaceId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Workspaces_Users_OwnerId",
                table: "Workspaces");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "ProjectMembers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Workspaces_OwnerId",
                table: "Workspaces");

            migrationBuilder.DropIndex(
                name: "IX_Projects_WorkspaceId",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "IssueTypeId",
                table: "Issues",
                newName: "TaskTypeId");

            migrationBuilder.RenameColumn(
                name: "IssueStatusId",
                table: "Issues",
                newName: "TaskStatusId");

            migrationBuilder.AddColumn<Guid>(
                name: "ReporterId",
                table: "Issues",
                type: "uuid",
                nullable: true);
        }
    }
}
