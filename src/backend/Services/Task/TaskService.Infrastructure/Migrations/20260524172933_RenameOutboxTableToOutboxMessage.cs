using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameOutboxTableToOutboxMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_outbox_messages",
                table: "outbox_messages");

            migrationBuilder.RenameTable(
                name: "outbox_messages",
                newName: "OutboxMessage");

            migrationBuilder.RenameIndex(
                name: "IX_outbox_messages_Status_OccurredOnUtc",
                table: "OutboxMessage",
                newName: "IX_OutboxMessage_Status_OccurredOnUtc");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboxMessage",
                table: "OutboxMessage",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboxMessage",
                table: "OutboxMessage");

            migrationBuilder.RenameTable(
                name: "OutboxMessage",
                newName: "outbox_messages");

            migrationBuilder.RenameIndex(
                name: "IX_OutboxMessage_Status_OccurredOnUtc",
                table: "outbox_messages",
                newName: "IX_outbox_messages_Status_OccurredOnUtc");

            migrationBuilder.AddPrimaryKey(
                name: "PK_outbox_messages",
                table: "outbox_messages",
                column: "Id");
        }
    }
}
