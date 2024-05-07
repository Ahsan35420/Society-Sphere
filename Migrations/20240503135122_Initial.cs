using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocietySphere.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SocietyName",
                table: "Organizers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GPA",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Semester",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SocietyName",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GPA",
                table: "Leaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Semester",
                table: "Leaders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SocietyName",
                table: "Leaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SocietyName",
                table: "Organizers");

            migrationBuilder.DropColumn(
                name: "GPA",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Semester",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "SocietyName",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "GPA",
                table: "Leaders");

            migrationBuilder.DropColumn(
                name: "Semester",
                table: "Leaders");

            migrationBuilder.DropColumn(
                name: "SocietyName",
                table: "Leaders");
        }
    }
}
