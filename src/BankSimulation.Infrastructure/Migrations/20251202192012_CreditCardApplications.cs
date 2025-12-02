using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSimulation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreditCardApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "card_applications",
                columns: table => new
                {
                    application_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    card_type_requested = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    monthly_income = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    employment_status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    employer_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    application_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    rejection_reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    approved_by = table.Column<int>(type: "int", nullable: true),
                    approved_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    credit_limit_approved = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_card_applications", x => x.application_id);
                    table.ForeignKey(
                        name: "FK_card_applications_users_approved_by",
                        column: x => x.approved_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_card_applications_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "card_limits",
                columns: table => new
                {
                    limit_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    card_id = table.Column<int>(type: "int", nullable: false),
                    limit_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    limit_amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    used_amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    reset_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_card_limits", x => x.limit_id);
                    table.ForeignKey(
                        name: "FK_card_limits_credit_cards_card_id",
                        column: x => x.card_id,
                        principalTable: "credit_cards",
                        principalColumn: "card_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_card_applications_approved_by",
                table: "card_applications",
                column: "approved_by");

            migrationBuilder.CreateIndex(
                name: "IX_card_applications_user_id",
                table: "card_applications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_card_limits_card_id",
                table: "card_limits",
                column: "card_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "card_applications");

            migrationBuilder.DropTable(
                name: "card_limits");
        }
    }
}
