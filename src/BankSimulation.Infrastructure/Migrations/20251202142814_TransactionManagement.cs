using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSimulation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TransactionManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "scheduled_transactions",
                columns: table => new
                {
                    schedule_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    from_account_id = table.Column<int>(type: "int", nullable: false),
                    to_account_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    frequency = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: true),
                    next_execution_date = table.Column<DateTime>(type: "date", nullable: false),
                    last_execution_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scheduled_transactions", x => x.schedule_id);
                    table.ForeignKey(
                        name: "FK_scheduled_transactions_accounts_from_account_id",
                        column: x => x.from_account_id,
                        principalTable: "accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_scheduled_transactions_accounts_to_account_id",
                        column: x => x.to_account_id,
                        principalTable: "accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_scheduled_transactions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transaction_types",
                columns: table => new
                {
                    type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    type_code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    fee_fixed = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    fee_percentage = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_types", x => x.type_id);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    transaction_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    from_account_id = table.Column<int>(type: "int", nullable: true),
                    to_account_id = table.Column<int>(type: "int", nullable: true),
                    amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    transaction_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    reference_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    fraud_score = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    fraud_flags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    requires_review = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    reviewed_by = table.Column<int>(type: "int", nullable: true),
                    reviewed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    transaction_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    completed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    ip_address = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    user_agent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    is_suspicious = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    suspicious_reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    reported_to_masak = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    masak_report_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.transaction_id);
                    table.ForeignKey(
                        name: "FK_transactions_accounts_from_account_id",
                        column: x => x.from_account_id,
                        principalTable: "accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transactions_accounts_to_account_id",
                        column: x => x.to_account_id,
                        principalTable: "accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transactions_users_created_by",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_transactions_users_reviewed_by",
                        column: x => x.reviewed_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "general_ledger",
                columns: table => new
                {
                    ledger_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    transaction_id = table.Column<int>(type: "int", nullable: false),
                    account_id = table.Column<int>(type: "int", nullable: false),
                    debit_amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    credit_amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    entry_type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    entry_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_general_ledger", x => x.ledger_id);
                    table.ForeignKey(
                        name: "FK_general_ledger_accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "account_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_general_ledger_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "transactions",
                        principalColumn: "transaction_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transaction_approvals",
                columns: table => new
                {
                    approval_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    transaction_id = table.Column<int>(type: "int", nullable: false),
                    approver_id = table.Column<int>(type: "int", nullable: false),
                    approval_level = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    comments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    approved_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_approvals", x => x.approval_id);
                    table.ForeignKey(
                        name: "FK_transaction_approvals_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "transactions",
                        principalColumn: "transaction_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transaction_approvals_users_approver_id",
                        column: x => x.approver_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transaction_fees",
                columns: table => new
                {
                    fee_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    transaction_id = table.Column<int>(type: "int", nullable: false),
                    fee_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fee_amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    applied_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_fees", x => x.fee_id);
                    table.ForeignKey(
                        name: "FK_transaction_fees_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "transactions",
                        principalColumn: "transaction_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_general_ledger_account_id",
                table: "general_ledger",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_general_ledger_transaction_id",
                table: "general_ledger",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_scheduled_transactions_from_account_id",
                table: "scheduled_transactions",
                column: "from_account_id");

            migrationBuilder.CreateIndex(
                name: "IX_scheduled_transactions_to_account_id",
                table: "scheduled_transactions",
                column: "to_account_id");

            migrationBuilder.CreateIndex(
                name: "IX_scheduled_transactions_user_id",
                table: "scheduled_transactions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_approvals_approver_id",
                table: "transaction_approvals",
                column: "approver_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_approvals_transaction_id",
                table: "transaction_approvals",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_fees_transaction_id",
                table: "transaction_fees",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_created_by",
                table: "transactions",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_date",
                table: "transactions",
                column: "transaction_date");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_from_date",
                table: "transactions",
                columns: new[] { "from_account_id", "transaction_date" });

            migrationBuilder.CreateIndex(
                name: "IX_transactions_reference_number",
                table: "transactions",
                column: "reference_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transactions_reviewed_by",
                table: "transactions",
                column: "reviewed_by");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_to_date",
                table: "transactions",
                columns: new[] { "to_account_id", "transaction_date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "general_ledger");

            migrationBuilder.DropTable(
                name: "scheduled_transactions");

            migrationBuilder.DropTable(
                name: "transaction_approvals");

            migrationBuilder.DropTable(
                name: "transaction_fees");

            migrationBuilder.DropTable(
                name: "transaction_types");

            migrationBuilder.DropTable(
                name: "transactions");
        }
    }
}
