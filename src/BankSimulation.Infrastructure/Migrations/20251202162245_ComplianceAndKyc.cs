using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSimulation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ComplianceAndKyc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "kvkk_consents",
                columns: table => new
                {
                    consent_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    consent_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    consent_given = table.Column<bool>(type: "bit", nullable: false),
                    consent_text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    consent_version = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    granted_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    revoked_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ip_address = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kvkk_consents", x => x.consent_id);
                    table.ForeignKey(
                        name: "FK_kvkk_consents_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "kvkk_data_requests",
                columns: table => new
                {
                    request_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    request_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    request_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    completed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    completed_by = table.Column<int>(type: "int", nullable: true),
                    response_data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    response_file_path = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kvkk_data_requests", x => x.request_id);
                    table.ForeignKey(
                        name: "FK_kvkk_data_requests_users_completed_by",
                        column: x => x.completed_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_kvkk_data_requests_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "kyc_documents",
                columns: table => new
                {
                    document_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    document_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    document_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    document_file_path = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    document_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    expiry_date = table.Column<DateTime>(type: "date", nullable: true),
                    upload_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    verified_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    verified_by = table.Column<int>(type: "int", nullable: true),
                    verification_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    rejection_reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kyc_documents", x => x.document_id);
                    table.ForeignKey(
                        name: "FK_kyc_documents_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_kyc_documents_users_verified_by",
                        column: x => x.verified_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "kyc_verifications",
                columns: table => new
                {
                    verification_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    verification_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    verification_method = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    verification_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    verification_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    verified_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    attempts = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kyc_verifications", x => x.verification_id);
                    table.ForeignKey(
                        name: "FK_kyc_verifications_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "masak_records",
                columns: table => new
                {
                    record_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    transaction_id = table.Column<int>(type: "int", nullable: true),
                    record_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    retention_until = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_masak_records", x => x.record_id);
                    table.ForeignKey(
                        name: "FK_masak_records_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "transactions",
                        principalColumn: "transaction_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_masak_records_users_customer_id",
                        column: x => x.customer_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "suspicious_activity_reports",
                columns: table => new
                {
                    sar_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    transaction_id = table.Column<int>(type: "int", nullable: true),
                    report_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    risk_score = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    created_by = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    reported_to_masak = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    masak_report_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    masak_reference_number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_suspicious_activity_reports", x => x.sar_id);
                    table.ForeignKey(
                        name: "FK_suspicious_activity_reports_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "transactions",
                        principalColumn: "transaction_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_suspicious_activity_reports_users_created_by",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_suspicious_activity_reports_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_kvkk_consents_user_id",
                table: "kvkk_consents",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_kvkk_data_requests_completed_by",
                table: "kvkk_data_requests",
                column: "completed_by");

            migrationBuilder.CreateIndex(
                name: "IX_kvkk_data_requests_user_id",
                table: "kvkk_data_requests",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_kyc_documents_user_id",
                table: "kyc_documents",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_kyc_documents_verified_by",
                table: "kyc_documents",
                column: "verified_by");

            migrationBuilder.CreateIndex(
                name: "IX_kyc_verifications_user_id",
                table: "kyc_verifications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_masak_records_customer_id",
                table: "masak_records",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_masak_records_transaction_id",
                table: "masak_records",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_suspicious_activity_reports_created_by",
                table: "suspicious_activity_reports",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_suspicious_activity_reports_transaction_id",
                table: "suspicious_activity_reports",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_suspicious_activity_reports_user_id",
                table: "suspicious_activity_reports",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "kvkk_consents");

            migrationBuilder.DropTable(
                name: "kvkk_data_requests");

            migrationBuilder.DropTable(
                name: "kyc_documents");

            migrationBuilder.DropTable(
                name: "kyc_verifications");

            migrationBuilder.DropTable(
                name: "masak_records");

            migrationBuilder.DropTable(
                name: "suspicious_activity_reports");
        }
    }
}
