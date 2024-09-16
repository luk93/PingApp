using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PingApp.Migrations
{
    /// <inheritdoc />
    public partial class connectioStringChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    IpAddress = table.Column<string>(type: "TEXT", nullable: true),
                    IpString = table.Column<string>(type: "TEXT", nullable: true),
                    LastIpStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    LastReplyDt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PingResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IpStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    ReplyDt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RoundTripTime = table.Column<long>(type: "INTEGER", nullable: true),
                    TimeToLive = table.Column<int>(type: "INTEGER", nullable: true),
                    BufferSizeSent = table.Column<int>(type: "INTEGER", nullable: true),
                    BufferSizeReceived = table.Column<int>(type: "INTEGER", nullable: true),
                    DeviceId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PingResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PingResults_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PingResults_DeviceId",
                table: "PingResults",
                column: "DeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PingResults");

            migrationBuilder.DropTable(
                name: "Devices");
        }
    }
}
