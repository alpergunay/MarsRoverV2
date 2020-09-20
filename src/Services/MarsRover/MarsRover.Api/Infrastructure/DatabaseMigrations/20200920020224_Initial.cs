using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarsRover.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "marsrover");

            migrationBuilder.CreateTable(
                name: "commands",
                schema: "marsrover",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Creator = table.Column<string>(nullable: true),
                    Modifier = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(maxLength: 200, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "directions",
                schema: "marsrover",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Creator = table.Column<string>(nullable: true),
                    Modifier = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(maxLength: 200, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_directions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "plateaus",
                schema: "marsrover",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Modifier = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    XCoordinate = table.Column<int>(nullable: false),
                    YCoordinate = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plateaus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rovers",
                schema: "marsrover",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Modifier = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    DirectionId = table.Column<int>(nullable: false),
                    PlateauId = table.Column<Guid>(nullable: false),
                    XCoordinate = table.Column<int>(nullable: false),
                    YCoordinate = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rovers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rovers_directions_DirectionId",
                        column: x => x.DirectionId,
                        principalSchema: "marsrover",
                        principalTable: "directions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rovers_plateaus_PlateauId",
                        column: x => x.PlateauId,
                        principalSchema: "marsrover",
                        principalTable: "plateaus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_rovers_DirectionId",
                schema: "marsrover",
                table: "rovers",
                column: "DirectionId");

            migrationBuilder.CreateIndex(
                name: "IX_rovers_PlateauId",
                schema: "marsrover",
                table: "rovers",
                column: "PlateauId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "commands",
                schema: "marsrover");

            migrationBuilder.DropTable(
                name: "rovers",
                schema: "marsrover");

            migrationBuilder.DropTable(
                name: "directions",
                schema: "marsrover");

            migrationBuilder.DropTable(
                name: "plateaus",
                schema: "marsrover");
        }
    }
}
