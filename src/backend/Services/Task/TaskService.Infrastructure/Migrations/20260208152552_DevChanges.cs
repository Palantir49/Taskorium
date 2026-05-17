using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DevChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_IssueStatus_ProjectId",
                table: "IssueStatus",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_IssueStatus_Projects_ProjectId",
                table: "IssueStatus",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssueStatus_Projects_ProjectId",
                table: "IssueStatus");

            migrationBuilder.DropIndex(
                name: "IX_IssueStatus_ProjectId",
                table: "IssueStatus");
        }
    }
}
