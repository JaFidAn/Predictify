using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prediction.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovingForecastEvaluationActualOutcomeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualOutcomes",
                table: "ForecastEvaluations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActualOutcomes",
                table: "ForecastEvaluations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
