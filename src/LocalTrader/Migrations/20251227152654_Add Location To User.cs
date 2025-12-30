using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalTrader.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (!migrationBuilder.IsNpgsql())
            {
                throw new Exception("This can only run on Npgsql postgres");
            }

            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS cube;");
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS earthdistance;");
            
            migrationBuilder.AddColumn<long>(
                name: "Location_Langitude",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Location_Latitude",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location_Langitude",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Location_Latitude",
                table: "AspNetUsers");
            
            migrationBuilder.Sql("DROP EXTENSION IF NOT EXISTS earthdistance;");
            migrationBuilder.Sql("DROP EXTENSION IF NOT EXISTS cube;");
        }
    }
}
