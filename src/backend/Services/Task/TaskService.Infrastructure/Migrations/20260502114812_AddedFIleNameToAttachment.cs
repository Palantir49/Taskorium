using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedFIleNameToAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Attachments",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Attachments");
        }
    }
}
