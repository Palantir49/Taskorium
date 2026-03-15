using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateIssueTypeToIssueTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssueType");

            migrationBuilder.RenameColumn(
                name: "IssueTypeId",
                table: "Issues",
                newName: "IssueTagId");

            migrationBuilder.AddColumn<int>(
                name: "IssueType",
                table: "Issues",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "IssueTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssueTag_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

          /*  migrationBuilder.CreateIndex(
                name: "IX_IssueStatus_ProjectId",
                table: "IssueStatus",
                column: "ProjectId");*/

            migrationBuilder.CreateIndex(
                name: "IX_IssueTag_ProjectId",
                table: "IssueTag",
                column: "ProjectId");

          /*  migrationBuilder.AddForeignKey(
                name: "FK_IssueStatus_Projects_ProjectId",
                table: "IssueStatus",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssueStatus_Projects_ProjectId",
                table: "IssueStatus");

            migrationBuilder.DropTable(
                name: "IssueTag");

          /*  migrationBuilder.DropIndex(
                name: "IX_IssueStatus_ProjectId",
                table: "IssueStatus");*/

            migrationBuilder.DropColumn(
                name: "IssueType",
                table: "Issues");

            migrationBuilder.RenameColumn(
                name: "IssueTagId",
                table: "Issues",
                newName: "IssueTypeId");

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
                name: "IX_IssueType_ProjectId",
                table: "IssueType",
                column: "ProjectId");
        }
    }
}
