using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DEVAMEET_CSharp.Migrations
{
    /// <inheritdoc />
    public partial class MeetObjectWalk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Walkable",
                table: "MeetObjects",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Walkable",
                table: "MeetObjects");
        }
    }
}
