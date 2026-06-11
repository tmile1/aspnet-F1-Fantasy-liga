using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace F1_Fantasy_liga.Migrations
{
    public partial class AddConstructorImagePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Constructors",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: string.Empty);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Constructors");
        }
    }
}
