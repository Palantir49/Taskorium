using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class remove_ownerId_from_workspace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workspaces_Users_OwnerId",
                table: "Workspaces");

            migrationBuilder.DropTable(
                name: "IssueType");

            migrationBuilder.DropIndex(
                name: "IX_Workspaces_OwnerId",
                table: "Workspaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkspaceMembers",
                table: "WorkspaceMembers");

            migrationBuilder.DropIndex(
                name: "IX_WorkspaceMembers_UserId",
                table: "WorkspaceMembers");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "IssueStatus");

            migrationBuilder.DropColumn(
                name: "IssueTypeId",
                table: "Issues");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Projects",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "Projects",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IssuePriority",
                table: "Issues",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IssueType",
                table: "Issues",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Issues",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartDate",
                table: "Issues",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkspaceMembers",
                table: "WorkspaceMembers",
                columns: new[] { "UserId", "WorkspaceId" });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IssueTag",
                columns: table => new
                {
                    IssuesId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueTag", x => new { x.IssuesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_IssueTag_Issues_IssuesId",
                        column: x => x.IssuesId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IssueTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "Email", "FullName", "KeycloakId", "Username" },
                values: new object[,]
                {
                    { new Guid("019d399c-2a77-7365-b1a4-e737e821d4fb"), new DateTimeOffset(new DateTime(2026, 3, 29, 12, 40, 29, 47, DateTimeKind.Unspecified).AddTicks(5205), new TimeSpan(0, 0, 0, 0, 0)), "carol@example.com", "Carol Chen", new Guid("cccccccc-cccc-cccc-cccc-ccccccccccc3"), "carol" },
                    { new Guid("019d399c-2a77-77cf-8fa0-3b54e54910ff"), new DateTimeOffset(new DateTime(2026, 3, 29, 12, 40, 29, 47, DateTimeKind.Unspecified).AddTicks(4107), new TimeSpan(0, 0, 0, 0, 0)), "alice@example.com", "Alice Anderson", new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), "alice" },
                    { new Guid("019d399c-2a77-7d11-bbd4-db8c27757b25"), new DateTimeOffset(new DateTime(2026, 3, 29, 12, 40, 29, 47, DateTimeKind.Unspecified).AddTicks(5202), new TimeSpan(0, 0, 0, 0, 0)), "bob@example.com", "Bob Brown", new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"), "bob" }
                });

            migrationBuilder.InsertData(
                table: "Workspaces",
                columns: new[] { "Id", "CreatedDate", "Name" },
                values: new object[] { new Guid("019d399c-2a77-79fd-9ff7-acf17ea6bc2e"), new DateTimeOffset(new DateTime(2026, 3, 29, 12, 40, 29, 47, DateTimeKind.Unspecified).AddTicks(6992), new TimeSpan(0, 0, 0, 0, 0)), "Acme Workspace" });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Abbreviation", "CreatedDate", "Description", "FinishDate", "Name", "StartDate", "WorkspaceId" },
                values: new object[] { new Guid("019d399c-2a77-7614-a948-620f1191a855"), "MOB", new DateTimeOffset(new DateTime(2026, 3, 29, 12, 40, 29, 47, DateTimeKind.Unspecified).AddTicks(9498), new TimeSpan(0, 0, 0, 0, 0)), "Customer-facing iOS and Android app.", null, "Mobile client", null, new Guid("019d399c-2a77-79fd-9ff7-acf17ea6bc2e") });

            migrationBuilder.InsertData(
                table: "WorkspaceMembers",
                columns: new[] { "UserId", "WorkspaceId", "JoinedAt", "Role" },
                values: new object[,]
                {
                    { new Guid("019d399c-2a77-7365-b1a4-e737e821d4fb"), new Guid("019d399c-2a77-79fd-9ff7-acf17ea6bc2e"), new DateTimeOffset(new DateTime(2026, 1, 17, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Member" },
                    { new Guid("019d399c-2a77-77cf-8fa0-3b54e54910ff"), new Guid("019d399c-2a77-79fd-9ff7-acf17ea6bc2e"), new DateTimeOffset(new DateTime(2026, 1, 15, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Creator" },
                    { new Guid("019d399c-2a77-7d11-bbd4-db8c27757b25"), new Guid("019d399c-2a77-79fd-9ff7-acf17ea6bc2e"), new DateTimeOffset(new DateTime(2026, 1, 16, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Admin" }
                });

            migrationBuilder.InsertData(
                table: "IssueStatus",
                columns: new[] { "Id", "CreatedDate", "Name", "Position", "ProjectId", "Type" },
                values: new object[,]
                {
                    { new Guid("019d399c-2a78-7197-b808-733763f23a62"), new DateTimeOffset(new DateTime(2026, 3, 29, 12, 40, 29, 48, DateTimeKind.Unspecified).AddTicks(1784), new TimeSpan(0, 0, 0, 0, 0)), "In progress", 1, new Guid("019d399c-2a77-7614-a948-620f1191a855"), 1 },
                    { new Guid("019d399c-2a78-774b-9570-9e62bc08f0a0"), new DateTimeOffset(new DateTime(2026, 3, 29, 12, 40, 29, 48, DateTimeKind.Unspecified).AddTicks(1786), new TimeSpan(0, 0, 0, 0, 0)), "Done", 2, new Guid("019d399c-2a77-7614-a948-620f1191a855"), 2 },
                    { new Guid("019d399c-2a78-7f43-9db3-0bf290c89ca6"), new DateTimeOffset(new DateTime(2026, 3, 29, 12, 40, 29, 48, DateTimeKind.Unspecified).AddTicks(1555), new TimeSpan(0, 0, 0, 0, 0)), "Backlog", 0, new Guid("019d399c-2a77-7614-a948-620f1191a855"), 0 }
                });

            migrationBuilder.InsertData(
                table: "Issues",
                columns: new[] { "Id", "CreatedDate", "Description", "DueDate", "IssuePriority", "IssueStatusId", "IssueType", "Key", "Name", "ProjectId", "ResolvedDate", "StartDate", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("019d399c-2a78-797f-a73a-3f9aa697f117"), new DateTimeOffset(new DateTime(2026, 3, 29, 12, 40, 29, 48, DateTimeKind.Unspecified).AddTicks(3808), new TimeSpan(0, 0, 0, 0, 0)), "Implement email and SSO login.", new DateTimeOffset(new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 3, new Guid("019d399c-2a78-7197-b808-733763f23a62"), 0, "MOB-1", "Login screen", new Guid("019d399c-2a77-7614-a948-620f1191a855"), null, null, null },
                    { new Guid("019d399c-2a7a-7ebc-8553-4464fb17c356"), new DateTimeOffset(new DateTime(2026, 3, 29, 12, 40, 29, 50, DateTimeKind.Unspecified).AddTicks(5567), new TimeSpan(0, 0, 0, 0, 0)), "Finalize strings for v1.", null, 1, new Guid("019d399c-2a78-774b-9570-9e62bc08f0a0"), 1, "MOB-3", "Push notification copy", new Guid("019d399c-2a77-7614-a948-620f1191a855"), null, null, null },
                    { new Guid("019d399c-2a7a-7ece-a77a-aff51fdd2458"), new DateTimeOffset(new DateTime(2026, 3, 29, 12, 40, 29, 50, DateTimeKind.Unspecified).AddTicks(5341), new TimeSpan(0, 0, 0, 0, 0)), "Reproduces on Android 14.", new DateTimeOffset(new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 4, new Guid("019d399c-2a78-7f43-9db3-0bf290c89ca6"), 2, "MOB-2", "Crash on cold start", new Guid("019d399c-2a77-7614-a948-620f1191a855"), null, null, null }
                });

            migrationBuilder.InsertData(
                table: "ProjectMembers",
                columns: new[] { "ProjectId", "UserId", "JoinedAt", "Role" },
                values: new object[,]
                {
                    { new Guid("019d399c-2a77-7614-a948-620f1191a855"), new Guid("019d399c-2a77-7365-b1a4-e737e821d4fb"), new DateTimeOffset(new DateTime(2026, 1, 18, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Viewer" },
                    { new Guid("019d399c-2a77-7614-a948-620f1191a855"), new Guid("019d399c-2a77-77cf-8fa0-3b54e54910ff"), new DateTimeOffset(new DateTime(2026, 1, 15, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Creator" },
                    { new Guid("019d399c-2a77-7614-a948-620f1191a855"), new Guid("019d399c-2a77-7d11-bbd4-db8c27757b25"), new DateTimeOffset(new DateTime(2026, 1, 16, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Member" }
                });

            migrationBuilder.InsertData(
                table: "Attachments",
                columns: new[] { "Id", "IssueId", "StoragePath", "UploaderId" },
                values: new object[,]
                {
                    { new Guid("019d399c-2a7a-75d1-b674-6b5a40ca34a7"), new Guid("019d399c-2a78-797f-a73a-3f9aa697f117"), "attachments/mob-1/wireframe.png", new Guid("019d399c-2a77-77cf-8fa0-3b54e54910ff") },
                    { new Guid("019d399c-2a7a-7ccf-aee6-3614b1d0492a"), new Guid("019d399c-2a7a-7ece-a77a-aff51fdd2458"), "attachments/mob-2/logcat.txt", new Guid("019d399c-2a77-7d11-bbd4-db8c27757b25") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceMembers_WorkspaceId",
                table: "WorkspaceMembers",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_IssueTag_TagsId",
                table: "IssueTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ProjectId",
                table: "Tags",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssueTag");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkspaceMembers",
                table: "WorkspaceMembers");

            migrationBuilder.DropIndex(
                name: "IX_WorkspaceMembers_WorkspaceId",
                table: "WorkspaceMembers");

            migrationBuilder.DeleteData(
                table: "Attachments",
                keyColumn: "Id",
                keyValue: new Guid("019d399c-2a7a-75d1-b674-6b5a40ca34a7"));

            migrationBuilder.DeleteData(
                table: "Attachments",
                keyColumn: "Id",
                keyValue: new Guid("019d399c-2a7a-7ccf-aee6-3614b1d0492a"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d399c-2a78-7197-b808-733763f23a62"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d399c-2a78-774b-9570-9e62bc08f0a0"));

            migrationBuilder.DeleteData(
                table: "IssueStatus",
                keyColumn: "Id",
                keyValue: new Guid("019d399c-2a78-7f43-9db3-0bf290c89ca6"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("019d399c-2a7a-7ebc-8553-4464fb17c356"));

            migrationBuilder.DeleteData(
                table: "ProjectMembers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("019d399c-2a77-7614-a948-620f1191a855"), new Guid("019d399c-2a77-7365-b1a4-e737e821d4fb") });

            migrationBuilder.DeleteData(
                table: "ProjectMembers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("019d399c-2a77-7614-a948-620f1191a855"), new Guid("019d399c-2a77-77cf-8fa0-3b54e54910ff") });

            migrationBuilder.DeleteData(
                table: "ProjectMembers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("019d399c-2a77-7614-a948-620f1191a855"), new Guid("019d399c-2a77-7d11-bbd4-db8c27757b25") });

            migrationBuilder.DeleteData(
                table: "WorkspaceMembers",
                keyColumns: new[] { "UserId", "WorkspaceId" },
                keyValues: new object[] { new Guid("019d399c-2a77-7365-b1a4-e737e821d4fb"), new Guid("019d399c-2a77-79fd-9ff7-acf17ea6bc2e") });

            migrationBuilder.DeleteData(
                table: "WorkspaceMembers",
                keyColumns: new[] { "UserId", "WorkspaceId" },
                keyValues: new object[] { new Guid("019d399c-2a77-77cf-8fa0-3b54e54910ff"), new Guid("019d399c-2a77-79fd-9ff7-acf17ea6bc2e") });

            migrationBuilder.DeleteData(
                table: "WorkspaceMembers",
                keyColumns: new[] { "UserId", "WorkspaceId" },
                keyValues: new object[] { new Guid("019d399c-2a77-7d11-bbd4-db8c27757b25"), new Guid("019d399c-2a77-79fd-9ff7-acf17ea6bc2e") });

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("019d399c-2a78-797f-a73a-3f9aa697f117"));

            migrationBuilder.DeleteData(
                table: "Issues",
                keyColumn: "Id",
                keyValue: new Guid("019d399c-2a7a-7ece-a77a-aff51fdd2458"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("019d399c-2a77-7365-b1a4-e737e821d4fb"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("019d399c-2a77-77cf-8fa0-3b54e54910ff"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("019d399c-2a77-7d11-bbd4-db8c27757b25"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("019d399c-2a77-7614-a948-620f1191a855"));

            migrationBuilder.DeleteData(
                table: "Workspaces",
                keyColumn: "Id",
                keyValue: new Guid("019d399c-2a77-79fd-9ff7-acf17ea6bc2e"));

            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IssuePriority",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "IssueType",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Issues");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Workspaces",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Projects",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "IssueStatus",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IssueTypeId",
                table: "Issues",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkspaceMembers",
                table: "WorkspaceMembers",
                column: "WorkspaceId");

            migrationBuilder.CreateTable(
                name: "IssueType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssueType_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workspaces_OwnerId",
                table: "Workspaces",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceMembers_UserId",
                table: "WorkspaceMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IssueType_ProjectId",
                table: "IssueType",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workspaces_Users_OwnerId",
                table: "Workspaces",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
