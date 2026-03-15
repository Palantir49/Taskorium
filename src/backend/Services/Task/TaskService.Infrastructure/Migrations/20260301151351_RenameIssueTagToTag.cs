using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameIssueTagToTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssueTag_Projects_ProjectId",
                table: "IssueTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IssueTag",
                table: "IssueTag");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "IssueTag");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "IssueTag");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "IssueTag");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "IssueStatus");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "IssueTag",
                newName: "TagsId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "IssueTag",
                newName: "IssuesId");

            migrationBuilder.RenameIndex(
                name: "IX_IssueTag_ProjectId",
                table: "IssueTag",
                newName: "IX_IssueTag_TagsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IssueTag",
                table: "IssueTag",
                columns: new[] { "IssuesId", "TagsId" });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tag_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tag_ProjectId",
                table: "Tag",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_IssueTag_Issues_IssuesId",
                table: "IssueTag",
                column: "IssuesId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IssueTag_Tag_TagsId",
                table: "IssueTag",
                column: "TagsId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssueTag_Issues_IssuesId",
                table: "IssueTag");

            migrationBuilder.DropForeignKey(
                name: "FK_IssueTag_Tag_TagsId",
                table: "IssueTag");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IssueTag",
                table: "IssueTag");

            migrationBuilder.RenameColumn(
                name: "TagsId",
                table: "IssueTag",
                newName: "ProjectId");

            migrationBuilder.RenameColumn(
                name: "IssuesId",
                table: "IssueTag",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_IssueTag_TagsId",
                table: "IssueTag",
                newName: "IX_IssueTag_ProjectId");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "IssueTag",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "IssueTag",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "IssueTag",
                type: "character varying(225)",
                maxLength: 225,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "IssueStatus",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_IssueTag",
                table: "IssueTag",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IssueTag_Projects_ProjectId",
                table: "IssueTag",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
