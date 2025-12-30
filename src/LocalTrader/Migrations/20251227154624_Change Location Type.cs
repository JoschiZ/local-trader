using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalTrader.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLocationType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Location_Latitude",
                table: "AspNetUsers",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Location_Langitude",
                table: "AspNetUsers",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Location_Latitude",
                table: "AspNetUsers",
                type: "real",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Location_Langitude",
                table: "AspNetUsers",
                type: "real",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);
        }
    }
}
