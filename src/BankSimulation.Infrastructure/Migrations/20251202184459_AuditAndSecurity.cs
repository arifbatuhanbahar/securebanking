using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSimulation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AuditAndSecurity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "audit_logs",
                columns: table => new
                {
                    log_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    table_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    record_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    action = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    old_value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    new_value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    changed_fields = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    changed_by = table.Column<int>(type: "int", nullable: true),
                    ip_address = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    timestamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_logs", x => x.log_id);
                });

            migrationBuilder.CreateTable(
                name: "data_access_log",
                columns: table => new
                {
                    log_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accessed_by_user_id = table.Column<int>(type: "int", nullable: false),
                    target_user_id = table.Column<int>(type: "int", nullable: false),
                    data_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    access_reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    access_timestamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ip_address = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    is_sensitive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_access_log", x => x.log_id);
                    table.ForeignKey(
                        name: "FK_data_access_log_users_accessed_by_user_id",
                        column: x => x.accessed_by_user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_data_access_log_users_target_user_id",
                        column: x => x.target_user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "encryption_keys",
                columns: table => new
                {
                    key_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    key_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    key_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    key_value_encrypted = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    expires_at = table.Column<DateTime>(type: "date", nullable: false),
                    rotation_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_encryption_keys", x => x.key_id);
                });

            migrationBuilder.CreateTable(
                name: "pci_audit_log",
                columns: table => new
                {
                    log_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    card_token = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ip_address = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    success = table.Column<bool>(type: "bit", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pci_audit_log", x => x.log_id);
                    table.ForeignKey(
                        name: "FK_pci_audit_log_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "security_events",
                columns: table => new
                {
                    event_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    event_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    severity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ip_address = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    user_agent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    event_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    resolved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    resolved_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    resolved_by = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_security_events", x => x.event_id);
                    table.ForeignKey(
                        name: "FK_security_events_users_resolved_by",
                        column: x => x.resolved_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_security_events_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_data_access_log_accessed_by_user_id",
                table: "data_access_log",
                column: "accessed_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_data_access_log_target_user_id",
                table: "data_access_log",
                column: "target_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_pci_audit_log_user_id",
                table: "pci_audit_log",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_security_events_resolved_by",
                table: "security_events",
                column: "resolved_by");

            migrationBuilder.CreateIndex(
                name: "IX_security_events_user_id",
                table: "security_events",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_logs");

            migrationBuilder.DropTable(
                name: "data_access_log");

            migrationBuilder.DropTable(
                name: "encryption_keys");

            migrationBuilder.DropTable(
                name: "pci_audit_log");

            migrationBuilder.DropTable(
                name: "security_events");
        }
    }
}
