using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalTrader.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "CREATE INDEX user_location_idx ON \"AspNetUsers\" USING gist(ll_to_earth(\"Location_Latitude\", \"Location_Langitude\"))");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "DROP INDEX user_location_idx");
        }
    }
}
