using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Real_time_Chat_Application.Migrations
{
    /// <inheritdoc />
    public partial class MakeSentimentAnalysisResultNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_YourEntities",
                table: "YourEntities");

            migrationBuilder.RenameTable(
                name: "YourEntities",
                newName: "Messages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "YourEntities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_YourEntities",
                table: "YourEntities",
                column: "Id");
        }
    }
}
