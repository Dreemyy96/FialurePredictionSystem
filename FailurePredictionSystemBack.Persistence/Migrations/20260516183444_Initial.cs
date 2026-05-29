using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FailurePredictionSystemBack.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentTokenHash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Hostname = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Metrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Hostname = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TimestampUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CpuUsagePercent = table.Column<double>(type: "double precision", nullable: false),
                    RamUsagePercent = table.Column<double>(type: "double precision", nullable: false),
                    DiskUsagePercent = table.Column<double>(type: "double precision", nullable: false),
                    FreeDiskSpaceGb = table.Column<double>(type: "double precision", nullable: false),
                    TemperatureCelsius = table.Column<double>(type: "double precision", nullable: false),
                    ErrorCount = table.Column<int>(type: "integer", nullable: false),
                    UptimeHours = table.Column<double>(type: "double precision", nullable: false),
                    ReceivedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Metrics_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_AgentId",
                table: "Equipments",
                column: "AgentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_AgentId",
                table: "Metrics",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_EquipmentId",
                table: "Metrics",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_TimestampUtc",
                table: "Metrics",
                column: "TimestampUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Metrics");

            migrationBuilder.DropTable(
                name: "Equipments");
        }
    }
}
