using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Real_time_Chat_Application.Migrations
{
    /// <inheritdoc />
    public partial class AddChatRoomToMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatRoom",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatRoom",
                table: "Messages");
        }
    }
}
