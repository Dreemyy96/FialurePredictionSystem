using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FailurePredictionSystemBack.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPredictionProcessing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PredictionStatus",
                table: "Metrics",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "Predictions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    MetricId = table.Column<Guid>(type: "uuid", nullable: false),
                    PredictedState = table.Column<int>(type: "integer", nullable: false),
                    NormalProbability = table.Column<double>(type: "double precision", nullable: false),
                    WarningProbability = table.Column<double>(type: "double precision", nullable: false),
                    CriticalProbability = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Predictions_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Predictions_Metrics_MetricId",
                        column: x => x.MetricId,
                        principalTable: "Metrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Metrics_PredictionStatus",
                table: "Metrics",
                column: "PredictionStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_EquipmentId",
                table: "Predictions",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_MetricId",
                table: "Predictions",
                column: "MetricId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Predictions");

            migrationBuilder.DropIndex(
                name: "IX_Metrics_PredictionStatus",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "PredictionStatus",
                table: "Metrics");
        }
    }
}
