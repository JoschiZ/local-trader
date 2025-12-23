using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LocalTrader.Data.Migrations
{
    /// <inheritdoc />
    public partial class WantLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllCollectionCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MagicCards",
                table: "MagicCards");

            migrationBuilder.EnsureSchema(
                name: "Account");

            migrationBuilder.EnsureSchema(
                name: "Card");

            migrationBuilder.RenameTable(
                name: "MagicCards",
                newName: "Magic",
                newSchema: "Card");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Magic",
                schema: "Card",
                table: "Magic",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Cards",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Condition = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    CardId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cards_Magic_CardId",
                        column: x => x.CardId,
                        principalSchema: "Card",
                        principalTable: "Magic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WantLists",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Accessibility = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WantLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WantLists_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WantedCards",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    WantListId = table.Column<int>(type: "integer", nullable: false),
                    MinimumCondition = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    CardId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WantedCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WantedCards_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WantedCards_Magic_CardId",
                        column: x => x.CardId,
                        principalSchema: "Card",
                        principalTable: "Magic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WantedCards_WantLists_WantListId",
                        column: x => x.WantListId,
                        principalSchema: "Account",
                        principalTable: "WantLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardId",
                schema: "Account",
                table: "Cards",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_UserId",
                schema: "Account",
                table: "Cards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_UserId_CardId_Condition",
                schema: "Account",
                table: "Cards",
                columns: new[] { "UserId", "CardId", "Condition" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WantedCards_CardId",
                schema: "Account",
                table: "WantedCards",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_WantedCards_UserId",
                schema: "Account",
                table: "WantedCards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WantedCards_UserId_CardId_WantListId",
                schema: "Account",
                table: "WantedCards",
                columns: new[] { "UserId", "CardId", "WantListId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WantedCards_WantListId",
                schema: "Account",
                table: "WantedCards",
                column: "WantListId");

            migrationBuilder.CreateIndex(
                name: "IX_WantLists_UserId",
                schema: "Account",
                table: "WantLists",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cards",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "WantedCards",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "WantLists",
                schema: "Account");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Magic",
                schema: "Card",
                table: "Magic");

            migrationBuilder.RenameTable(
                name: "Magic",
                schema: "Card",
                newName: "MagicCards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MagicCards",
                table: "MagicCards",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AllCollectionCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Condition = table.Column<int>(type: "integer", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CardId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllCollectionCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllCollectionCards_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AllCollectionCards_MagicCards_CardId",
                        column: x => x.CardId,
                        principalTable: "MagicCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllCollectionCards_CardId",
                table: "AllCollectionCards",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_AllCollectionCards_UserId",
                table: "AllCollectionCards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AllCollectionCards_UserId_CardId_Condition",
                table: "AllCollectionCards",
                columns: new[] { "UserId", "CardId", "Condition" },
                unique: true);
        }
    }
}
