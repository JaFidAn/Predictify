using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prediction.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangingForecastEvaluationActualOutcomeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualOutcomeId",
                table: "ForecastEvaluations");

            migrationBuilder.AddColumn<string>(
                name: "ActualOutcomes",
                table: "ForecastEvaluations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualOutcomes",
                table: "ForecastEvaluations");

            migrationBuilder.AddColumn<Guid>(
                name: "ActualOutcomeId",
                table: "ForecastEvaluations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
