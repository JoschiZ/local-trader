using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LocalTrader.Migrations
{
    /// <inheritdoc />
    public partial class SchemaFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionRadius",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Location_Langitude",
                table: "AspNetUsers",
                newName: "Location_Longitude");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "Magic",
                table: "WantLists",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location_Longitude",
                table: "AspNetUsers",
                newName: "Location_Langitude");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "Magic",
                table: "WantLists",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<decimal>(
                name: "ActionRadius",
                table: "AspNetUsers",
                type: "numeric",
                nullable: true);
        }
    }
}
