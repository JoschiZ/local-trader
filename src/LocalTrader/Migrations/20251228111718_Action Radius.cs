using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalTrader.Migrations
{
    /// <inheritdoc />
    public partial class ActionRadius : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ActionRadius",
                table: "AspNetUsers",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Location_Radius",
                table: "AspNetUsers",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionRadius",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Location_Radius",
                table: "AspNetUsers");
        }
    }
}
