using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSimulation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_UserManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tc_kimlik_no = table.Column<string>(type: "nchar(11)", fixedLength: true, maxLength: 11, nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "date", nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    email_verified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    phone_verified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    address_line1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    address_line2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    city = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    postal_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    country = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false, defaultValue: "TR"),
                    password_hash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    password_salt = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    password_changed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Active"),
                    failed_login_attempts = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    locked_until = table.Column<DateTime>(type: "datetime2", nullable: true),
                    kyc_status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    risk_level = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "Low"),
                    is_pep = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_login_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    gdpr_anonymized = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "login_attempts",
                columns: table => new
                {
                    attempt_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    email_attempted = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ip_address = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    success = table.Column<bool>(type: "bit", nullable: false),
                    failure_reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    attempted_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    user_agent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_login_attempts", x => x.attempt_id);
                    table.ForeignKey(
                        name: "FK_login_attempts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "password_history",
                columns: table => new
                {
                    history_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    changed_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    changed_by = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_password_history", x => x.history_id);
                    table.ForeignKey(
                        name: "FK_password_history_users_changed_by",
                        column: x => x.changed_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_password_history_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    role_name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    assigned_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    assigned_by = table.Column<int>(type: "int", nullable: true),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => x.role_id);
                    table.ForeignKey(
                        name: "FK_user_roles_users_assigned_by",
                        column: x => x.assigned_by,
                        principalTable: "users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "user_sessions",
                columns: table => new
                {
                    session_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    session_token = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ip_address = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    user_agent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    device_info = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    last_activity_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_sessions", x => x.session_id);
                    table.ForeignKey(
                        name: "FK_user_sessions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_login_attempts_email_time",
                table: "login_attempts",
                columns: new[] { "email_attempted", "attempted_at" });

            migrationBuilder.CreateIndex(
                name: "IX_login_attempts_ip_time",
                table: "login_attempts",
                columns: new[] { "ip_address", "attempted_at" });

            migrationBuilder.CreateIndex(
                name: "IX_login_attempts_user_id",
                table: "login_attempts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_password_history_changed_by",
                table: "password_history",
                column: "changed_by");

            migrationBuilder.CreateIndex(
                name: "IX_password_history_user_date",
                table: "password_history",
                columns: new[] { "user_id", "changed_at" });

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_assigned_by",
                table: "user_roles",
                column: "assigned_by");

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_user_role_unique",
                table: "user_roles",
                columns: new[] { "user_id", "role_name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_sessions_token",
                table: "user_sessions",
                column: "session_token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_sessions_user_active",
                table: "user_sessions",
                columns: new[] { "user_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_status_kyc",
                table: "users",
                columns: new[] { "status", "kyc_status" });

            migrationBuilder.CreateIndex(
                name: "IX_users_tc_kimlik_no",
                table: "users",
                column: "tc_kimlik_no",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "login_attempts");

            migrationBuilder.DropTable(
                name: "password_history");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "user_sessions");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
