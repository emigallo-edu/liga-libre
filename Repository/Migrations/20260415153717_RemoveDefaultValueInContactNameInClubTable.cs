using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDefaultValueInContactNameInClubTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ContactName",
                schema: "dbo",
                table: "Clubs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "nombre temporal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ContactName",
                schema: "dbo",
                table: "Clubs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "nombre temporal",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
