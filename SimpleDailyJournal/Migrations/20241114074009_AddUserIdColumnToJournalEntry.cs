using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleDailyJournal.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdColumnToJournalEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "JournalEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "JournalEntries");
        }
    }
}
