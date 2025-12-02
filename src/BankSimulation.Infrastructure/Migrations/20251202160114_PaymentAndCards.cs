using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSimulation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PaymentAndCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "credit_cards",
                columns: table => new
                {
                    card_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    card_number_encrypted = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    card_last_four = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    card_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    card_brand = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    credit_limit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    available_limit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    current_balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    minimum_payment = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    payment_due_date = table.Column<DateTime>(type: "date", nullable: false),
                    interest_rate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    expiry_month = table.Column<int>(type: "int", nullable: false),
                    expiry_year = table.Column<int>(type: "int", nullable: false),
                    cvv_encrypted = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    issued_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    activated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_cards", x => x.card_id);
                    table.ForeignKey(
                        name: "FK_credit_cards_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "payment_gateways",
                columns: table => new
                {
                    gateway_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    gateway_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    gateway_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    api_endpoint = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    api_key_encrypted = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_gateways", x => x.gateway_id);
                });

            migrationBuilder.CreateTable(
                name: "payment_methods",
                columns: table => new
                {
                    payment_method_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    method_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    card_token = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    card_last_four = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    card_brand = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    expiry_month = table.Column<int>(type: "int", nullable: false),
                    expiry_year = table.Column<int>(type: "int", nullable: false),
                    cardholder_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    is_default = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    last_used_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_methods", x => x.payment_method_id);
                    table.ForeignKey(
                        name: "FK_payment_methods_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "card_transactions",
                columns: table => new
                {
                    card_transaction_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    card_id = table.Column<int>(type: "int", nullable: false),
                    merchant_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    merchant_category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    transaction_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    authorization_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ip_address = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_card_transactions", x => x.card_transaction_id);
                    table.ForeignKey(
                        name: "FK_card_transactions_credit_cards_card_id",
                        column: x => x.card_id,
                        principalTable: "credit_cards",
                        principalColumn: "card_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "recurring_payments",
                columns: table => new
                {
                    recurring_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    payment_method_id = table.Column<int>(type: "int", nullable: false),
                    merchant_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    frequency = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    next_payment_date = table.Column<DateTime>(type: "date", nullable: false),
                    last_payment_date = table.Column<DateTime>(type: "date", nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recurring_payments", x => x.recurring_id);
                    table.ForeignKey(
                        name: "FK_recurring_payments_payment_methods_payment_method_id",
                        column: x => x.payment_method_id,
                        principalTable: "payment_methods",
                        principalColumn: "payment_method_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_recurring_payments_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_card_transactions_card_id",
                table: "card_transactions",
                column: "card_id");

            migrationBuilder.CreateIndex(
                name: "IX_credit_cards_user_id",
                table: "credit_cards",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_methods_user_id",
                table: "payment_methods",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_recurring_payments_payment_method_id",
                table: "recurring_payments",
                column: "payment_method_id");

            migrationBuilder.CreateIndex(
                name: "IX_recurring_payments_user_id",
                table: "recurring_payments",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "card_transactions");

            migrationBuilder.DropTable(
                name: "payment_gateways");

            migrationBuilder.DropTable(
                name: "recurring_payments");

            migrationBuilder.DropTable(
                name: "credit_cards");

            migrationBuilder.DropTable(
                name: "payment_methods");
        }
    }
}
