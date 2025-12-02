using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSimulation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FraudAndRisk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "fraud_alerts",
                columns: table => new
                {
                    alert_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    transaction_id = table.Column<int>(type: "int", nullable: false),
                    fraud_score = table.Column<int>(type: "int", nullable: false),
                    triggered_rules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    alert_severity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    reviewed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    reviewed_by = table.Column<int>(type: "int", nullable: true),
                    resolution_notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fraud_alerts", x => x.alert_id);
                    table.ForeignKey(
                        name: "FK_fraud_alerts_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "transactions",
                        principalColumn: "transaction_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_fraud_alerts_users_reviewed_by",
                        column: x => x.reviewed_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_fraud_alerts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "fraud_rules",
                columns: table => new
                {
                    rule_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rule_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    rule_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    rule_description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    rule_conditions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    risk_score_weight = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fraud_rules", x => x.rule_id);
                });

            migrationBuilder.CreateTable(
                name: "risk_profiles",
                columns: table => new
                {
                    profile_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    risk_level = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    transaction_velocity_score = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    amount_anomaly_score = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    geographic_risk_score = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    behavioral_score = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    last_calculated_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    factors = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_risk_profiles", x => x.profile_id);
                    table.ForeignKey(
                        name: "FK_risk_profiles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_fraud_alerts_reviewed_by",
                table: "fraud_alerts",
                column: "reviewed_by");

            migrationBuilder.CreateIndex(
                name: "IX_fraud_alerts_transaction_id",
                table: "fraud_alerts",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_fraud_alerts_user_id",
                table: "fraud_alerts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_risk_profiles_user_id",
                table: "risk_profiles",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fraud_alerts");

            migrationBuilder.DropTable(
                name: "fraud_rules");

            migrationBuilder.DropTable(
                name: "risk_profiles");
        }
    }
}
