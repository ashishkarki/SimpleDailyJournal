using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleDailyJournal.Migrations
{
    /// <inheritdoc />
    public partial class AddSentimentMaxLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sentiment",
                table: "JournalEntries",
                type: "TEXT",
                maxLength: 10,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sentiment",
                table: "JournalEntries");
        }
    }
}
