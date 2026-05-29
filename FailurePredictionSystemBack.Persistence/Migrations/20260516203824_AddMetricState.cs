using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FailurePredictionSystemBack.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMetricState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Metrics",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Metrics");
        }
    }
}
